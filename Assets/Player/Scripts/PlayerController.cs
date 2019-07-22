using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//private const float MOVE_CONTROL_REDUCTION_FACTOR = 0.4f;

	#region Component Data

	[Header("Movement")]
	public float MoveForce = 2f;
	private float moveForce;
	public bool CanMove = true;
	public Vector3 MoveDir { get; private set; }

	[Header("Mouse Rotation")]
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
	private Health health;
	private Animations anims;
	private RectTransform cursor;
	
	private Vector3 previousMousePos;
	private bool IsSameMousePosition { get { return Input.mousePosition.Equals(previousMousePos); } }

	#endregion

	// Use this for initialization
	void Start ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No rigid body found in player!");
		}

		ResetMoveControl();

		health = GetComponent<Health>();
		anims = GetComponentInChildren<Animations>();
		
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

		previousMousePos = Input.mousePosition;
	}

	private void UpdateVelocity()
	{
		if (!CanMove)
		{
			return;
		}

		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		Vector3 vDir = Vector3.forward * v;
		Vector3 hDir = Vector3.right * h;
		Vector3 dir = vDir + hDir;
		if (dir.sqrMagnitude > 1)
		{
			dir.Normalize();
		}
		dir *= moveForce;
		Vector3 delta = dir * Time.deltaTime;
		rb.MovePosition(transform.position + delta);

		Vector3 rotatedDir = (Quaternion.Inverse(transform.rotation) * dir).normalized;
		anims.SetVelocity(rotatedDir.x, rotatedDir.z);
		anims.SetIsMoving(Input.GetButton("Vertical") || Input.GetButton("Horizontal"));
		MoveDir = dir.normalized;
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
		}
	}

	private void UpdateMouseRotation()
	{
		if (IsSameMousePosition)
		{
			return;
		}

		cursor.position = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay(cursor.position);
		RaycastHit mouseHit;
		if (Physics.Raycast(ray, out mouseHit, RaycastRange))
		{
			DoRotation(mouseHit);
		}
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

	public void ResetMoveControl()
	{
		moveForce = MoveForce;
	}

	public void ReduceMoveControl(float moveReductionFactor)
	{
		moveForce = MoveForce * moveReductionFactor;
	}
}
