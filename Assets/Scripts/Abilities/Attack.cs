using UnityEngine;

public class Attack : Ability
{

	public bool CanAttack = true;

	private Coroutine attackCoroutine;

	protected override void DoStart()
	{
		Animations.OnAttackEnded.AddListener(AttackEndEvent);
		Animations.OnDoDamage.AddListener(DoDamageEvent);
		Animations.OnDoAoe.AddListener(DoAoeEvent);

		LaunchTimersInUpdate = false;
	}

	protected override void DoAction()
	{
		if (!CanAttack)
		{
			CancelAction();
			return;
		}

		Animations.Attack();

		RestartResetAttackCharges();

		CanAttack = false;
		PlayerController.ReduceMoveControl();
	}

	public void AttackEndEvent()
	{
		CanAttack = true;
		PlayerController.ResetMoveControl();
	}

	public void DoDamageEvent()
	{

	}

	public void DoAoeEvent()
	{

	}

	protected override void DoActionAfterDuration()
	{
		
	}

	protected override void DoHalfDurationAction()
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
