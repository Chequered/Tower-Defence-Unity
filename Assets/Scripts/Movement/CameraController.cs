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
	private float startTime;
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
		startTime = Time.time;
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
		}
	}

    Vector3 pos = new Vector3(0, 0, 0);
	Quaternion lerpRot = new Quaternion(0, 0, 0, 1);
	void Update () 
	{
		if(lerping)
		{
			float fracComplete = (Time.time - startTime) * lerpSpeed;
			float fracJourney = fracComplete / lerpLength;
			transform.parent.position = Vector3.Lerp(startPos, gameStartPos, fracJourney);
			if(lerpRot.x <= 0)
			{
				transform.parent.rotation = lerpRot;
				lerpRot.x += 0.0085f;
			}else{
				lerping = false;
				done = true;
			}
		}
		if(done && !ui.gameObject.activeSelf)
		{
			ui.gameObject.SetActive(true);
		}
		if(Input.GetKey(KeyCode.LeftShift))
		{
			//left,right + forward,backwards
			transform.parent.position += transform.parent.forward * ((moveSpeed * shiftMultiplier) * Input.GetAxis("Vertical") * Time.deltaTime);
			transform.parent.position += transform.parent.right * ((moveSpeed * shiftMultiplier) * Input.GetAxis("Horizontal") * Time.deltaTime);

			//up + down
			transform.parent.position += transform.parent.up * ((scrollSpeed * shiftMultiplier * 2) * -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);

		}else{
			transform.parent.position += transform.parent.forward * (moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
			transform.parent.position += transform.parent.right * (moveSpeed  * Input.GetAxis("Horizontal") * Time.deltaTime);

			if(transform.parent.position.y  <= maxHeight && transform.parent.position.y >= minHeight)
			{
				transform.parent.position += transform.parent.up * (scrollSpeed  * -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);
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
