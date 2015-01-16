using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour
{
	public static Settings s;
	public static bool showAllRanges;
	public static bool disableParticles;
	public static int difficulty;
	public bool startWithMenu;
	public bool startWithFade;

	public Image fadePlane;

	private void Awake()
	{
		s = this;
		difficulty = PlayerPrefs.GetInt("Dif");
	}

	public void Start()
	{
		if(startWithFade)
		{
			fadingIn = true;
			Color c = fadePlane.color;
			c.a = 1f;
			fadePlane.color = c;
		}
	}

	bool fadingIn;
	private void Update()
	{
		if(fadingIn)
		{
			Color c = fadePlane.color;
			c.a -= 0.0045f;
			fadePlane.color = c;
			if(c.a <= 0)
			{
				fadingIn = false;
			}
		}
	}

	public void SetCheats(bool b)
	{
		if(b)
		{
			PlayerPrefs.SetInt("Cheats", 1);
		}else{
			PlayerPrefs.SetInt("Cheats", 0);
		}
	}

	public void Mute(bool b)
	{
		if(b)
		{
			Destroy(Camera.main.gameObject.GetComponent<AudioListener>());
		}else{
			Camera.main.gameObject.AddComponent<AudioListener>();
		}
	}

	public void SetParticles(bool b)
	{
		disableParticles = b;
	}

	public void SetDifficulty(float dif)
	{
		difficulty = (int) dif;
		PlayerPrefs.SetInt("Dif", difficulty);
	}
}

