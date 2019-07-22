using System.Collections;
using UnityEngine;

public class CoroutineUtils {

	public static Coroutine timeout(MonoBehaviour mono, float seconds, System.Action action)
	{
		return mono.StartCoroutine(timeoutCoroutine(seconds, action));
	}

	private static IEnumerator timeoutCoroutine(float seconds, System.Action action)
	{
		yield return new WaitForSeconds(seconds);
		action.Invoke();
	}
}
