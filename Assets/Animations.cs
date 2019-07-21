using UnityEngine;
using UnityEngine.Events;

public class Animations : MonoBehaviour {

	public enum Weapon
	{
		SWORD = 0,
		PISTOL = 1,
		RIFLE = 1
	}

	public UnityEvent OnDoDamage;
	public UnityEvent OnDoAoe;
	public UnityEvent OnJumpBegin;
	public UnityEvent OnAttackEnded;

	private const int NB_ATTACK_ANIM = 3;

	private Animator anim;
	private PlayerStates states;

	private int currentAttackAnim = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		states = GetComponentInParent<PlayerStates>();
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

	public void SetIsMoving(bool isMoving)
	{
		anim.SetBool("isMoving", isMoving);
	}

	public void Attack()
	{
		anim.SetTrigger("Attack");
		anim.SetInteger("attackAnim", currentAttackAnim);
		currentAttackAnim++;
		currentAttackAnim %= NB_ATTACK_ANIM;
	}

	public void ResetAttackAnim()
	{
		currentAttackAnim = 0;
		anim.SetInteger("attackAnim", currentAttackAnim);
	}

	public void JumpAttack()
	{
		anim.SetTrigger("JumpAttack");
	}

	// Events

	public void AttackEndEvent()
	{
		OnAttackEnded.Invoke();
	}

	public void DoDamageEvent()
	{
		OnDoDamage.Invoke();
	}

	public void DoAoeEvent()
	{
		OnDoAoe.Invoke();
	}

	public void JumpBeginEvent()
	{
		OnJumpBegin.Invoke();
	}
}
