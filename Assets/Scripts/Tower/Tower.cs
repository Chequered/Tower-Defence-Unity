using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

	[SerializeField] protected string name;
	[SerializeField] protected string description;
	[SerializeField] protected int hp;
					 protected int maxHp;
	[SerializeField] protected BuildMode buildmode;
	[SerializeField] protected int damage;
	[SerializeField] protected float attackRange;
	[SerializeField] protected float shootCooldown;
	[SerializeField] protected bool canShoot;
	[SerializeField] protected GameObject menuObj;
	[SerializeField] protected int baseUpgradeCost;
	[SerializeField] protected GameObject rangeCyl;

	[SerializeField] protected Gun gun;
	protected List<GameObject> enemiesInRange = new List<GameObject>();
	protected List<GameObject> removeQueue = new List<GameObject>();

	protected bool build;
	protected TowerMenu menu;

	protected GameObject towerLinked;

	Vector3 rangeScale = new Vector3(0,0,0);
    private void Start()
    {

		maxHp = hp;

		damageCost = baseUpgradeCost;
		speedCost = baseUpgradeCost;
		rangeCost = baseUpgradeCost;
		hpCost = baseUpgradeCost;

		rangeScale = rangeCyl.transform.localScale;
		rangeScale.x = attackRange * 2f;
		rangeScale.z = attackRange * 2f;
		rangeCyl.transform.localScale = rangeScale;

		menu = menuObj.GetComponent<TowerMenu>();
		menu.UpdateText(menu.damage, "Damage: " + this.damage);
		menu.UpdateText(menu.health, "Health: " + this.hp);
		menu.UpdateText(menu.range, "Range: " + this.attackRange);
		menu.UpdateText(menu.speed, "Shots/Min: " +  60 / this.shootCooldown);
		menu.UpdateText(menu.name, this.name);
		menu.UpdateText(menu.description, this.description);

		menu.UpdateText(menu.damageU,"Upgrade: " + damageCost + "g");
		menu.UpdateText(menu.speedU,"Upgrade: " + speedCost + "g");
		menu.UpdateText(menu.healthU,"Upgrade: " + hpCost + "g");
		menu.UpdateText(menu.rangeU,"Upgrade: " + rangeCost + "g");

		menu.SetTower(this);
    }

	float timeSinceLastShot;
	private void Update()
	{
		if(!GameManager.paused)
		{
			CleanUpEnemiesInRange();
			if(build)
			{
				if(canShoot)
				{
					if(timeSinceLastShot < Time.time)
					{
						if(enemiesInRange.Count >= 1)
						{
							removeQueue.Clear();
							ReselectTarget();
							Shoot();
						}
					}
					foreach(GameObject g in GameManager.gm.enemies)
					{
						if(!enemiesInRange.Contains(g))
						{
							if(Vector3.Distance(transform.position, g.transform.position) < attackRange)
							{
								enemiesInRange.Add(g);
								ReselectTarget();
							}
						}
					}
				}
				if(Input.GetMouseButtonUp(0))
				{
					if(!hovering)
					{
						//Deselect();
					}
				}
				if(towerLinked == null)
				{

					bool foundTower = false;
					foreach(GameObject tower in GameManager.gm.towers)
					{
						if(!foundTower)
						{
							if(tower.transform.position != transform.position)
							{
								if(Vector3.Distance(transform.position, tower.transform.position) <= attackRange)
								{
									towerLinked = tower;
									foundTower = true;
									GetComponent<LineRenderer>().SetPosition(1, tower.transform.position);
								}
							}
						}
					}
					if(!foundTower)
					{
						Kill();
					}
				}
			}
		}
	}

	public void Shoot()
	{
		if(!Settings.disableParticles)
		{
			gun.particleSystem.Emit(2);
		}
		audio.pitch = Random.Range(0.8f, 1.3f);
		audio.Play();
		timeSinceLastShot = Time.time + shootCooldown;
		if(buildmode == BuildMode.MortarTower)
		{
			GameObject expl = Instantiate(Buildmanager.BM.mortarExplosion, enemiesInRange[0].transform.position, Quaternion.identity) as GameObject;
			expl.GetComponent<MortarExplosion>().GiveStats(damage);
		}else{
			if(enemiesInRange[0].GetComponent<Enemy>().OnAttack(damage))
			{
				enemiesInRange.Remove(enemiesInRange[0]);
				ReselectTarget();
			}
		}
	}

	public int SortByHealth(GameObject g1, GameObject g2)
	{
		return g1.GetComponent<Enemy>().GetHealth().CompareTo(g2.GetComponent<Enemy>().GetHealth());
	}

	private void ReselectTarget()
	{
		if(enemiesInRange.Count >= 1)
		{
			enemiesInRange.Sort(SortByHealth);
			gun.SetTarget(enemiesInRange[0]);
		}
	}

	private void CleanUpEnemiesInRange()
	{
		foreach(GameObject e in enemiesInRange)
		{
			if(e == null)
			{
				removeQueue.Add(e);
			}
		}
		foreach(GameObject g in removeQueue)
		{
			enemiesInRange.Remove(g);
		}
	}

	public void Build(GameObject lTarget)
	{
		this.build = true;
		this.collider.enabled = true;
		GetComponent<LineRenderer>().SetPosition(0, transform.position);
		GetComponent<LineRenderer>().SetPosition(1, lTarget.transform.position);
		GetComponent<LineRenderer>().enabled = true;
		towerLinked = lTarget;
	}

	int damageCost;
	int speedCost;
	int hpCost;
	int rangeCost;
	public void Upgrade(string type)
	{
		if(type == "Damage")
		{
			if(GameManager.gm.gold.GetAmount() >= damageCost)
			{
				GameManager.gm.gold.DeductAmount(damageCost);
				this.damage = damage / 2 + damage;
				damageCost = damageCost / 2 + damageCost;
				menu.UpdateText(menu.damage, "Damage: " + this.damage);
				menu.UpdateText(menu.damageU, "Upgrade: " + damageCost + "g"); 
			}
		}
		if(type == "Health")
		{
			if(GameManager.gm.gold.GetAmount() >= hpCost)
			{
				GameManager.gm.gold.DeductAmount(hpCost);
				this.hp += maxHp / 2;
				this.maxHp = maxHp / 2 + maxHp;
				hpCost = hpCost / 2 + hpCost;
				menu.UpdateText(menu.health, "Health: " + this.hp + "/" + maxHp);
				menu.UpdateText(menu.healthU, "Upgrade: " + hpCost + "g"); 
			}
		}
		if(type == "Range")
		{
			if(GameManager.gm.gold.GetAmount() >= rangeCost)
			{
				GameManager.gm.gold.DeductAmount(rangeCost);
				this.attackRange += 1.2f;
				rangeCost = rangeCost / 2 + rangeCost;
				menu.UpdateText(menu.range, "Range: " + this.attackRange);
				menu.UpdateText(menu.rangeU, "Upgrade: " + rangeCost + "g");

				rangeScale = rangeCyl.transform.localScale;
				rangeScale.x = attackRange * 2;
				rangeScale.z = attackRange * 2;
				rangeCyl.transform.localScale = rangeScale;
			}
		}
		if(type == "Speed")
		{
			if(GameManager.gm.gold.GetAmount() >= speedCost)
			{
				GameManager.gm.gold.DeductAmount(speedCost);
				this.shootCooldown -= 0.2f ;
				speedCost = speedCost / 2 + speedCost;
				float showNum = 60 / this.shootCooldown;
				showNum = (float) System.Math.Round(showNum, 2);
				menu.UpdateText(menu.speed, "Shots/Min: " + showNum);
				menu.UpdateText(menu.speedU, "Upgrade: " + speedCost + "g"); 
			}
		}
	}

	public bool OnAttack(int dmg)
	{
		this.hp -= dmg;
		menu.UpdateText(menu.health, "Health: " + this.hp + "/" + maxHp);
		if(this.hp <= 0)
		{
			return true;
		}
		return false;
	}

	public void Kill()
	{
		GameManager.gm.towers.Remove(this.gameObject);
		//play anim
		Destroy(this.gameObject); //destroy after anim length
	}

	private bool hovering;
	private void OnMouseEnter()
	{
		hovering = true;
	}

	private void OnMouseExit()
	{
		hovering = false;
	}

	public void OnMouseOver()
	{
		if(Buildmanager.BM.buildMode == BuildMode.None)
		{
			if(Input.GetMouseButtonUp(0))
			{
				Select();
			}
		}
	}

	public float GetRange()
	{
		return attackRange;
	}

	public GameObject GetRangeCyl()
	{
		return this.rangeCyl;
	}

	public BuildMode GetBuildingMode()
	{
		return this.buildmode;
	}

	public string GetDescription()
	{
		return this.description;
	}

	public void Select()
	{
		if(Buildmanager.BM.canBuild)
		{
			foreach(GameObject tower in GameManager.gm.towers)
			{
				if(tower.GetComponent<Tower>())
				{
					tower.GetComponent<Tower>().Deselect();
				}else{
					tower.GetComponent<ResourceBuilding>().Deselect();
				}
			}
			menu.Show();
			ShowRange();
		}
	}

	public void Deselect()
	{
		menu.Close();
		HideRange();
	}

	public void ShowRange()
	{
		rangeCyl.renderer.enabled = true;	
	}

	public void HideRange()
	{
		if(!Settings.showAllRanges)
		{
			rangeCyl.renderer.enabled = false;	
		}
	}
}