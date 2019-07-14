using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {

	public int Damage = 30;
	public float Cooldown = 0.5f;

	public bool CanHurt { get { return canhurt; } }
	private bool canhurt = true;

	public RaycastHit? GetHit(GameObject go)
	{
		Ray ray = new Ray(transform.position, transform.up);
		int layerMask = LayerMask.GetMask(LayerMask.LayerToName(go.layer));
		RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000, layerMask);
		//Debug.Log(raycastHits.Length > 0 ? "Hit" : "No hit");
		//Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1000);
		return raycastHits.Length > 0 ? raycastHits[0] : (RaycastHit?)null;
	}

	public void StartCooldown()
	{
		StartCoroutine(cooldown(Cooldown));
	}

	private IEnumerator cooldown(float cd)
	{
		canhurt = false;
		yield return new WaitForSeconds(cd);
		canhurt = true;
	}
}
