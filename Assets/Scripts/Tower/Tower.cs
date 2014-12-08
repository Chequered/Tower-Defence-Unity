using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    private Gun gun;
	private List<GameObject> enemiesInRange = new List<GameObject>();

	public float shootCooldown;

    private void Start()
    {
        gun = transform.FindChild("Gun").GetComponent<Gun>();
    }

	float timeSinceLastShot;
	private void Update()
	{
		if(timeSinceLastShot < Time.time)
		{
			if(enemiesInRange.Count >= 1)
			{
				Shoot();
			}
		}
	}

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.tag == "Enemy")
        {
            enemiesInRange.Add(coll.gameObject);
            gun.SetTarget(coll.gameObject);
			enemiesInRange.Sort(SortByHealth);
        }
    }

	private void Shoot()
	{
		gun.particleSystem.Emit(45);
		timeSinceLastShot = Time.time + shootCooldown;
	}

	private int SortByHealth(GameObject g1, GameObject g2)
	{
		return g1.GetComponent<Enemy>().GetHealth().CompareTo(g2.GetComponent<Enemy>().GetHealth());
	}
}