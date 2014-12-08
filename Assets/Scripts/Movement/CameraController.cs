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

	// Use this for initialization
	void Start () {
	
	}

    Vector3 pos = new Vector3(0, 0, 0);
	void Update () {
        pos.x = Input.GetAxis("Horizontal") * moveSpeed;
        pos.z = Input.GetAxis("Vertical") * moveSpeed;
        pos.y = -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        transform.position += pos;
	}
}
