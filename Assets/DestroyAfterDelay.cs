using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
	public float DelayInSeconds = 5f;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(waitAndDestroy(DelayInSeconds));
    }

    IEnumerator waitAndDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
