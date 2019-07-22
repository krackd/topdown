using UnityEngine;

public class Dash : MoveAbility
{
	protected override void DoActionBeforeDuration()
	{
		base.DoActionBeforeDuration();

		Rigidbody.velocity = PlayerController.MoveDir * Velocity;
		PlayerController.CanMove = false;
	}

	protected override void DoActionAfterDuration()
	{
		base.DoActionAfterDuration();

		Rigidbody.velocity = Vector3.zero;
		PlayerController.CanMove = true;
	}
	
}
