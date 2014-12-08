using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;

	public List<GameObject> towers;
	public GameObject hq;

	private void Awake()
	{
		gm = this;
		towers = new List<GameObject>();
		hq = GameObject.FindGameObjectWithTag("HQ");
	}
}

