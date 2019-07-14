using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 Direction = Vector3.up;
	public float Speed = 1f;

	public int Damage = 1;

	public string[] TagsToIgnore;

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(Direction * Speed * Time.deltaTime * 1000f);
	}

	private void OnTriggerEnter(Collider other)
	{
		DoDestroy(other.gameObject);
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		DoDestroy(collision.gameObject);
	}

	private void DoDestroy(GameObject other)
	{
		if (!shouldIgnore(other))
		{
			Destroy(gameObject);
		}
	}

	private bool shouldIgnore(GameObject otherGo)
	{
		foreach (string tag in TagsToIgnore)
		{
			if (otherGo.CompareTag(tag))
			{
				return true;
			}
		}
		return false;
	}
}
