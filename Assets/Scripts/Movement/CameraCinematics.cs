using UnityEngine;
using System.Collections;

public class CameraCinematics : MonoBehaviour
{

	public void StartLerp()
	{
		transform.FindChild("Main Camera").GetComponent<CameraController>().LerpToStartPos(transform.position, transform.rotation);
		GameManager.gm.StartGame();
	}

}

