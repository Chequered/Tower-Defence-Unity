using UnityEngine;
using System.Collections;

public class MortarExplosion : MonoBehaviour {

	public float explosionRadius;

	private int dmg;

	public void GiveStats(float dmg)
	{
		this.dmg = dmg;
	}

	private void Start()
	{
		GameObject[] enemiesInRange = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach(GameObject enem in enemiesInRange)
		{
			enem.GetComponent<Enemy>().OnAttack(
		}
	}
}
