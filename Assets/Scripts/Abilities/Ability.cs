using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

	[Header("Ability")]
	public float DurationInSeconds = 2f;
	public float RechargeCooldown = 3f;
	public int Charges = 3;
	private int charges;

	[Header("Input")]
	public string ButtonName;

	[Header("Timers")]
	public bool LaunchTimersInUpdate = true;
	public float HalfDurationFactor = 0.5f;

	private Health health;
	private bool IsDead { get { return health != null && health.IsDead; } }

	protected PlayerController PlayerController;
	protected Rigidbody Rigidbody;
	protected Animations Animations;

	// Use this for initialization
	void Start () {
		charges = Charges;

		PlayerController = GetComponent<PlayerController>();

		Rigidbody = GetComponent<Rigidbody>();
		if (Rigidbody == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		health = GetComponent<Health>();
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
			charges--;

			if (LaunchTimersInUpdate)
			{
				LaunchTimers();
			}
			
		}
	}

	protected void LaunchTimers()
	{
		timeout(DurationInSeconds * HalfDurationFactor, () =>
		{
			DoHalfDurationAction();
		});

		timeout(DurationInSeconds, () =>
		{
			DoActionAfterDuration();
			timeout(RechargeCooldown, () =>
			{
				charges++;
			});
		});
	}

	private Coroutine timeout(float seconds, System.Action action)
	{
		return StartCoroutine(timeoutCoroutine(seconds, action));
	}

	private IEnumerator timeoutCoroutine(float seconds, System.Action action)
	{
		yield return new WaitForSeconds(seconds);
		action.Invoke();
	}

	protected virtual void DoStart()
	{

	}

	protected abstract void DoAction();
	protected abstract void DoHalfDurationAction();
	protected abstract void DoActionAfterDuration();
}
