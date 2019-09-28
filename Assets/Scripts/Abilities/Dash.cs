using System.Collections;
using UnityEngine;

public class Dash : MoveAbility
{
	public GameObject DashEffect;
	public float DashDurationInSeconds = 0.5f;

	private Queue velocities = new Queue();

	private JumpAttack jumpAttack;
	private Jump jump;

	protected override void DoStart()
	{
		base.DoStart();

		jumpAttack = GetComponent<JumpAttack>();
		jump = GetComponent<Jump>();

		if (DashEffect != null)
		{
			DashEffect.SetActive(false);
		}
	}

	protected override void DoActionBeforeDuration()
	{
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

		// Triggering dash trail effect
		if (DashEffect != null) {
			DashEffect.SetActive(true);
			StartCoroutine(stopDash(DashDurationInSeconds));
		}

		// Remove anim at the moment
		//if (!jump.IsJumping && !jumpAttack.IsJumping)
		//{
		//	Animations.SetIsDashing(true);
		//}
	}

	protected override void DoActionAfterDuration()
	{
		base.DoActionAfterDuration();

		//Vector3 velocity = (Vector3)velocities.Dequeue();
		//Vector3 newVelocity = Rigidbody.velocity - velocity * 0.5f;
		//if (Vector3.Dot(newVelocity, PlayerController.MoveDir) > 0)
		//{
		//	Rigidbody.velocity = newVelocity;
		//}
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.useGravity = true;
		PlayerController.CanMove = true;
		Health.IsInvincible = false;
		Animations.SetIsDashing(false);

		//if (DashEffect != null)
		//{
		//	DashEffect.SetActive(false);
		//}
	}

	IEnumerator stopDash(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (DashEffect != null)
		{
			DashEffect.SetActive(false);
		}
	}

}
