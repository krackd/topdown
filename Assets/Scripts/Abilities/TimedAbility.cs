using UnityEngine;

public abstract class TimedAbility : Ability {

	[Header("Timers")]
	public float DurationInSeconds = 2f;	
	public float HalfDurationFactor = 0.5f;
	public bool LockForDuration = true;

	public bool CanPerform = true;

	protected sealed override void DoAction()
	{
		base.DoAction();

		if (ActionCanceled || !CanPerform)
		{
			return;
		}

		DoActionBeforeDuration();

		if (LockForDuration)
		{
			CanPerform = false;
		}
	}

	protected override void LaunchTimers()
	{
		base.LaunchTimers();

		CoroutineUtils.timeout(this, DurationInSeconds * HalfDurationFactor, () =>
		{
			DoHalfDurationAction();
		});

		CoroutineUtils.timeout(this, DurationInSeconds, () =>
		{
			DoActionAfterDuration();

			if (LockForDuration)
			{
				CanPerform = true;
			}
		});
	}

	protected virtual void DoActionBeforeDuration()
	{

	}

	protected virtual void DoHalfDurationAction()
	{

	}

	protected virtual void DoActionAfterDuration()
	{

	}

}
