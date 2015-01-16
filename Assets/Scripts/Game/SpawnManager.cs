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

	private int spawnedSoFar = 0;

	private IEnumerator StartSpawning(int enemyID)
	{
		while(true)
		{
			int amount = Random.Range(0, 4);
			yield return new WaitForSeconds(baseTime + amount);
			amount = Random.Range(0, 4);
			amount += (int) (spawnedSoFar / 85);
			for(int i = 0; i < amount; i++)
			{
				if(!GameManager.paused)
				{
					GameObject e;
					e = Instantiate(enemies[enemyID], randomSpawnpoint(), Quaternion.identity) as GameObject;
					e.transform.FindChild("Enemy Object").GetComponent<Enemy>().AddHealth(spawnedSoFar / 35);
					GameManager.gm.enemies.Add(e.transform.FindChild("Enemy Object").gameObject);
					spawnedSoFar++;
				}
			}
		}
	}

	private void Update()
	{
		//Debug only
		if(Input.GetKeyUp(KeyCode.F))
		{
			GameObject e = Instantiate(enemies[0], randomSpawnpoint(), Quaternion.identity) as GameObject;
			e.transform.FindChild("Enemy Object").GetComponent<Enemy>().AddHealth(spawnedSoFar / 35);
			GameManager.gm.enemies.Add(e.transform.FindChild("Enemy Object").gameObject);
		}
	}

	private Vector3 randomSpawnpoint()
	{
		int i = Mathf.FloorToInt(Random.Range(0, spawnPoints.Length));
		return spawnPoints[i].transform.position;
	}
}

