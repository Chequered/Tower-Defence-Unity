using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public static bool paused;

	public SpawnManager sm;
	public Canvas mUI;

	public List<GameObject> towers;
	public List<GameObject> enemies;
	public GameObject hq;

	public int startingGold;
	public int startingWood;
	public int startingFood;
	public int startingStone;
	public Resource gold;
	public Resource wood;
	public Resource stone;
	public Resource food;


	private void Awake()
	{
		gm = this;
		towers = new List<GameObject>();
		enemies = new List<GameObject>();
		hq = GameObject.FindGameObjectWithTag("HQ");
		sm = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
		gold = new Resource();
		wood = new Resource();
		stone = new Resource();
		food = new Resource();
		
		gold.SetAmount(startingGold);
		wood.SetAmount(startingWood);
		food.SetAmount(startingFood);
		stone.SetAmount(startingStone);
	}

	public void StartGame()
	{
		SetPause(false);
		if(PlayerPrefs.GetInt("Cheats") == 1)
		{
			GameManager.gm.AddResource(ResourceType.Gold, 9999);
			GameManager.gm.AddResource(ResourceType.Food, 9999);	
			GameManager.gm.AddResource(ResourceType.Wood, 9999);	
			GameManager.gm.AddResource(ResourceType.Stone, 9999);	
		}
	}

	private void Update()
	{
		if(hq == null)
		{
			paused = true;
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			TogglePause();
		}		                 
	}

	public void AddResource(ResourceType type, int amount)
	{
		if(type == ResourceType.Gold)
		{
			gold.AddAmount(amount);
		}
		if(type == ResourceType.Wood)
		{
			wood.AddAmount(amount);
		}
		if(type == ResourceType.Stone)
		{
			stone.AddAmount(amount);
		}
		if(type == ResourceType.Food)
		{
			food.AddAmount(amount);
		}
	}

	public void DeductResource(ResourceType type, int amount)
	{
		if(type == ResourceType.Gold)
		{
			gold.DeductAmount(amount);
		}
		if(type == ResourceType.Wood)
		{
			wood.DeductAmount(amount);
		}
		if(type == ResourceType.Stone)
		{
			stone.DeductAmount(amount);
		}
		if(type == ResourceType.Food)
		{
			food.DeductAmount(amount);
		}
	}

	public void RemoveEnemy(GameObject enemy)
	{
		enemies.Remove(enemy);
		Destroy(enemy.transform.parent.gameObject);
	}

	public void TogglePause()
	{
		if(paused)
		{
			paused = false;
		}else{
			paused = true;
		}
		mUI.GetComponent<CanvasManager>().TogglePause();
	}

	public void SetPause(bool b)
	{
		paused = b;
	}

	public bool CheckSufficientResource(ResourceType t, int a)
	{
		if(t == ResourceType.Wood)
		{
			if(wood.GetAmount() >= a)
			{
				return true;
			}else{
				return false;
			}
		}
		if(t == ResourceType.Food)
		{
			if(food.GetAmount() >= a)
			{
				return true;
			}else{
				return false;
			}
		}
		if(t == ResourceType.Stone)
		{
			if(stone.GetAmount() >= a)
			{
				return true;
			}else{
				return false;
			}
		}
		if(t == ResourceType.Gold)
		{
			if(gold.GetAmount() >= a)
			{
				return true;
			}else{
				return false;
			}
		}
		return false;
	}

}

