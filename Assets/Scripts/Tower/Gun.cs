using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    private GameObject target;

	public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }
}
