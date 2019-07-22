using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedAbility : Ability {

	[Header("Timers")]
	public float DurationInSeconds = 2f;	
	public float HalfDurationFactor = 0.5f;

	protected sealed override void DoAction()
	{
		base.DoAction();

		if (ActionCanceled)
		{
			return;
		}

		DoActionBeforeDuration();
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
