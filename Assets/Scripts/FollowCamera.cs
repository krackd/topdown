using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[Header("Target")]
	public GameObject target;
	public float TranslationSmoothTime = 0.1f;
	public float RotationSpeed = 0.1f;
	public bool SkipYTranslation = true;

	[Header("Offset")]
	public bool UseLookAroundButton = false;
	public Vector3 CamOffset = Vector3.zero;
	public Vector3 MouseOffset = Vector3.zero;

	private RectTransform cursor;

	// Use this for initialization
	void Start()
	{
		if (target == null)
		{
			Debug.LogError("The target of follow camera could not be null!");
		}
	}

	private void Awake()
	{
		GameObject cursorGo = GameObject.FindGameObjectWithTag("Cursor");
		cursor = cursorGo != null ? cursorGo.GetComponent<RectTransform>() : null;

		UpdatePosition(false, false);
		UpdateRotation();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		UpdatePosition(true, SkipYTranslation);
	}

	private void UpdatePosition(bool isSmooth, bool skipY)
	{
		Vector3 currentVelocity = Vector3.zero;
		Vector3 targetPos = target.transform.position;
		targetPos += CamOffset;

		if (skipY)
		{
			targetPos.y = transform.position.y;
		}

		Vector3 pos = isSmooth ? Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, TranslationSmoothTime) : targetPos;
		
		if (!UseLookAroundButton || Input.GetButton("LookAround"))
		{
			pos = lookAround(pos);
		}

		transform.position = pos;
	}

	private Vector3 lookAround(Vector3 pos)
	{
		Vector3 mouse = Camera.main.ScreenToViewportPoint(cursor.position);
		mouse *= 2f;
		mouse.y -= 1f;
		mouse.x -= 1f;

		pos.z += MouseOffset.z * mouse.y;
		pos.x += MouseOffset.x * mouse.x;
		return pos;
	}

	private void UpdateRotation()
	{
		transform.LookAt(target.transform);
	}
}
