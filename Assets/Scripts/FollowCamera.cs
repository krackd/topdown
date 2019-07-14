using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public GameObject target;
	public float TranslationSmoothTime = 0.1f;
	public float RotationSpeed = 0.1f;

	[Header("Offset")]
	public Vector3 CamOffset = Vector3.zero;
	public Vector3 MouseOffset = Vector3.zero;

	// Use this for initialization
	void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;

		if (target == null)
		{
			Debug.LogError("The target of follow camera could not be null!");
		}
	}

	private void Awake()
	{
		UpdatePosition(false);
		UpdateRotation();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		UpdatePosition(true);
	}

	private void UpdatePosition(bool isSmooth)
	{
		Vector3 currentVelocity = Vector3.zero;
		Vector3 targetPos = target.transform.position;
		targetPos += CamOffset;
		Vector3 pos = isSmooth ? Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, TranslationSmoothTime) : targetPos;
		
		Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		mouse *= 2f;
		mouse.y -= 1f;
		mouse.x -= 1f;

		pos.z += MouseOffset.z * mouse.y;
		pos.x += MouseOffset.x * mouse.x;

		transform.position = pos;
	}

	private void UpdateRotation()
	{
		transform.LookAt(target.transform);
	}
}
