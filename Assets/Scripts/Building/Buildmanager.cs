using UnityEngine;
using System.Collections;

public enum BuildMode
{
    None,
    GunTower
}

public class Buildmanager : MonoBehaviour {

    public static Buildmanager BM;

    public Color canBuildColor;
    public Color canNotBuildColor;

    public GameObject[] buildings;
    public BuildMode buildMode;

    private GameObject selectedBuilding;
    private bool canBuild;

    private GameObject buildingOutline;

    private void Awake()
    {
        BM = this;
    }

    private void Start()
    {

    }

    Vector3 point = new Vector3(0, 0, 0);
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999))
        {
            if (buildingOutline != null)
            {
                point.x = hit.point.x;
                point.z = hit.point.z;
                point.y = 0;
                buildingOutline.transform.position = point;
            }
            if (hit.transform.tag == "Map")
            {
                if(buildingOutline != null)
                {
                    buildingOutline.transform.FindChild("Tower Prefab").renderer.material.color = canBuildColor;
                }
                if (canBuild)
                { 
                    if (Input.GetMouseButtonUp(0) && buildMode != BuildMode.None)
                    {
                        Build(hit.point);
                    }
                }
            }
            else
            {
                if (buildingOutline != null)
                {
                    buildingOutline.transform.FindChild("Tower Prefab").renderer.material.color = canNotBuildColor;
                }
            }
        }
    }

    private void Build(Vector3 pos)
    {
        pos.y = 0;
		GameObject tower = Instantiate(selectedBuilding, pos, selectedBuilding.transform.rotation) as GameObject;
		GameManager.gm.towers.Add(tower);
        SetBuildMode("None");
        Destroy(buildingOutline);
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
            buildingOutline = Instantiate(buildings[0], new Vector3(999, 999, 99), buildings[0].transform.rotation) as GameObject;
            buildingOutline.collider.enabled = false;
            selectedBuilding = buildings[0];
        }
        if(mode == "None")
        {
            buildMode = BuildMode.None;
            selectedBuilding = null;
        }
    }

    public void SetCanbuild(bool b)
    {
        canBuild = b;
    }
}
