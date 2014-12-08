using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    private GameObject target;
    private bool targetSet;

	public void SetTarget(GameObject target)
    {
        this.target = target;
        this.targetSet = true;
    }

    private void Update()
    {
        if(targetSet)
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }
}
