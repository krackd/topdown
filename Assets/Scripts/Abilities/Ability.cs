using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

	[Header("Ability")]
	public float RechargeCooldown = 3f;
	public int Charges = 3;
	protected int charges;
	public bool HasCharges { get { return charges > 0; } }
	public bool LaunchTimersInUpdate = true;

	[Header("Input")]
	public string ButtonName;
	
	protected PlayerController PlayerController { get; private set; }
	protected Rigidbody Rigidbody { get; private set; }
	protected Health Health { get; private set; }
	protected Animations Animations { get; private set; }

	protected bool IsDead { get { return Health != null && Health.IsDead; } }

	protected bool ActionCanceled = false;

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

			if (ActionCanceled)
			{
				ActionCanceled = false;
				return;
			}

			charges--;
			
			if (LaunchTimersInUpdate)
			{
				LaunchTimers();
			}

		}
	}

	protected virtual void LaunchTimers()
	{
		CoroutineUtils.timeout(this, RechargeCooldown, () =>
		{
			charges++;
		});
	}

	protected virtual void DoStart()
	{

	}

	protected void CancelAction()
	{
		ActionCanceled = true;
	}

	protected virtual void DoAction()
	{

	}
}
