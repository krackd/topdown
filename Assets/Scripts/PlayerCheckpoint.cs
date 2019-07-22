using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour {

	public float WorldLimitY = -10f;

	[Header("Current checkpoint")]
	public Checkpoint Checkpoint;

	private Vector3 startPosition;

	void Start()
	{
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y <= WorldLimitY)
		{
			transform.position = Checkpoint != null ? Checkpoint.CheckpointPosition : startPosition;
		}
	}
}
