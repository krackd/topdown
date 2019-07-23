using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public UnityEvent OnDeath;
	public UnityEvent OnResurect;

	[Header("Health")]
	public int MaxHP = 1;
	public bool IsInvincible = false;

	[Header("Modifiers")]
	public float ProjectileDamageFactor = 1f;
	public float MeleeDamageFactor = 1f;

	public bool IsDead { get { return hp <= 0; } }


	private int hp;

	// Use this for initialization
	void Start () {
		hp = MaxHP;
	}

	private void OnTriggerEnter(Collider other)
	{
		OnCollision(other.gameObject);
	}

	private void OnTriggerStay(Collider other)
	{
		OnCollision(other.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		OnCollision(collision.gameObject);
	}

	private void OnCollisionStay(Collision collision)
	{
		OnCollision(collision.gameObject);
	}

	private void OnCollision(GameObject otherGo)
	{
		Projectile projectile = otherGo.GetComponent<Projectile>();
		if (projectile != null)
		{
			Hurt((int)(projectile.Damage * ProjectileDamageFactor));
		}

		MeleeWeapon beam = otherGo.GetComponentInParent<MeleeWeapon>();
		if (beam != null && beam.CanHurt)
		{
			Hurt((int)(beam.Damage * MeleeDamageFactor));
			beam.StartCooldown();
		}
	}

	public void Hurt(int amount)
	{
		if (IsInvincible || IsDead)
		{
			return;
		}

		hp = clamp(hp - amount);
		Debug.Log(gameObject.name + " hp: " + hp);
		if (IsDead)
		{
			Die();
		}
	}

	public void Heal(int amount)
	{
		if (IsDead)
		{
			return;
		}

		hp = clamp(hp + amount);
	}

	public void Die()
	{
		OnDeath.Invoke();
	}

	public void Resurect()
	{
		hp = MaxHP / 2;
		OnResurect.Invoke();
	}

		private int clamp(int value)
	{
		return hp = (int)Mathf.Clamp(value, 0, MaxHP);
	}
}
