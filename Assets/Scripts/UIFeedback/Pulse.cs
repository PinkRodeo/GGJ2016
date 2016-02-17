using UnityEngine;

public class Pulse : MonoBehaviour
{
	public float scaleStrength = 1.5f;
	public float scaleDurationInSeconds = 0.25f;
	public float fadeDelay = 0.3f;

	void Update ()
	{
		if (Input.GetKey(KeyCode.Z))
		{
			DoIt();
		}
	}

	public void DoIt()
	{
		if (IsInvoking("Hide"))
		{
			CancelInvoke("Hide");
		}
		transform.localScale = Vector3.one;
		LeanTween.cancel(gameObject);
		var tween = LeanTween.scale(gameObject, new Vector3(scaleStrength, scaleStrength, scaleStrength), scaleDurationInSeconds);
		tween.setEase(LeanTweenType.easeOutBack);
		Invoke("Hide", scaleDurationInSeconds + fadeDelay);
	}

	public void Hide()
	{
		LeanTween.cancel(gameObject);
		transform.localScale = Vector3.zero;
	}
}
