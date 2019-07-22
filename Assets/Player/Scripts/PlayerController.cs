using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private const float MOVE_CONTROL_REDUCTION_FACTOR = 0.5f;

	#region Component Data

	[Header("Movement")]
	public float MoveForce = 2f;
	private float moveForce;
	[Range(0.01f, 10f)]
	private bool canMove = true;

	[Header("Mouse Rotation")]
	public float RaycastRange = Mathf.Infinity;
	public float RotationSpeed = 20f;
	public GameObject CursorInScene;

	[Header("Joystick Rotation")]
	public float JoystickSensitivity = 10f;

	[Header("Attack")]
	public float AttackRechargeDelayInSeconds = 3f;
	public int MaxAttackCharges = 3;
	private int attackCharges;
	private bool canAttack = true;
	private Coroutine attackCoroutine;

	[Header("Dash")]
	public float DashVelocity = 5f;
	public float DashDurationInSeconds = 0.5f;
	public float DashRechargeCooldown = 3f;
	public int DashCharges = 3;
	private int dashCharges;

	[Header("Jump")]
	public float JumpVelocity = 5f;
	public float JumpDurationInSeconds = 0.5f;
	public float JumpRechargeCooldown = 3f;
	private bool canJumpAttack = true;

	#endregion

	#region Properties

	private bool IsDead { get { return health != null && health.IsDead; } }

	#endregion

	#region Private fields

	private Rigidbody rb;
	private Collider[] colliders;
	private Health health;
	private PlayerStates states;
	private Animations anims;
	private RectTransform cursor;
	private Vector3 moveDir;
	
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

		colliders = GetComponents<Collider>();

		ResetMoveControl();

		health = GetComponent<Health>();
		states = GetComponent<PlayerStates>();
		anims = GetComponentInChildren<Animations>();
		anims.OnAttackEnded.AddListener(AttackEndEvent);
		anims.OnDoDamage.AddListener(DoDamageEvent);
		anims.OnDoAoe.AddListener(DoAoeEvent);
		anims.OnJumpBegin.AddListener(JumpBeginEvent);

		GameObject cursorGo = GameObject.FindGameObjectWithTag("Cursor");
		cursor = cursorGo != null ? cursorGo.GetComponent<RectTransform>() : null;

		dashCharges = DashCharges;
		attackCharges = MaxAttackCharges;
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
		UpdateAttack();
		UpdateDash();
		UpdateJumpAttack();

		previousMousePos = Input.mousePosition;
	}

	private void UpdateDash()
	{
		if (dashCharges <= 0 || !canMove)
		{
			return;
		}

		if (Input.GetButtonDown("Dash"))
		{
			rb.velocity = moveDir * DashVelocity;
			canMove = false;
			dashCharges--;
			timeout(DashDurationInSeconds, () =>
			{
				rb.velocity = Vector3.zero;
				canMove = true;
				timeout(DashRechargeCooldown, () =>
				{
					dashCharges++;
				});
			});
		}
	}

	private void UpdateJumpAttack()
	{
		if (!canJumpAttack)
		{
			return;
		}

		if (Input.GetButtonDown("Jump"))
		{
			canJumpAttack = false;
			canAttack = false;
			// JumpBeginEvent will chang the velocity
			anims.JumpAttack();
		}
	}

	public void JumpBeginEvent()
	{
		DoJump();
	}
	
	private void DoJump()
	{
		rb.velocity = (transform.forward + transform.up).normalized * JumpVelocity;
		ReduceMoveControl();

		timeout(JumpDurationInSeconds * 0.6f, () =>
		{
			rb.velocity = -transform.up * JumpVelocity;
		});

		timeout(JumpDurationInSeconds, () =>
		{
			rb.velocity = Vector3.zero;
			canMove = true;
		});

		timeout(JumpRechargeCooldown, () =>
		{
			canJumpAttack = true;
		});
	}

	private void UpdateAttack()
	{
		if (attackCharges <= 0 || !canAttack)
		{
			return;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			anims.Attack();

			RestartResetAttackCharges();

			canAttack = false;
			ReduceMoveControl();
			attackCharges--;
		}
	}

	private void RestartResetAttackCharges()
	{
		if (attackCoroutine != null)
		{
			StopCoroutine(attackCoroutine);
		}

		attackCoroutine = timeout(AttackRechargeDelayInSeconds, () =>
		{
			attackCharges = MaxAttackCharges;
			anims.ResetAttackAnim();
			attackCoroutine = null;
		});
	}

	public void AttackEndEvent()
	{
		canAttack = true;
		ResetMoveControl();
	}

	public void DoDamageEvent()
	{
		
	}

	public void DoAoeEvent()
	{

	}

	private void UpdateVelocity()
	{
		if (!canMove)
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
		moveDir = dir.normalized;
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

	private void ResetMoveControl()
	{
		moveForce = MoveForce;
	}

	private void ReduceMoveControl()
	{
		moveForce = MoveForce * MOVE_CONTROL_REDUCTION_FACTOR;
	}

	private bool noModifierPressed()
	{
		return !anyModifierPressed();
	}

	private bool anyModifierPressed()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand);
	}

	private Coroutine timeout(float seconds, System.Action action)
	{
		return StartCoroutine(timeoutCoroutine(seconds, action));
	}

	private IEnumerator timeoutCoroutine(float seconds, System.Action action)
	{
		yield return new WaitForSeconds(seconds);
		action.Invoke();
	}
}
