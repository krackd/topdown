﻿using UnityEngine;

public class Attack : TimedAbility
{
	[Header("Attack")]
	public float Range = 1f;
	public bool CanAttack = true;
	public LayerMask AttackMask;
	public Transform[] RaycastOrigins;

	private const float ATTACK_MOVE_REDUCTION_FACTOR = 0.5f;
	private Coroutine attackCoroutine;

	private int nbAttacks = 0;

	protected override void DoStart()
	{
		base.DoStart();

		Animations.OnAttackEnded.AddListener(AttackEndEvent);
		Animations.OnDoDamage.AddListener(DoDamageEvent);
		Animations.OnDoAoe.AddListener(DoAoeEvent);

		LaunchTimersInUpdate = false;
	}

	protected override void DoActionBeforeDuration()
	{
		base.DoActionBeforeDuration();
	
		if (!CanAttack)
		{
			CancelAction();
			return;
		}

		Animations.Attack();

		RestartResetAttackCharges();

		CanAttack = false;
		nbAttacks++;
		PlayerController.ReduceMoveControl(ATTACK_MOVE_REDUCTION_FACTOR);
	}

	protected override void DoActionAfterDuration()
	{
		base.DoActionAfterDuration();

		AttackEndEvent();
	}

	public void AttackEndEvent()
	{
		nbAttacks--;

		if (nbAttacks <= 0)
		{
			nbAttacks = 0;
			CanAttack = true;
			PlayerController.ResetMoveControl();
		}
	}

	public void DoDamageEvent()
	{
		RaycastHit hit = new RaycastHit();
		foreach (Transform originTransform in RaycastOrigins)
		{
			Vector3 origin = originTransform.position;
			Vector3 dir = transform.forward;
			Ray ray = new Ray(origin, dir);
			if (Physics.Raycast(ray, out hit, Range, AttackMask.value))
			{
				Health health = hit.collider.gameObject.GetComponent<Health>();
				if (health == null)
				{
					health = hit.collider.gameObject.GetComponentInParent<Health>();
				}

				if (health != null)
				{
					health.Hurt(1);
				}
			}
		}
	}

	public void DoAoeEvent()
	{

	}

	private void RestartResetAttackCharges()
	{
		if (attackCoroutine != null)
		{
			StopCoroutine(attackCoroutine);
		}

		attackCoroutine = CoroutineUtils.timeout(this, RechargeCooldown, () =>
		{
			charges = Charges;
			Animations.ResetAttackAnim();
			attackCoroutine = null;
		});
	}
}
