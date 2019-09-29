using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour {

	[System.Serializable]
	public class ChargesChangedEvent : UnityEvent<int> { }

	[Header("Ability")]
	public float RechargeCooldown = 3f;
	public int Charges = 3;
	protected int charges;
	public bool HasCharges { get { return charges > 0; } }
	public bool LaunchTimersInUpdate = true;

	[Header("Input")]
	public string ButtonName;

	[Header("Events")]
	public ChargesChangedEvent OnChargesChanged;

	private GameObject Player;

	protected PlayerController PlayerController { get; private set; }
	protected Rigidbody Rigidbody { get; private set; }
	protected Health Health { get; private set; }
	protected Animations Animations { get; private set; }

	protected bool IsDead { get { return Health != null && Health.IsDead; } }

	protected bool ActionCanceled = false;

	// Use this for initialization
	void Start () {
		charges = Charges;


		Player = GameObject.FindGameObjectWithTag("Player");
		PlayerController = Player.GetComponent<PlayerController>();

		Rigidbody = Player.GetComponent<Rigidbody>();
		if (Rigidbody == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		Health = Player.GetComponent<Health>();
		Animations = Player.GetComponentInChildren<Animations>();

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
			OnChargesChanged.Invoke(charges);


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
			OnChargesChanged.Invoke(charges);
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
