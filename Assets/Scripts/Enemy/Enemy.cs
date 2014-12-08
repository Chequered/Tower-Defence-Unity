using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public const float COOLDOWN_SEEK = 1;

	[SerializeField] protected float movementSpeed;
	[SerializeField] protected float attackRange;
	[SerializeField] protected int health;

	protected GameObject target;
	
	private float seekCoolDown = COOLDOWN_SEEK;

	bool targetReached;
	Vector3 posToAdd = new Vector3(0, 0, 0);
	private void Update()
	{
		if(target != null)
		{
			if(Vector3.Distance(this.gameObject.transform.position, target.transform.position) > attackRange && !targetReached)
			{
				posToAdd = transform.forward * movementSpeed * Time.deltaTime;
				posToAdd.y = 0;
				transform.position += posToAdd;
				GetComponent<LineRenderer>().SetPosition(0, transform.position);
			}else{
				targetReached = true;
			}
		}else if (!targetReached){
			SeekNewTarget();
		}

		if(seekCoolDown < Time.time)
		{
			SeekNewTarget();
		}

	}

	private void SeekNewTarget()
	{

		float bestDistance = Mathf.Infinity;
		for(int i = 0; i < GameManager.gm.towers.Count; i++)
		{
			if(Vector3.Distance(this.gameObject.transform.position, GameManager.gm.towers[i].transform.position) < bestDistance)
			{
				bestDistance = Vector3.Distance(this.gameObject.transform.position, GameManager.gm.towers[i].transform.position);
				if(target != GameManager.gm.towers[i])
				{
					target = GameManager.gm.towers[i];
					transform.LookAt(target.transform.position);
				}
			}
		}
		
		if(Vector3.Distance(this.gameObject.transform.position, GameManager.gm.hq.transform.position) < bestDistance)
		{
			target = GameManager.gm.hq;
			transform.LookAt(target.transform.position);
		}
		seekCoolDown = Time.time + COOLDOWN_SEEK;
		GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
		targetReached = false;
	}

	private void AttackHQ()
	{
		target = GameManager.gm.hq;
		transform.LookAt(target.transform.position);		
	}

	public int GetHealth()
	{
		return this.health;
	}
}

