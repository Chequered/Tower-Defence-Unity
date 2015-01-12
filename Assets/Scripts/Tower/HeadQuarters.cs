using UnityEngine;
using System.Collections;

public class HeadQuarters : MonoBehaviour {

	public Texture goodTex;
	public Texture badTex;

	private Tower tower;
	private Buildmanager bm;
	private LineRenderer ln;

	private void Start()
	{
		tower = GetComponent<Tower>();
		ln = GetComponent<LineRenderer>();
		bm = Buildmanager.BM;
		GameManager.gm.towers.Add(this.gameObject);
	}
	
	private void Update()
	{
		if(bm.buildMode != null)
		{
			if(bm.buildingOutline != null && bm.closestTower != null)
			{
				ln.SetPosition(0, bm.closestTower.transform.position);
				ln.SetPosition(1, bm.buildingOutline.transform.position);
				ln.enabled = true;
				if(bm.IsInRange())
				{
					ln.material.SetTexture("_MainTex", goodTex);
				}else{
					ln.material.SetTexture("_MainTex", badTex);
				}
			}else{
				ln.enabled = false;
			}
		}
	}
}