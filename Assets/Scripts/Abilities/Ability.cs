using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

	[Header("Ability")]
	public float DurationInSeconds = 2f;
	public float RechargeCooldown = 3f;
	public int Charges = 3;
	protected int charges;

	[Header("Input")]
	public string ButtonName;

	[Header("Timers")]
	public bool LaunchTimersInUpdate = true;
	public float HalfDurationFactor = 0.5f;
	
	protected PlayerController PlayerController { get; private set; }
	protected Rigidbody Rigidbody { get; private set; }
	protected Health Health { get; private set; }
	protected Animations Animations { get; private set; }

	protected bool IsDead { get { return Health != null && Health.IsDead; } }

	private bool actionCanceled = false;

	// Use this for initialization
	void Start () {
		charges = Charges;

		PlayerController = GetComponent<PlayerController>();

		Rigidbody = GetComponent<Rigidbody>();
		if (Rigidbody == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		Health = GetComponent<Health>();
		Animations = GetComponentInChildren<Animations>();

		DoStart();
	}
	
	// Update is called once per frame
	void Update () {
		if (charges <= 0 || IsDead)
		{
			return;
		}

		if (Input.GetButtonDown(ButtonName))
		{
			DoAction();

			if (actionCanceled)
			{
				actionCanceled = false;
				return;
			}

			charges--;
			
			if (LaunchTimersInUpdate)
			{
				LaunchTimers();
			}
			
		}
	}

	protected void LaunchTimers()
	{
		CoroutineUtils.timeout(this, DurationInSeconds * HalfDurationFactor, () =>
		{
			DoHalfDurationAction();
		});

		CoroutineUtils.timeout(this, DurationInSeconds, () =>
		{
			DoActionAfterDuration();
			CoroutineUtils.timeout(this, RechargeCooldown, () =>
			{
				charges++;
			});
		});
	}

	protected virtual void DoStart()
	{

	}

	protected void CancelAction()
	{
		actionCanceled = true;
	}

	protected abstract void DoAction();
	protected abstract void DoHalfDurationAction();
	protected abstract void DoActionAfterDuration();
}
