using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float maxHeight;
    public float minHeight;

    public float maxLeft;
    public float maxTop;
    public float maxRight;
    public float maxBottom;

    public float moveSpeed;
    public float scrollSpeed;
	public float rotationSpeed;
	public float shiftMultiplier;

	public Vector3 gameStartPos;
	public Quaternion gameStartRot;

	public GameObject ui;

	public float lerpSpeed;
	private float lerpLength;
	private bool lerping;
	private Vector3 startPos;
	private Quaternion startRot;
	private bool done;

	public void LerpToStartPos(Vector3 sp, Quaternion sr)
	{
		this.startPos = sp;
		this.startRot = sr;
		lerping = true;
		currentTime = 0.05f;
		lerpLength = Vector3.Distance(startPos, gameStartPos);
		lerpRot = sr;
	}

	private void Start()
	{
		if(Settings.s.startWithMenu)
		{
			ui.gameObject.SetActive(false);
		}else{
			transform.parent.transform.position = gameStartPos;
			transform.parent.transform.rotation = gameStartRot;
			done = true;
		}
		maxShiftMultiplier = shiftMultiplier;
	}

    Vector3 pos = new Vector3(0, 0, 0);
	Quaternion lerpRot = new Quaternion(0, 0, 0, 1);
	float currentTime;
	float maxShiftMultiplier;
	void Update () 
	{
		if(lerping)
		{
			currentTime += 0.005f;
			float fracComplete = currentTime * lerpSpeed;
			float fracJourney = fracComplete / lerpLength;
			transform.parent.position = Vector3.Lerp(startPos, gameStartPos, fracJourney);
			if(lerpRot.x <= 0)
			{
				lerpRot.x += 0.00215f;
				transform.parent.rotation = lerpRot;
			}else{
				lerping = false;
				done = true;
				GameManager.gm.StartGame();
			}
		}
		if(done && !ui.gameObject.activeSelf)
		{
			ui.gameObject.SetActive(true);
		}
		if(done)
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				shiftMultiplier = maxShiftMultiplier / Time.timeScale;
			}else{
				shiftMultiplier = 1 / Time.timeScale;
			}
				if(transform.parent.position.z  <= maxTop && transform.parent.position.z >= maxBottom)
				{
					transform.parent.position += transform.parent.forward * ((moveSpeed * shiftMultiplier) * Input.GetAxis("Vertical") * Time.deltaTime);
				}else{
					if(transform.parent.position.z > maxTop)
					{
						transform.parent.Translate(0, 0, -(transform.parent.position.z - maxTop));
					}else if(transform.parent.position.z < maxBottom){
						transform.parent.Translate(0, 0, maxBottom - transform.parent.position.z);
					}
				}
				if(transform.parent.position.x  <= maxRight && transform.parent.position.x >= maxLeft)
				{
					transform.parent.position += transform.parent.right * ((moveSpeed * shiftMultiplier) * Input.GetAxis("Horizontal") * Time.deltaTime);
				}else{
					if(transform.parent.position.x > maxRight)
					{
						transform.parent.Translate(-(transform.parent.position.x - maxRight), 0, 0);
					}else if(transform.parent.position.x < maxLeft){
						transform.parent.Translate(maxLeft - transform.parent.position.x, 0, 0);
					}
				}

				if(transform.parent.position.y  <= maxHeight && transform.parent.position.y >= minHeight)
				{
					transform.parent.position += transform.parent.up * ((scrollSpeed * shiftMultiplier * 2) * -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);
				}else{
					if(transform.parent.position.y > maxHeight)
					{
						transform.parent.Translate(0, -(transform.parent.position.y - maxHeight) ,0);
					}else if(transform.parent.position.y < minHeight){
						transform.parent.Translate(0, minHeight - transform.parent.position.y, 0);
					}

			}
		}
	}
}
