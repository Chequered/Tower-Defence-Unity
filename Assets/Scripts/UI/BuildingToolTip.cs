using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingToolTip : MonoBehaviour {

	private void Start()
	{
		this.gameObject.SetActive(false);
	}

	public void SetBuilding(GameObject b)
	{
		GameManager.gm.mUI.GetComponent<CanvasManager>().ToolTip(Buildmanager.BM.GetBuilding(b.GetComponent<BuildTowerButton>().building));
	}

	public void CloseToolTip()
	{
		GameManager.gm.mUI.GetComponent<CanvasManager>().CloseToolTip();
	}

	public void SetText(string txt)
	{
		transform.FindChild("Text").GetComponent<Text>().text = txt;
	}
}
