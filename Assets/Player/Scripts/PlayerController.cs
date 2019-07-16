using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Component Data

	[Header("Movement")]
	public float MoveForce = 2f;
	[Range(1f, 10f)]
	public float MaxSpeed = 2f;

	[Header("Mouse Rotation")]
	public float MouseRaycastSensitivity = 15f;
	public float RaycastRange = Mathf.Infinity;
	public float RotationSpeed = 20f;
	public GameObject CursorInScene;

	[Header("Joystick Rotation")]
	public float JoystickSensitivity = 10f;

	#endregion

	#region Properties

	private bool IsDead { get { return health != null && health.IsDead; } }

	#endregion

	#region Private fields

	private Rigidbody rb;
	private Collider[] colliders;
	private Health health;
	private RectTransform cursor;
	private bool isUsingJoystickRotation = false;
	private bool IsSameMousePosition { get { return MouseDelta == Vector3.zero; } }
	private float MouseDeltaX { get { return Input.GetAxis("Mouse X") * MouseRaycastSensitivity; } }
	private float MouseDeltaY { get { return Input.GetAxis("Mouse Y") * MouseRaycastSensitivity; } }
	private Vector3 MouseDelta { get { return new Vector3(MouseDeltaX, MouseDeltaY, 0f); } }

	#endregion

	// Use this for initialization
	void Start ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		colliders = GetComponents<Collider>();

		health = GetComponent<Health>();

		GameObject cursorGo = GameObject.FindGameObjectWithTag("Cursor");
		cursor = cursorGo != null ? cursorGo.GetComponent<RectTransform>() : null;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (IsDead)
		{
			return;
		}

		UpdateVelocity();
	}

	void Update()
	{
		if (IsDead)
		{
			return;
		}

		UpdateRotation();
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
		Vector3 delta = dir * Time.deltaTime;
		rb.MovePosition(transform.position + delta);
	}

	private void UpdateRotation()
	{
		UpdateMouseRotation();
		UpdateJoystickRotation();
	}

	private void UpdateJoystickRotation()
	{
		float joystickH = Input.GetAxis("JoystickHorizontal");
		float joystickV = -Input.GetAxis("JoystickVertical");

		if (joystickH != 0 || joystickV != 0)
		{
			Vector3 dir = new Vector3(joystickH, 0, joystickV).normalized;
			LookAtDirection(dir);
			isUsingJoystickRotation = true;
		}
	}

	private void UpdateMouseRotation()
	{
		if (IsSameMousePosition)
		{
			return;
		}

		cursor.position += MouseDelta;

		Ray ray = Camera.main.ScreenPointToRay(cursor.position);
		RaycastHit mouseHit;
		if (Physics.Raycast(ray, out mouseHit, RaycastRange))
		{
			DoRotation(mouseHit);
		}

		isUsingJoystickRotation = false;
	}

	private void DoRotation(RaycastHit mouseHit)
	{
		CursorInScene.transform.position = mouseHit.point;
		Vector3 mousePos = mouseHit.point;
		mousePos.y = transform.position.y;

		Vector3 dir = (mousePos - transform.position).normalized;
		LookAtDirection(dir);
	}

	private void LookAtDirection(Vector3 dir)
	{
		Quaternion rot = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * RotationSpeed);
	}
}
