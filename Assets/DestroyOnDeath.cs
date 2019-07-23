using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{

	private void Start()
	{
		Health health = GetComponent<Health>();
		health.OnDeath.AddListener(OnDeath);
	}

	public void OnDeath()
	{
		Destroy(gameObject);
	}
}
