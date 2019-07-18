using UnityEngine;
using UnityEngine.Events;

public class PlayerStates : MonoBehaviour {

	public UnityEvent OnSwitchRifle;
	public UnityEvent OnSwitchPistol;
	public UnityEvent OnSwitchSword;

	private Animations anims;

	void Start()
	{
		anims = GetComponentInChildren<Animations>();
		OnSwitchSword.Invoke();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			OnSwitchSword.Invoke();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			OnSwitchPistol.Invoke();
		}
		//else if (Input.GetKeyDown(KeyCode.Alpha3))
		//{
		//	OnSwitchRifle.Invoke();
		//}
	}

	public void SetVelocity(float hSpeed, float vSpeed)
	{
		anims.SetVelocity(hSpeed, vSpeed);
	}
}
