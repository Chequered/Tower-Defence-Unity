using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElement : MonoBehaviour {

	public Text hpTxt;

	void Start () {
	}

	void Update()
	{	

	}

	public void UpdateElement (GameObject origin) {
		hpTxt.text = "" + origin.GetComponent<Enemy>().GetHealth();		
	}
}
