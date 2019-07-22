using System.Collections;
using UnityEngine;

public class Dash : MoveAbility
{
	private Queue velocities = new Queue();

	private JumpAttack jump;

	private bool canDash = true;

	protected override void DoStart()
	{
		base.DoStart();

		jump = GetComponent<JumpAttack>();
	}

	protected override void DoActionBeforeDuration()
	{
		if (!canDash)
		{
			CancelAction();
			return;
		}

		base.DoActionBeforeDuration();

		Vector3 moveDir = PlayerController.MoveDir;
		if (moveDir == Vector3.zero)
		{
			moveDir = transform.forward;
		}

		Vector3 dashVelocity = moveDir * Velocity;
		Vector3 prevVelocity = Rigidbody.velocity;
		prevVelocity.y = 0;
		Rigidbody.velocity = prevVelocity + dashVelocity;
		Rigidbody.useGravity = false;
		PlayerController.CanMove = false;
		Health.IsInvincible = true;
		velocities.Enqueue(dashVelocity);

		if (!jump.IsJumping)
		{
			Animations.SetIsDashing(true);
		}
		
		canDash = false;
	}

	protected override void DoHalfDurationAction()
	{
		base.DoHalfDurationAction();
		// Actually will be after end of duration
		canDash = true;
	}

	protected override void DoActionAfterDuration()
	{
		base.DoActionAfterDuration();

		Vector3 velocity = (Vector3)velocities.Dequeue();
		Vector3 newVelocity = Rigidbody.velocity - velocity * 0.5f;
		if (Vector3.Dot(newVelocity, PlayerController.MoveDir) > 0)
		{
			Rigidbody.velocity = newVelocity;
		}
		Rigidbody.useGravity = true;
		PlayerController.CanMove = true;
		Health.IsInvincible = false;
		Animations.SetIsDashing(false);
	}
	
}
