using UnityEngine;

public class Animations : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		PlayerStates states = GetComponentInParent<PlayerStates>();
		if (states != null)
		{
			Debug.Log("PlayerStates found");
			states.OnSwitchPistol.AddListener(SwitchPistol);
			states.OnSwitchRifle.AddListener(SwitchRifle);
			states.OnSwitchSword.AddListener(SwitchSword);
		}
	}

	public void SwitchPistol()
	{
		anim.SetTrigger("SwitchPistol");
	}

	public void SwitchRifle()
	{
		anim.SetTrigger("SwitchRifle");
	}

	public void SwitchSword()
	{
		anim.SetTrigger("SwitchSword");
	}

	public void SetVelocity(float hSpeed, float vSpeed)
	{
		anim.SetFloat("vSpeed", vSpeed);
		anim.SetFloat("hSpeed", hSpeed);
	}
}
