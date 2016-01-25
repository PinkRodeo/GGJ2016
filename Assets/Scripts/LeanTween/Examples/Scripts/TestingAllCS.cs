using System;
using System.Collections;
using UnityEngine;

public class TestingAllCs : MonoBehaviour {
	public AnimationCurve CustomAnimationCurve;
	public Transform Pt1;
	public Transform Pt2;
	public Transform Pt3;
	public Transform Pt4;
	public Transform Pt5;
	
	public delegate void NextFunc();
	private int _exampleIter;
	private readonly string[] _exampleFunctions = { /**/"updateValue3Example", "loopTestClamp", "loopTestPingPong", "moveOnACurveExample", "customTweenExample", "moveExample", "rotateExample", "scaleExample", "updateValueExample", "delayedCallExample", "alphaExample", "moveLocalExample", "rotateAroundExample", "colorExample" };
	private bool _useEstimatedTime = true;
	private GameObject _ltLogo;
	private TimingType _timingType = TimingType.SteadyNormalTime;
	private int _descrTimeScaleChangeId;
	private Vector3 _origin;

	public enum TimingType{
		SteadyNormalTime,
		IgnoreTimeScale,
		HalfTimeScale,
		VariableTimeScale,
		Length
	}

	private void Awake(){
		// LeanTween.init(3200); // This line is optional. Here you can specify the maximum number of tweens you will use (the default is 400).  This must be called before any use of LeanTween is made for it to be effective.
	}

	private void Start () {
		_ltLogo = GameObject.Find("LeanTweenLogo");
		LeanTween.delayedCall(1f, CycleThroughExamples);
		_origin = _ltLogo.transform.position;
		
		//LeanTween.move( ltLogo, Vector3.zero, 10f);
		//LeanTween.delayedCall(2f, pauseNow);
		//LeanTween.delayedCall(5,loopPause);
		//LeanTween.delayedCall(8, loopResume);
	}

	private void PauseNow(){
		Time.timeScale = 0f;
		Debug.Log("pausing");
	}

	private void OnGUI(){
		var label = _useEstimatedTime ? "useEstimatedTime" : "timeScale:"+Time.timeScale;
		GUI.Label(new Rect(0.03f*Screen.width,0.03f*Screen.height,0.5f*Screen.width,0.3f*Screen.height), label);
	}

	private void EndlessCallback(){
		Debug.Log("endless");
	}

	private void CycleThroughExamples(){
		if(_exampleIter==0){
			var iter = (int)_timingType + 1;
			if(iter>(int)TimingType.Length)
				iter = 0;
			_timingType = (TimingType)iter;
			_useEstimatedTime = _timingType==TimingType.IgnoreTimeScale;
			Time.timeScale = _useEstimatedTime ? 0 : 1f; // pause the Time Scale to show the effectiveness of the useEstimatedTime feature (this is very usefull with Pause Screens)
			if(_timingType==TimingType.HalfTimeScale)
				Time.timeScale = 0.5f;

			if(_timingType==TimingType.VariableTimeScale){
				_descrTimeScaleChangeId = LeanTween.value( gameObject, 0.01f, 10.0f, 3f).setOnUpdate( (float val)=>{
					//Debug.Log("timeScale val:"+val);
					Time.timeScale = val;
				}).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
			}else{
				Debug.Log("cancel variable time");
				LeanTween.cancel( _descrTimeScaleChangeId );
			}
		}
		gameObject.BroadcastMessage( _exampleFunctions[ _exampleIter ] );

		// Debug.Log("cycleThroughExamples time:"+Time.time + " useEstimatedTime:"+useEstimatedTime);
		const float delayTime = 1.1f;
		LeanTween.delayedCall( gameObject, delayTime, CycleThroughExamples).setUseEstimatedTime(_useEstimatedTime);

		_exampleIter = _exampleIter+1>=_exampleFunctions.Length ? 0 : _exampleIter + 1;
	}

	public void UpdateValue3Example(){
		Debug.Log("updateValue3Example Time:"+Time.time);
		LeanTween.value( gameObject, UpdateValue3ExampleCallback, new Vector3(0.0f, 270.0f, 0.0f), new Vector3(30.0f, 270.0f, 180f), 0.5f ).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(UpdateValue3ExampleUpdate).setUseEstimatedTime(_useEstimatedTime);
	}

	public void UpdateValue3ExampleUpdate( Vector3 val){
		//Debug.Log("val:"+val+" obj:"+obj);
	}

	public void UpdateValue3ExampleCallback( Vector3 val ){
		_ltLogo.transform.eulerAngles = val;
		// Debug.Log("updateValue3ExampleCallback:"+val);
	}

	public void LoopTestClamp(){
		Debug.Log("loopTestClamp Time:"+Time.time);
		var cube1 = GameObject.Find("Cube1");
		cube1.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		LeanTween.scaleZ( cube1, 4.0f, 1.0f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(_useEstimatedTime);//
	}

	public void LoopTestPingPong(){
		Debug.Log("loopTestPingPong Time:"+Time.time);
		var cube2 = GameObject.Find("Cube2");
		cube2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		LeanTween.scaleY( cube2, 4.0f, 1.0f ).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(_useEstimatedTime);
		//LeanTween.scaleY( cube2, 4.0f, 1.0f, LeanTween.options().setEaseOutQuad().setRepeat(8).setLoopPingPong().setUseEstimatedTime(useEstimatedTime) );
	}

	public void ColorExample(){
		var lChar = GameObject.Find("LCharacter");
		LeanTween.color( lChar, new Color(1.0f,0.0f,0.0f,0.5f), 0.5f ).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(_useEstimatedTime);
	}

	public void MoveOnACurveExample(){
		Debug.Log("moveOnACurveExample Time:"+Time.time);

		var path = new[] { _origin,Pt1.position,Pt2.position,Pt3.position,Pt3.position,Pt4.position,Pt5.position,_origin};
		LeanTween.move( _ltLogo, path, 1.0f ).setEase(LeanTweenType.easeOutQuad).setOrientToPath(true).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void CustomTweenExample(){
		Debug.Log("customTweenExample starting pos:"+_ltLogo.transform.position+" origin:"+_origin);
		
		LeanTween.moveX( _ltLogo, -10.0f, 0.5f ).setEase(CustomAnimationCurve).setUseEstimatedTime(_useEstimatedTime);
		LeanTween.moveX( _ltLogo, 0.0f, 0.5f ).setEase(CustomAnimationCurve).setDelay(0.5f).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void MoveExample(){
		Debug.Log("moveExample");
		
		LeanTween.move( _ltLogo, new Vector3(-2f,-1f,0f), 0.5f).setUseEstimatedTime(_useEstimatedTime);
		LeanTween.move( _ltLogo, _origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void RotateExample(){
		Debug.Log("rotateExample");

		var returnParam = new Hashtable {{"yo", 5.0}};

		LeanTween.rotate( _ltLogo, new Vector3(0f,360f,0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(RotateFinished).setOnCompleteParam(returnParam).setOnUpdate(RotateOnUpdate).setUseEstimatedTime(_useEstimatedTime);
	}

	public void RotateOnUpdate( float val ){
		//Debug.Log("rotating val:"+val);
	}

	public void RotateFinished( object hash )
	{
		var h = hash as Hashtable;
		if (h != null) Debug.Log("rotateFinished hash:"+h["yo"]);
	}

	public void ScaleExample(){
		Debug.Log("scaleExample");
		
		var currentScale = _ltLogo.transform.localScale;
		LeanTween.scale( _ltLogo, new Vector3(currentScale.x+0.2f,currentScale.y+0.2f,currentScale.z+0.2f), 1f ).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void UpdateValueExample(){
		Debug.Log("updateValueExample");
		var pass = new Hashtable {{"message", "hi"}};
		LeanTween.value( gameObject, (Action<float, object>) UpdateValueExampleCallback, _ltLogo.transform.eulerAngles.y, 270f, 1f ).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(pass).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void UpdateValueExampleCallback( float val, object hash ){
		// Hashtable h = hash as Hashtable;
		// Debug.Log("message:"+h["message"]+" val:"+val);
		var tmp = _ltLogo.transform.eulerAngles;
		tmp.y = val;
		_ltLogo.transform.eulerAngles = tmp;
	}
	
	public void DelayedCallExample(){
		Debug.Log("delayedCallExample");
		
		LeanTween.delayedCall(0.5f, DelayedCallExampleCallback).setUseEstimatedTime(_useEstimatedTime);
	}
	
	public void DelayedCallExampleCallback(){
		Debug.Log("Delayed function was called");
		var currentScale = _ltLogo.transform.localScale;

		LeanTween.scale( _ltLogo, new Vector3(currentScale.x-0.2f,currentScale.y-0.2f,currentScale.z-0.2f), 0.5f ).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(_useEstimatedTime);
	}

	public void AlphaExample(){
		Debug.Log("alphaExample");
		
		var lChar = GameObject.Find ("LCharacter");
		LeanTween.alpha( lChar, 0.0f, 0.5f ).setUseEstimatedTime(_useEstimatedTime);
		LeanTween.alpha( lChar, 1.0f, 0.5f ).setDelay(0.5f).setUseEstimatedTime(_useEstimatedTime);
	}

	public void MoveLocalExample(){
		Debug.Log("moveLocalExample");
		
		var lChar = GameObject.Find ("LCharacter");
		var origPos = lChar.transform.localPosition;
		LeanTween.moveLocal( lChar, new Vector3(0.0f,2.0f,0.0f), 0.5f ).setUseEstimatedTime(_useEstimatedTime);
		LeanTween.moveLocal( lChar, origPos, 0.5f ).setDelay(0.5f).setUseEstimatedTime(_useEstimatedTime);
	}

	public void RotateAroundExample(){
		Debug.Log("rotateAroundExample");
		
		var lChar = GameObject.Find ("LCharacter");
		LeanTween.rotateAround( lChar, Vector3.up, 360.0f, 1.0f ).setUseEstimatedTime(_useEstimatedTime);
	}

	public void LoopPause(){
		var cube1 = GameObject.Find("Cube1");
		LeanTween.pause(cube1);
	}

	public void LoopResume(){
		var cube1 = GameObject.Find("Cube1");
		LeanTween.resume(cube1 );
	}

	public void PunchTest(){
		LeanTween.moveX( _ltLogo, 7.0f, 1.0f ).setEase(LeanTweenType.punch).setUseEstimatedTime(_useEstimatedTime);
	}
}
