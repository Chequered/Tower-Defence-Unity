using UnityEngine;
using System.Collections;

public class ResourceBuilding : MonoBehaviour
{
	//GO's
	public GameObject rangeObj;
	public TowerMenu menu;
	private GameObject towerLinked;

	//data
	public string name;
	public string description;
	public BuildMode mode;
	public ResourceType type;
	public int baseUpgradeCost;

	//stats
	public int hp;
	private int maxHP;
	public float range;
	public float productionCooldown;
	public int productionStrength;

	//vars
	protected bool build;
	
	Vector3 rangeScale = new Vector3(0,0,0);
	private void Start()
	{
		
		maxHP = hp;
		
		productionCost = baseUpgradeCost;
		speedCost = baseUpgradeCost;
		rangeCost = baseUpgradeCost;
		hpCost = baseUpgradeCost;
		
		rangeScale = rangeObj.transform.localScale;
		rangeScale.x = range * 2f;
		rangeScale.z = range * 2f;
		rangeObj.transform.localScale = rangeScale;

		menu.UpdateText(menu.damage, "Production: " + this.productionStrength);
		menu.UpdateText(menu.health, "Health: " + this.hp);
		menu.UpdateText(menu.range, "Range: " + this.range);
		menu.UpdateText(menu.speed, "Production/Min: " +  60 / this.productionCooldown);
		menu.UpdateText(menu.name, this.name);
		menu.UpdateText(menu.description, this.description);
		
		menu.UpdateText(menu.damageU,"Upgrade: " + productionCost + "g");
		menu.UpdateText(menu.speedU,"Upgrade: " + speedCost + "g");
		menu.UpdateText(menu.healthU,"Upgrade: " + hpCost + "g");
		menu.UpdateText(menu.rangeU,"Upgrade: " + rangeCost + "g");

		menu.SetRB(this);
	}

	float timeSinceLastProduction = 0;
	private void Update()
	{
		if(build)
		{
			if(timeSinceLastProduction < Time.time)
			{
				Produce();
			}
			if(towerLinked == null)
			{
				bool foundTower = false;
				foreach(GameObject tower in GameManager.gm.towers)
				{
					if(!foundTower)
					{
						if(Vector3.Distance(transform.position, tower.transform.position) <= range)
						{
							towerLinked = tower;
							foundTower = true;
							GetComponent<LineRenderer>().SetPosition(1, tower.transform.position);
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

	private void Produce()
	{
		timeSinceLastProduction = Time.time + productionCooldown;
		GameManager.gm.AddResource(type, productionStrength);
	}

	int productionCost;
	int speedCost;
	int hpCost;
	int rangeCost;
	public void Upgrade(string type)
	{
		if(type == "Production_Strength")
		{
			if(GameManager.gm.gold.GetAmount() >= productionCost)
			{
				GameManager.gm.gold.DeductAmount(productionCost);
				this.productionStrength = productionStrength / 2 + productionStrength;
				productionCost = productionCost / 2 + productionCost;
				menu.UpdateText(menu.damage, "Damage: " + this.productionStrength);
				menu.UpdateText(menu.damageU, "Upgrade: " + productionCost + "g"); 
			}
		}
		if(type == "Health")
		{
			if(GameManager.gm.gold.GetAmount() >= hpCost)
			{
				GameManager.gm.gold.DeductAmount(hpCost);
				this.hp += maxHP / 2;
				this.maxHP = maxHP / 2 + maxHP;
				hpCost = hpCost / 2 + hpCost;
				menu.UpdateText(menu.health, "Health: " + this.hp + "/" + maxHP);
				menu.UpdateText(menu.healthU, "Upgrade: " + hpCost + "g"); 
			}
		}
		if(type == "Range")
		{
			if(GameManager.gm.gold.GetAmount() >= rangeCost)
			{
				GameManager.gm.gold.DeductAmount(rangeCost);
				this.range += 1.2f;
				rangeCost = rangeCost / 2 + rangeCost;
				menu.UpdateText(menu.range, "Range: " + this.range);
				menu.UpdateText(menu.rangeU, "Upgrade: " + rangeCost + "g");
				
				rangeScale = rangeObj.transform.localScale;
				rangeScale.x = range * 2;
				rangeScale.z = range * 2;
				rangeObj.transform.localScale = rangeScale;

				Debug.Log("range");
			}
		}
		if(type == "Production_Speed")
		{
			if(GameManager.gm.gold.GetAmount() >= speedCost)
			{
				GameManager.gm.gold.DeductAmount(speedCost);
				this.productionCooldown -= 0.2f ;
				speedCost = speedCost / 2 + speedCost;
				float showNum = 60 / this.productionCooldown;
				showNum = (float) System.Math.Round(showNum, 2);
				menu.UpdateText(menu.speed, "Production/Min: " + showNum);
				menu.UpdateText(menu.speedU, "Upgrade: " + speedCost + "g"); 
			}
		}
	}
	
	
	public bool OnAttack(int dmg)
	{
		this.hp -= dmg;
		menu.UpdateText(menu.health, "Health: " + this.hp + "/" + maxHP);
		if(this.hp <= 0)
		{
			return true;
		}
		return false;
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
		return range;
	}
	
	public GameObject GetRangeCyl()
	{
		return this.rangeObj;
	}
	
	public BuildMode GetBuildingMode()
	{
		return this.mode;
	}
	
	public void Select()
	{
		menu.Show();
		ShowRange();
	}
	
	public void Deselect()
	{
		menu.Close();
		HideRange();
	}
	
	public string GetDescription()
	{
		return this.description;
	}
	
	public void ShowRange()
	{
		rangeObj.renderer.enabled = true;	
	}
	
	public void HideRange()
	{
		if(!Settings.showAllRanges)
		{
			rangeObj.renderer.enabled = false;	
		}
	}
}

