using UnityEngine;

public abstract class MoveAbility : Ability
{
	[Header("Move ability")]
	public float Velocity = 5f;

	protected Attack Attack { get; private set; }

	protected override void DoStart()
	{
		Attack = GetComponent<Attack>();
		if (Attack == null)
		{
			Debug.LogError("Attack component not found");
		}
	}
}
