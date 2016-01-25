using UnityEngine;

public class LogoCinematic : MonoBehaviour {

	public GameObject Lean;

	public GameObject Tween;

	private void Awake(){
		
	}


	private void Start () {
		//Time.timeScale = 0.2f;
		
		// Slide in
		Tween.transform.localPosition += -Vector3.right * 15f;
		LeanTween.moveLocalX(Tween, Tween.transform.localPosition.x+15f, 0.4f).setEase(LeanTweenType.linear).setDelay(0f).setOnComplete( PlayBoom );

		// Drop Down tween down
		Tween.transform.RotateAround(Tween.transform.position, Vector3.forward, -30f);
		LeanTween.rotateAround(Tween, Vector3.forward, 30f, 0.4f).setEase(LeanTweenType.easeInQuad).setDelay(0.4f).setOnComplete( PlayBoom );

		// Drop Lean In
		Lean.transform.position += Vector3.up * 5.1f;
		LeanTween.moveY(Lean, Lean.transform.position.y-5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.6f).setOnComplete( PlayBoom );
	}

	private static void PlayBoom(){
		// Make your own Dynamic Audio at http://leanaudioplay.dentedpixel.com
		
		var volumeCurve = new AnimationCurve( new Keyframe(-0.001454365f, 0.006141067f, -3.698472f, -3.698472f), new Keyframe(0.007561419f, 1.006896f, -3.613532f, -3.613532f), new Keyframe(0.9999977f, 0.00601998f, -0.1788428f, -0.1788428f));
		var frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.001724138f, 0.01912267f, 0.01912267f), new Keyframe(0.9981073f, 0.007586207f, 0f, 0f));
		var audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new[]{ new Vector3(0.1f,0f,0f)} ).setFrequency(11025));
		LeanAudio.play(audioClip, Vector3.zero);
	}
	
}
