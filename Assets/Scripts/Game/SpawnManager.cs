using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public GameObject[] enemies;
	public float baseTime;

	private GameObject[] spawnPoints;

	private void Start()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
		StartCoroutine(StartSpawning(0));
	}

	private IEnumerator StartSpawning(int enemyID)
	{
		while(true)
		{
			int amount = Random.Range(0, 4);
			yield return new WaitForSeconds(baseTime + amount);
			amount = Random.Range(0, 4);
			for(int i = 0; i < amount; i++)
			{
				if(!GameManager.paused)
				{
					GameObject e = Instantiate(enemies[enemyID], randomSpawnpoint(), Quaternion.identity) as GameObject;
					GameManager.gm.enemies.Add(e.transform.FindChild("Enemy Object").gameObject);
				}
			}
		}
	}

	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.F))
		{
			GameObject e = Instantiate(enemies[0], randomSpawnpoint(), Quaternion.identity) as GameObject;
			GameManager.gm.enemies.Add(e.transform.FindChild("Enemy Object").gameObject);
		}
	}

	private Vector3 randomSpawnpoint()
	{
		int i = Mathf.FloorToInt(Random.Range(0, spawnPoints.Length));
		return spawnPoints[i].transform.position;
	}
}

