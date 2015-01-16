using UnityEngine;
using System.Collections;

public class Cost : MonoBehaviour
{
	public ResourceType type;
	public int amount;

	public int GetAmount()
	{
		return amount + GameManager.gm.towers.Count;
	}
}

