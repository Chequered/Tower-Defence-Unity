using UnityEngine;
using System.Collections;

public enum ResourceType
{
	Gold,
	Wood,
	Stone,
	Food
}

public class Resource : ScriptableObject
{
	[SerializeField] protected int amount;
	[SerializeField] protected int startingAmount;
	protected string name;

	private void Start()
	{
		amount = startingAmount;
	}

	public int GetAmount()
	{
		return amount;
	}

	public void SetAmount(int amount)
	{
		this.amount = amount;
	}

	public void AddAmount(int amount)
	{
		this.amount += amount;
	}

	public void DeductAmount(int amount)
	{
		this.amount -= amount;
	}
}

