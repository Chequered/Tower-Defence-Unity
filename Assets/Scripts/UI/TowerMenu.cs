using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerMenu : MonoBehaviour {

	private Tower tower;
	private ResourceBuilding RB;

	public Text name;
	public Text range;
	public Text damage;
	public Text health;
	public Text speed;

	public Text rangeU;
	public Text damageU;
	public Text healthU;
	public Text speedU;

	public Text description;

	public void UpdateText(Text element, string text)
	{
		if(element != null)
		{
			element.text = text;
		}
	}

	public void Close()
	{
		this.gameObject.SetActive(false);
		if(!Settings.showAllRanges)
		{
			if(tower != null)
			{
				tower.GetRangeCyl().renderer.enabled = false;
			}else{
				RB.GetRangeCyl().renderer.enabled = false;
			}
		}
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void SetTower(Tower t)
	{
		this.tower = t;
	}

	public void SetRB(ResourceBuilding rb)
	{
		this.RB = rb;
	}
}
