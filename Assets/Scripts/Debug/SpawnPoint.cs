using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 1);
	}
}
