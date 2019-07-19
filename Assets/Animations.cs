using UnityEngine;

public class Animations : MonoBehaviour {

	public enum Weapon
	{
		SWORD = 0,
		PISTOL = 1,
		RIFLE = 1
	}

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		PlayerStates states = GetComponentInParent<PlayerStates>();
		if (states != null)
		{
			states.OnSwitchPistol.AddListener(SwitchPistol);
			states.OnSwitchRifle.AddListener(SwitchRifle);
			states.OnSwitchSword.AddListener(SwitchSword);
		}
	}

	private void SetWeapon(Weapon weapon)
	{
		anim.SetInteger("weapon", (int)weapon);
		anim.SetTrigger("SwitchWeapon");
	}

	public void SwitchSword()
	{
		SetWeapon(Weapon.SWORD);
	}

	public void SwitchPistol()
	{
		SetWeapon(Weapon.PISTOL);
	}
	
	public void SwitchRifle()
	{
		SetWeapon(Weapon.RIFLE);
	}
	
	public void SetVelocity(float hSpeed, float vSpeed)
	{
		anim.SetFloat("vSpeed", vSpeed);
		anim.SetFloat("hSpeed", hSpeed);
		anim.SetBool("isMoving", hSpeed != 0 || vSpeed != 0);
	}

	public void SetVelocity(bool isMoving)
	{
		anim.SetBool("isMoving", isMoving);
	}
}
