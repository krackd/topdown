using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Component Data

	[Header("Movement")]
	public float MoveForce = 2f;
	[Range(1f, 10f)]
	public float MaxSpeed = 2f;

	[Header("Rotation")]
	public float RotationSpeed = 20f;
	public LayerMask MouseMask;

	#endregion

	#region Properties

	private bool IsDead { get { return health != null && health.IsDead; } }

	#endregion

	#region Private fields

	private Rigidbody rb;
	private Collider[] colliders;
	private Health health;

	#endregion

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		colliders = GetComponents<Collider>();

		health = GetComponent<Health>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (IsDead)
		{
			return;
		}

		UpdateVelocity();
		UpdateRotation();
	}

	void Update()
	{
		if (IsDead)
		{
			return;
		}


	}

	private void UpdateVelocity()
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		Vector3 vDir = Vector3.forward * v;
		Vector3 hDir = Vector3.right * h;
		Vector3 dir = vDir + hDir;
		if (dir.sqrMagnitude > 1)
		{
			dir.Normalize();
		}
		dir *= MoveForce;
		rb.MovePosition(transform.position + dir * Time.deltaTime);

	}

	private void UpdateRotation()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit mouseHit;
		if (Physics.Raycast(ray, out mouseHit, MouseMask.value))
		{
			DoRotation(mouseHit);
		}
	}

	private void DoRotation(RaycastHit mouseHit)
	{
		Vector3 mousePos = mouseHit.point;
		mousePos.y = transform.position.y;

		Vector3 dir = (mousePos - transform.position).normalized;
		Quaternion rot = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * RotationSpeed);
	}
}
