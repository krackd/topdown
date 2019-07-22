using System.Collections;
using UnityEngine;

public class Dash : MoveAbility
{
	private Queue velocities = new Queue();

	protected override void DoActionBeforeDuration()
	{
		base.DoActionBeforeDuration();

		Vector3 dashVelocity = PlayerController.MoveDir * Velocity;
		Vector3 prevVelocity = Rigidbody.velocity;
		prevVelocity.y = 0;
		Rigidbody.velocity = prevVelocity + dashVelocity;
		PlayerController.CanMove = false;
		velocities.Enqueue(dashVelocity);
	}

	protected override void DoActionAfterDuration()
	{
		base.DoActionAfterDuration();

		Vector3 velocity = (Vector3)velocities.Dequeue();
		Rigidbody.velocity -= velocity * 0.5f;
		PlayerController.CanMove = true;
	}
	
}
