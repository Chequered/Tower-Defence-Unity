using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public const float COOLDOWN_SEEK = 1;

	public GameObject uiElement;
	
	[SerializeField] protected int goldWorth;
	[SerializeField] protected int damage;
	[SerializeField] protected float attackCooldown;
	[SerializeField] protected float movementSpeed;
	[SerializeField] protected float attackRange;
	[SerializeField] protected int health;

	protected GameObject target;

	private void Start()
	{
		this.health *= Settings.difficulty;
		ui = uiElement.GetComponent<UIElement>();
		ui.UpdateElement(this.gameObject);
	}

	UIElement ui;
	float seekCoolDown = COOLDOWN_SEEK;
	bool targetReached;
	Vector3 posToAdd = new Vector3(0, 0, 0);
	Vector3 lookPos = new Vector3(0, 0, 0);
	private void Update()
	{
		if(!GameManager.paused)
		{
			if(target != null)
			{
				if(Vector3.Distance(this.gameObject.transform.position, target.transform.position) > attackRange && !targetReached)
				{
					posToAdd = transform.forward * movementSpeed * Time.deltaTime;
					posToAdd.y = 0;
					transform.position += posToAdd;
					lookPos = target.transform.position;
					lookPos.y = transform.position.y;
					transform.LookAt(lookPos);
					uiElement.transform.position += posToAdd;
				}else{
					targetReached = true;
				}
				if(targetReached)
				{
					Attack ();
				}
			}else if (!targetReached){
				SeekNewTarget();
			}
			if(seekCoolDown < Time.time)
			{
				SeekNewTarget();
			}
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
				}
			}
		}
		
		if(Vector3.Distance(this.gameObject.transform.position, GameManager.gm.hq.transform.position) < bestDistance)
		{
			target = GameManager.gm.hq;
			transform.LookAt(target.transform.position);
		}
		seekCoolDown = Time.time + COOLDOWN_SEEK;	
		targetReached = false;
	}

	public bool OnAttack(int damage)
	{
		this.health -= damage;
		ui.UpdateElement(this.gameObject);
		if(this.health <= 0)
		{
			GameManager.gm.RemoveEnemy(this.gameObject);
			GameManager.gm.gold.AddAmount(goldWorth);
			return true;
		}
		return false;
	}

	float timeSinceLastAttack;
	private void Attack()
	{
		if(timeSinceLastAttack < Time.time)
		{
			timeSinceLastAttack = Time.time + attackCooldown;
			if(target.GetComponent<Tower>())
			{
				if(target.GetComponent<Tower>().OnAttack(damage))
				{
					target.GetComponent<Tower>().Kill();
					SeekNewTarget();
				}
			}else{
				if(target.GetComponent<ResourceBuilding>().OnAttack(damage))
				{
					target.GetComponent<ResourceBuilding>().Kill();
					SeekNewTarget();
				}
			}
		}
	}

	private void AttackHQ()
	{
		target = GameManager.gm.hq;	
	}

	public int GetHealth()
	{
		return this.health;
	}
}

