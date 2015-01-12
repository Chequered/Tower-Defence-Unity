using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
	public static Settings s;
	public static bool showAllRanges;
	public static bool disableParticles;
	public static int difficulty;
	public bool startWithMenu;

	private void Awake()
	{
		s = this;
		difficulty = PlayerPrefs.GetInt("Dif");
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

