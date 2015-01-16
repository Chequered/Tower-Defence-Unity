using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MortarExplosion : MonoBehaviour {

	public float explosionRadius;

	private int dmg;

	public void GiveStats(int dmg)
	{
		this.dmg = dmg;
	}

	private void Start()
	{
		List<GameObject> enemiesInRange = new List<GameObject>();
		foreach(GameObject enem in GameManager.gm.enemies)
		{
			if(Vector3.Distance(transform.position, enem.transform.position) < explosionRadius)
			{
				enemiesInRange.Add(enem);
			}
		}
		for(int i = 0; i < enemiesInRange.Count; i++)
		{
			if(enemiesInRange[i].GetComponent<Enemy>())
			{
				enemiesInRange[i].GetComponent<Enemy>().OnAttack(dmg);
			}
		}
		Destroy(this.gameObject, 55);
	}
}
