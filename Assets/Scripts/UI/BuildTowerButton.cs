using UnityEngine;
using System.Collections;

public class BuildTowerButton : MonoBehaviour {

	public BuildMode building;

	private void OnMouseEnter()
	{
		GameManager.gm.mUI.GetComponent<CanvasManager>().ToolTip(Buildmanager.BM.GetBuilding(building));
	}
}
