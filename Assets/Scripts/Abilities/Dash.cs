using UnityEngine;

public class Dash : MoveAbility
{
	protected override void DoAction()
	{
		Rigidbody.velocity = PlayerController.MoveDir * Velocity;
		PlayerController.CanMove = false;
	}

	protected override void DoActionAfterDuration()
	{
		Rigidbody.velocity = Vector3.zero;
		PlayerController.CanMove = true;
	}

	protected override void DoHalfDurationAction()
	{
		
	}
	
}
