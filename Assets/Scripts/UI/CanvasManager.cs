using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour {

	private GameObject pausePanel;
	public Text gold;
	public Text wood;
	public Text stone;
	public Text food;

	public GameObject notification;
	public GameObject tooltip;

	public GameObject showRanges;
	public GameObject hideRanges;

	public GameObject gameOverButton;
	public Image fadePlane;

	private void Start()
	{
		pausePanel = transform.FindChild("Paused panel").gameObject;
		msgText = notification.transform.FindChild("Text").GetComponent<Text>();
		msgPanelColor = notification.GetComponent<Image>().color;
		msgTextColor = msgText.color;
		notification.gameObject.SetActive(false);
		msgText.gameObject.SetActive(false);
	}

	private void Update()
	{
		gold.text = "Gold: " + GameManager.gm.gold.GetAmount();
		wood.text = "Wood: " + GameManager.gm.wood.GetAmount();
		stone.text = "Stone: " + GameManager.gm.stone.GetAmount();
		food.text = "Food: " + GameManager.gm.food.GetAmount();
		if(fadeCooldown >= 0)
		{
			fadeCooldown -= 0.02f;
		}else{
			Color p = notification.GetComponent<Image>().color;
			Color t = msgText.color;

			p.a -= 0.03f;
			t.a -= 0.03f;

			notification.GetComponent<Image>().color = p;
			msgText.color = t;
		}

		if(fadingOut)
		{
			Color c = fadePlane.color;
			c.a += 0.0035f;
			fadePlane.color = c;
			if(c.a >= 1)
			{
				Application.LoadLevel(0);
			}
		}
	}

	public void TogglePause()
	{
		if(GameManager.paused)
		{
			pausePanel.SetActive(true);
		}else{
			pausePanel.SetActive(false);
		}
	}

	float fadeCooldown;
	Text msgText;
	Color msgPanelColor;
	Color msgTextColor;
	public void PrintMessage(string msg, float cooldown)
	{
		notification.transform.FindChild("Text").GetComponent<Text>().text = msg;
		notification.GetComponent<Image>().color = msgPanelColor;
		msgText.color = msgTextColor;
		fadeCooldown = cooldown;

		notification.gameObject.SetActive(true);
		msgText.gameObject.SetActive(true);
	}

	private void OnMouseEnter()
    {
        Buildmanager.BM.SetCanbuild(false);
    }

    private void OnMouseExit()
    {
        Buildmanager.BM.SetCanbuild(true);
    }

	
	public void ShowRanges()
	{
		Settings.showAllRanges = true;
		showRanges.SetActive(false);
		hideRanges.SetActive(true);
		foreach(GameObject tower in GameManager.gm.towers)
		{
			if(tower.GetComponent<Tower>())
			{
				tower.GetComponent<Tower>().ShowRange();
			}else{
				tower.GetComponent<ResourceBuilding>().ShowRange();
			}
		}
	}

	public void ToolTip(GameObject building)
	{
		tooltip.gameObject.SetActive(true);
		if(building.GetComponent<Tower>())
		{
			tooltip.GetComponent<BuildingToolTip>().SetText(building.GetComponent<Tower>().GetDescription() + "\n" + "Cost: " + building.GetComponent<Cost>().GetAmount() + " " + building.GetComponent<Cost>().type);
		}else if(building.GetComponent<ResourceBuilding>()){
			tooltip.GetComponent<BuildingToolTip>().SetText(building.GetComponent<ResourceBuilding>().GetDescription() + "\n" + "Cost: " + building.GetComponent<Cost>().GetAmount() + " " + building.GetComponent<Cost>().type);
		}
	}

	public void CloseToolTip()
	{
		tooltip.gameObject.SetActive(false);
	}

	public void GameOverMSG()
	{
		PrintMessage("Game Over", Mathf.Infinity);
		gameOverButton.SetActive(true);
	}

	bool fadingIn;
	public void FadeIn()
	{
		Color c = fadePlane.color;
		c.a = 1;
		fadePlane.color = c;
		fadingIn = true;
	}

	bool fadingOut;
	public void RestartGame()
	{
		Time.timeScale = 1;
		fadingOut = true;
		Destroy(gameOverButton);
	}

	public void HideRanges()
	{
		Settings.showAllRanges = false;
		showRanges.SetActive(true);
		hideRanges.SetActive(false);
		foreach(GameObject tower in GameManager.gm.towers)
		{
			if(tower.GetComponent<Tower>())
			{
				tower.GetComponent<Tower>().HideRange();
			}else{
				tower.GetComponent<ResourceBuilding>().HideRange();
			}
		}
	}
}
