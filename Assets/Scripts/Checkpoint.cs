using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public Vector3 CheckpointPosition { get; private set; }

	void Start()
	{
		CheckpointPosition = GetComponentInChildren<CheckpointPosition>().transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerCheckpoint pc = other.GetComponent<PlayerCheckpoint>();
			if (pc == null)
			{
				pc = other.GetComponentInParent<PlayerCheckpoint>();
			}

			if (pc != null)
			{
				UpdateCheckpoint(pc);
			}
		}
	}

	private void UpdateCheckpoint(PlayerCheckpoint pc)
	{
		pc.Checkpoint = this;
	}
}
