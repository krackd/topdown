using UnityEngine;

public class JumpAttack : MoveAbility
{
	protected override void DoStart()
	{
		base.DoStart();
		Animations.OnJumpBegin.AddListener(JumpBeginEvent);
		LaunchTimersInUpdate = false;
		HalfDurationFactor = 0.6f;
	}
	
	protected override void DoAction()
	{
		Attack.CanAttack = false;
		// JumpBeginEvent will change the velocity
		Animations.JumpAttack();
	}

	public void JumpBeginEvent()
	{
		DoJump();
	}

	private void DoJump()
	{
		Rigidbody.velocity = (transform.forward + transform.up).normalized * Velocity;
		PlayerController.ReduceMoveControl();
		LaunchTimers();
	}

	protected override void DoHalfDurationAction()
	{
		Rigidbody.velocity += -transform.up * Velocity;
	}

	protected override void DoActionAfterDuration()
	{
		Rigidbody.velocity = Vector3.zero;
		PlayerController.CanMove = true;
	}
	
}
