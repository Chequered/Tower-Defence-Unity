using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildMode
{
    None,
    GunTower,
	MusketTower,
	MortarTower,
	Windmill,
	Sawmill,
	Mine
}

public class Buildmanager : MonoBehaviour {

    public static Buildmanager BM;

    public Color canBuildColor;
    public Color canNotBuildColor;

    public GameObject[] buildings;
    public BuildMode buildMode;
	public GameObject mortarExplosion;

	public GameObject closestTower;
	public bool canBuild;
	public GameObject buildingOutline;

	private bool inRange;
	private GameObject selectedBuilding;

    private void Awake()
    {
        BM = this;
    }

    private void Start()
    {

    }

	Vector3 point = new Vector3(0, 0, 0);
	List<GameObject> towersInRange = new List<GameObject>();
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999))
        {
            if (buildingOutline != null)
            {
				buildingOutline.transform.position = hit.point;

				float best = Mathf.Infinity;
				float dis = 0;
				inRange = false;
				towersInRange.Clear();
				foreach(GameObject tower in GameManager.gm.towers)
				{
					dis = Vector3.Distance(tower.transform.position, buildingOutline.transform.position);
					if(tower.GetComponent<Tower>())
					{
						if(dis < tower.GetComponent<Tower>().GetRange())
						{
							towersInRange.Add(tower);
						}
					}else{
						if(dis < tower.GetComponent<ResourceBuilding>().GetRange())
						{
							towersInRange.Add(tower);
						}
					}
				}
				foreach(GameObject tower in towersInRange)
				{
					dis = Vector3.Distance(tower.transform.position, buildingOutline.transform.position);
					if(dis < best)
					{
						closestTower = tower;
						best = dis;
					}
					inRange = true;
				}
            }
			if(Input.GetMouseButtonUp(1))
			{
				Destroy(buildingOutline);
				foreach(GameObject b in GameManager.gm.towers)
				{
					if(b.GetComponent<Tower>())
					{
						b.GetComponent<Tower>().HideRange();
					}else{
						b.GetComponent<ResourceBuilding>().HideRange();
					}
				}
				buildingOutline = null;
				buildMode = BuildMode.None;
			}
			if (canBuild && inRange)
			{
				if(buildMode == BuildMode.GunTower || buildMode == BuildMode.MusketTower || buildMode == BuildMode.MortarTower)
				{
					if (hit.transform.tag == "Map" || hit.transform.tag == "Area_Farm" || hit.transform.tag == "Area_Forrest" || hit.transform.tag == "Area_Mountain")
					{
						if (Input.GetMouseButtonUp(0))
						{
							Build(hit.point);
						}
		            }
				}
				if(buildMode == BuildMode.Windmill)
				{
					if (hit.transform.tag == "Area_Farm")
					{
						if (Input.GetMouseButtonUp(0))
						{
							Build(hit.point);
						}
					}
				}
				if(buildMode == BuildMode.Sawmill)
				{
					if (hit.transform.tag == "Area_Forrest")
					{
						if (Input.GetMouseButtonUp(0))
						{
							Build(hit.point);
						}
					}
				}
				
				if(buildMode == BuildMode.Mine)
				{
					if (hit.transform.tag == "Area_Mountain")
					{
						if (Input.GetMouseButtonUp(0))
						{
							Build(hit.point);
						}
					}
				}
			}
        }
    }

    private void Build(Vector3 pos)
    {
		if(GameManager.gm.CheckSufficientResource(selectedBuilding.GetComponent<Cost>().type, selectedBuilding.GetComponent<Cost>().amount))
		{
			GameManager.gm.DeductResource(selectedBuilding.GetComponent<Cost>().type, selectedBuilding.GetComponent<Cost>().amount);
			GameObject tower = Instantiate(selectedBuilding, pos, selectedBuilding.transform.rotation) as GameObject;
			if(tower.GetComponent<Tower>())
			{
				tower.GetComponent<Tower>().Build(closestTower);
			}else{
				tower.GetComponent<ResourceBuilding>().Build(closestTower);
			}
			GameManager.gm.towers.Add(tower);
	        SetBuildMode("None");
			Destroy(buildingOutline);
			foreach(GameObject _tower in GameManager.gm.towers)
			{
				if(_tower.GetComponent<Tower>())
				{
					_tower.GetComponent<Tower>().HideRange();
				}else{
					_tower.GetComponent<ResourceBuilding>().HideRange();
				}
			}
		}else{
			GameManager.gm.mUI.GetComponent<CanvasManager>().PrintMessage("You have insufficient resources!", 3);
		}
    }

    public void SetBuildMode(string mode)
    {
        if (buildingOutline != null)
        {
            Destroy(buildingOutline);
        }
        if(mode == "GunTower")
        {
            buildMode = BuildMode.GunTower;
			buildingOutline = Instantiate(GetBuilding(BuildMode.GunTower), new Vector3(999, 999, 99), GetBuilding(BuildMode.GunTower).transform.rotation) as GameObject; //Change to lose-code
			selectedBuilding = GetBuilding(buildMode);
        }
		if(mode == "MusketTower")
		{
			buildMode = BuildMode.MusketTower;
			buildingOutline = Instantiate(GetBuilding(BuildMode.MusketTower), new Vector3(999, 999, 99), GetBuilding(BuildMode.MusketTower).transform.rotation) as GameObject; //Change to lose-code
			selectedBuilding = GetBuilding(buildMode);
		}
		if(mode == "MortarTower")
		{
			buildMode = BuildMode.MortarTower;
			buildingOutline = Instantiate(GetBuilding(BuildMode.MortarTower), new Vector3(999, 999, 99), GetBuilding(BuildMode.MortarTower).transform.rotation) as GameObject; //Change to lose-code
			selectedBuilding = GetBuilding(buildMode);
		}
		if(mode == "Windmill")
		{
			buildMode = BuildMode.Windmill;
			buildingOutline = Instantiate(GetBuilding(buildMode), new Vector3(999, 999, 99), GetBuilding(buildMode).transform.rotation) as GameObject;
			selectedBuilding = GetBuilding(buildMode);
		}
		if(mode == "SawMill")
		{
			buildMode = BuildMode.Sawmill;
			buildingOutline = Instantiate(GetBuilding(BuildMode.Sawmill), new Vector3(999, 999, 99), GetBuilding(BuildMode.Sawmill).transform.rotation) as GameObject;
			selectedBuilding = GetBuilding(buildMode);
		}
		if(mode == "Mine")
		{
			buildMode = BuildMode.Mine;
			buildingOutline = Instantiate(GetBuilding(BuildMode.Mine), new Vector3(999, 999, 99), GetBuilding(BuildMode.Mine).transform.rotation) as GameObject;
			selectedBuilding = GetBuilding(buildMode);
		}
		if(mode == "None")
		{
			buildMode = BuildMode.None;
			selectedBuilding = null;
		}
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

	public GameObject GetBuilding(BuildMode mode)
	{
		for(int i = 0; i < buildings.Length; i ++)
		{
			if(buildings[i].GetComponent<Tower>())
			{
				if(buildings[i].GetComponent<Tower>().GetBuildingMode() == mode)
				{
					return buildings[i];
				}
			}else{
				if(buildings[i].GetComponent<ResourceBuilding>().GetBuildingMode() == mode)
				{
					return buildings[i];
				}
			}
		}
		return null;
	}

    public void SetCanbuild(bool b)
    {
        canBuild = b;
    }

	public bool IsInRange()
	{
		return inRange;
	}
}
