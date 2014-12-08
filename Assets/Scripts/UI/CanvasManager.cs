using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {

	private void OnMouseEnter()
    {
        Buildmanager.BM.SetCanbuild(false);
    }

    private void OnMouseExit()
    {
        Buildmanager.BM.SetCanbuild(true);
    }
}
