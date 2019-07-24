using UnityEngine;

public class TeleportOnMeleeAttack : Ability
{
	[Header("Teleport")]
	public float Range = 5f;
	public float TeleportDistance = 1f;
	public LayerMask AttackMask;
	public Transform[] RaycastOrigins;

	protected override void DoAction()
	{
		base.DoAction();

		RaycastHit hit = new RaycastHit();
		foreach (Transform originTransform in RaycastOrigins)
		{
			Vector3 origin = originTransform.position;
			Vector3 dir = transform.forward;
			Ray ray = new Ray(origin, dir);
			if (Physics.Raycast(ray, out hit, Range, AttackMask.value))
			{
				Health health = hit.collider.gameObject.GetComponent<Health>();
				if (health == null)
				{
					health = hit.collider.gameObject.GetComponentInParent<Health>();
				}

				if (health != null && !health.IsDead)
				{
					Vector3 pos = hit.point - dir * TeleportDistance;
					pos.y = transform.position.y;
					transform.position = pos;
				}
			}
		}
	}
}
