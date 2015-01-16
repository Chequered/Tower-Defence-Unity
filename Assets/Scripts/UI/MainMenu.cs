using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject cheatsToggle;

	private GameObject settingsMenu;
	private GameObject creditsMenu;
	private GameObject helpMenu;

	private void Start()
	{
		GameManager.gm.SetPause(true);
		if(!Settings.s.startWithMenu)
		{
			Toggle(false);
		}
		settingsMenu = transform.FindChild("Settings").gameObject;
		creditsMenu = transform.FindChild("Credits").gameObject;
		helpMenu = transform.FindChild("HowToPlay").gameObject;

		if(PlayerPrefs.GetInt("Cheats") == 1)
		{
			cheatsToggle.GetComponent<Toggle>().isOn = true;
		}
	}

	public void GoToSettingsScreen()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		settingsMenu.SetActive(true);
		creditsMenu.SetActive(false);
		helpMenu.SetActive(false);
	}
	
	public void GoToCreditsScreen()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		settingsMenu.SetActive(false);
		creditsMenu.SetActive(true);
		helpMenu.SetActive(false);
	}

	public void GoToMainScreen()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}
		settingsMenu.SetActive(false);
		creditsMenu.SetActive(false);
		helpMenu.SetActive(false);
	}

	public void GoToHelpScreen()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		settingsMenu.SetActive(false);
		creditsMenu.SetActive(false);
		helpMenu.SetActive(true);
	}

	public void Toggle(bool b)
	{
		this.gameObject.SetActive(b);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
