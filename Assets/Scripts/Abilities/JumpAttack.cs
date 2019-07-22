using UnityEngine;

public class JumpAttack : MoveAbility
{
	public bool IsJumping { get { return nbJumps > 0; } }

	private const float JUMP_MOVE_REDUCTION_FACTOR = 0.3f;

	private int nbJumps = 0;

	protected override void DoStart()
	{
		base.DoStart();
		Animations.OnJumpBegin.AddListener(JumpBeginEvent);
		LaunchTimersInUpdate = false;
		HalfDurationFactor = 0.6f;
	}
	
	protected override void DoActionBeforeDuration()
	{
		base.DoActionBeforeDuration();

		// JumpBeginEvent will change the velocity
		Animations.JumpAttack();
		nbJumps++;
	}

	public void JumpBeginEvent()
	{
		DoJump();
	}

	private void DoJump()
	{
		Rigidbody.velocity += (transform.forward + transform.up).normalized * Velocity;
		PlayerController.ReduceMoveControl(JUMP_MOVE_REDUCTION_FACTOR);
		LaunchTimers();
	}

	protected override void DoHalfDurationAction()
	{
		if (nbJumps <= 1)
		{
			Rigidbody.velocity += -transform.up * 1.5f * Velocity;
		}
	}

	protected override void DoActionAfterDuration()
	{
		Rigidbody.velocity = Vector3.zero;
		PlayerController.ResetMoveControl();
		nbJumps--;
	}
	
}
