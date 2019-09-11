using UnityEngine;

public class Jump : MoveAbility
{
	public bool IsJumping { get { return nbJumps > 0; } }

	private const float JUMP_MOVE_REDUCTION_FACTOR = 0.8f;

	private int nbJumps = 0;

	private Collider[] colliders;

	protected override void DoStart()
	{
		base.DoStart();
		Animations.OnJumpBegin.AddListener(JumpBeginEvent);
		LaunchTimersInUpdate = false;
		//HalfDurationFactor = 0.6f;

		colliders = GetComponentsInChildren<Collider>();
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
		Rigidbody.velocity += (transform.forward + transform.up * 0.5f).normalized * Velocity;
		//Rigidbody.useGravity = false;
		//SetCollidersEnabled(false);
		Health.IsInvincible = true;
		PlayerController.ReduceMoveControl(JUMP_MOVE_REDUCTION_FACTOR);
		LaunchTimers();
	}

	protected override void DoHalfDurationAction()
	{
		//if (nbJumps <= 1)
		//{
		//	SetCollidersEnabled(true);
		//	Rigidbody.velocity += (transform.forward * 0.5f - transform.up * 0.125f).normalized * 0.5f * Velocity;
		//	Rigidbody.useGravity = true;
		//}
	}

	protected override void DoActionAfterDuration()
	{
		//Rigidbody.velocity = Rigidbody.velocity * 0.5f;
		PlayerController.ResetMoveControl();
		Health.IsInvincible = false;
		nbJumps--;
	}

	private void SetCollidersEnabled(bool isEnabled)
	{
		foreach (Collider collider in colliders)
		{
			collider.enabled = isEnabled;
		}
	}
}
