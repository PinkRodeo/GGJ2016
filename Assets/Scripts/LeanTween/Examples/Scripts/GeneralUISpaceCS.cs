using UnityEngine;

public class GeneralUiSpaceCS : MonoBehaviour {

	public RectTransform MainWindow;
	public RectTransform MainParagraphText;
	public RectTransform MainTitleText;
	public RectTransform MainButton1;
	public RectTransform MainButton2;

	public RectTransform PauseRing1;
	public RectTransform PauseRing2;
	public RectTransform PauseWindow;

	public RectTransform ChatWindow;
	public RectTransform ChatRect;
	public Sprite[] ChatSprites;
	public RectTransform ChatBar1;
	public RectTransform ChatBar2;

	private void Start () {
		//Time.timeScale = 1f/4f;
		
		// *********** Main Window **********
		// Scale the whole window in
		MainWindow.localScale = Vector3.zero;
		LeanTween.scale( MainWindow, new Vector3(1f,1f,1f), 0.6f).setEase(LeanTweenType.easeOutBack);

		// Fade the main paragraph in while moving upwards
		MainParagraphText.anchoredPosition3D += new Vector3(0f,-10f,0f);
		LeanTween.textAlpha( MainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
		LeanTween.textAlpha( MainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.move( MainParagraphText, MainParagraphText.anchoredPosition3D + new Vector3(0f,10f,0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);

		// Flash text to purple and back
		LeanTween.textColor( MainTitleText, new Color(133f/255f,145f/255f,223f/255f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);

		// Fade button in
		LeanTween.textAlpha(MainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.alpha(MainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);


		// *********** Pause Button **********
		// Drop pause button in
		PauseWindow.anchoredPosition3D += new Vector3(0f,200f,0f);
		LeanTween.moveY( PauseWindow, PauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);

		// Punch Pause Symbol
		var pauseText = PauseWindow.Find("PauseText").GetComponent<RectTransform>();
		LeanTween.moveZ( pauseText, pauseText.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2.0f);

		// Rotate rings around in opposite directions
		LeanTween.rotateAroundLocal(PauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
		LeanTween.rotateAroundLocal(PauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		

		// *********** Chat Window **********
		// Flip the chat window in
		ChatWindow.RotateAround(ChatWindow.position, Vector3.up, -180f);
		LeanTween.rotateAround(ChatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);

		// Play a series of sprites on the window on repeat endlessly
		LeanTween.play(ChatRect, ChatSprites).setLoopPingPong();

		// Animate the bar up and down while changing the color to red-ish
		LeanTween.color( ChatBar2, new Color(248f/255f,67f/255f,108f/255f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
		LeanTween.scale( ChatBar2, new Vector2(1f,0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
	}

}
