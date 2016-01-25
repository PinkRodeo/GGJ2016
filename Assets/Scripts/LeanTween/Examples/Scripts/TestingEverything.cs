using System.Collections;
using UnityEngine;

public class TempTestingCancel : MonoBehaviour {
    public bool IsTweening;
    public bool TweenOverride = false;
    private LTDescr _tween;
 
    // Use this for initialization
	private void Start () {
        _tween = LeanTween.move(gameObject, transform.position + Vector3.one*3f, Random.Range(2,2) ).setRepeat(-1).setLoopClamp ();
    }
 
    public void Update () {
	    if (_tween == null) return;
	    IsTweening = LeanTween.isTweening(gameObject);
	    if(TweenOverride){
             
		    // this next line works  
		    //tween.cancel();
 
		    // this one doesn't
		    LeanTween.cancel(gameObject);
	    }
    }
}

public class TestingEverything : MonoBehaviour {

	public GameObject Cube1;
	public GameObject Cube2;
	public GameObject Cube3;
	public GameObject Cube4;


	private bool _eventGameObjectWasCalled, _eventGeneralWasCalled;
	private int _lt1Id;
	private LTDescr _lt2;
	private LTDescr _lt3;
	private LTDescr _lt4;
	private LTDescr[] _groupTweens;
	private GameObject[] _groupGOs;
	private int _groupTweensCnt;
	private int _rotateRepeat;
	private int _rotateRepeatAngle;
	private GameObject _boxNoCollider;
	private float _timeElapsedNormalTimeScale;
	private float _timeElapsedIgnoreTimeScale;

	private void Awake(){
		_boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Destroy( _boxNoCollider.GetComponent( typeof(BoxCollider) ) );
	}

	private void Start () {
		LeanTest.timeout = 45f;
		LeanTest.expected = 31;

		LeanTween.init(10 + 1200);
		// add a listener
		LeanTween.addListener(Cube1, 0, EventGameObjectCalled);

		LeanTest.expect(LeanTween.isTweening() == false, "NOTHING TWEEENING AT BEGINNING" );

		LeanTest.expect(LeanTween.isTweening(Cube1) == false, "OBJECT NOT TWEEENING AT BEGINNING" );

		LeanTween.scaleX( Cube4, 2f, 0f).setOnComplete( ()=>{
			LeanTest.expect( Cube4.transform.localScale.x == 2f, "TWEENED WITH ZERO TIME" );
		});

		// dispatch event that is received
		LeanTween.dispatchEvent(0);
		LeanTest.expect( _eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED" );

		// do not remove listener
		LeanTest.expect(LeanTween.removeListener(Cube2, 0, EventGameObjectCalled)==false, "EVENT GAMEOBJECT NOT REMOVED" );
		// remove listener
		LeanTest.expect(LeanTween.removeListener(Cube1, 0, EventGameObjectCalled), "EVENT GAMEOBJECT REMOVED" );

		// add a listener
		LeanTween.addListener(1, EventGeneralCalled);
		
		// dispatch event that is received
		LeanTween.dispatchEvent(1);
		LeanTest.expect( _eventGeneralWasCalled, "EVENT ALL RECEIVED" );

		// remove listener
		LeanTest.expect( LeanTween.removeListener( 1, EventGeneralCalled), "EVENT ALL REMOVED" );

		_lt1Id = LeanTween.move( Cube1, new Vector3(3f,2f,0.5f), 1.1f ).id;
		LeanTween.move( Cube2, new Vector3(-3f,-2f,-0.5f), 1.1f );

		LeanTween.reset();


		var cr = new LTSpline(new Vector3(-1f,0f,0f), new Vector3(0f,0f,0f), new Vector3(4f,0f,0f), new Vector3(20f,0f,0f), new Vector3(30f,0f,0f));
		cr.place( Cube4.transform, 0.5f );
		LeanTest.expect( Vector3.Distance( Cube4.transform.position, new Vector3(10f,0f,0f) ) <= 0.7f, "SPLINE POSITIONING AT HALFWAY", "position is:"+Cube4.transform.position+" but should be:(10f,0f,0f)");
		LeanTween.color(Cube4, Color.green, 0.01f);	

		// OnStart Speed Test for ignoreTimeScale vs normal timeScale
		var cube = Instantiate( _boxNoCollider );
		cube.name = "normalTimeScale";
		// float timeElapsedNormal = Time.time;
		LeanTween.moveX(cube, 12f, 1f).setIgnoreTimeScale( false ).setOnComplete( ()=>{
			_timeElapsedNormalTimeScale = Time.time;
		});

		var descr = LeanTween.descriptions( cube );
		LeanTest.expect( descr.Length >= 0 && descr[0].to.x == 12f, "WE CAN RETRIEVE A DESCRIPTION");

		cube = Instantiate( _boxNoCollider );
		cube.name = "ignoreTimeScale";
		LeanTween.moveX(cube, 5f, 1f).setIgnoreTimeScale( true ).setOnComplete( ()=>{
			_timeElapsedIgnoreTimeScale = Time.time;
		});

		StartCoroutine( TimeBasedTesting() );
	}

	private IEnumerator TimeBasedTesting(){
		yield return new WaitForSeconds(1);
		
		yield return new WaitForEndOfFrame();

		LeanTest.expect( Mathf.Abs( _timeElapsedNormalTimeScale - _timeElapsedIgnoreTimeScale ) < 0.15f, "START IGNORE TIMING", "timeElapsedIgnoreTimeScale:"+_timeElapsedIgnoreTimeScale+" timeElapsedNormalTimeScale:"+_timeElapsedNormalTimeScale );

		Time.timeScale = 4f;

		var pauseCount = 0;
		LeanTween.value( gameObject, 0f, 1f, 1f).setOnUpdate( ( float val )=>{
			pauseCount++;
		}).pause();

		// Bezier should end at exact end position not just 99% close to it
		var roundCirc = new[]{ new Vector3(0f,0f,0f), new Vector3(-9.1f,25.1f,0f), new Vector3(-1.2f,15.9f,0f), new Vector3(-25f,25f,0f), new Vector3(-25f,25f,0f), new Vector3(-50.1f,15.9f,0f), new Vector3(-40.9f,25.1f,0f), new Vector3(-50f,0f,0f), new Vector3(-50f,0f,0f), new Vector3(-40.9f,-25.1f,0f), new Vector3(-50.1f,-15.9f,0f), new Vector3(-25f,-25f,0f), new Vector3(-25f,-25f,0f), new Vector3(0f,-15.9f,0f), new Vector3(-9.1f,-25.1f,0f), new Vector3(0f,0f,0f) };
		var cubeRound = Instantiate( _boxNoCollider );
		cubeRound.name = "bRound";
		var onStartPos = cubeRound.transform.position;
		LeanTween.moveLocal(cubeRound, roundCirc, 0.5f).setOnComplete( ()=>{
			LeanTest.expect(cubeRound.transform.position==onStartPos, "BEZIER CLOSED LOOP SHOULD END AT START","onStartPos:"+onStartPos+" onEnd:"+cubeRound.transform.position);
		});

		// Spline should end at exact end position not just 99% close to it
		var roundSpline = new[]{ new Vector3(0f,0f,0f), new Vector3(0f,0f,0f), new Vector3(2f,0f,0f), new Vector3(0.9f,2f,0f), new Vector3(0f,0f,0f), new Vector3(0f,0f,0f) };
		var cubeSpline = Instantiate( _boxNoCollider );
		cubeSpline.name = "bSpline";
		var onStartPosSpline = cubeSpline.transform.position;
		LeanTween.moveSplineLocal(cubeSpline, roundSpline, 0.5f).setOnComplete( ()=>{
			LeanTest.expect(Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.01f, "BEZIER CLOSED LOOP SHOULD END AT START","onStartPos:"+onStartPosSpline+" onEnd:"+cubeSpline.transform.position+" dist:"+Vector3.Distance(onStartPosSpline, cubeSpline.transform.position));
		});


		
		
		// Groups of tweens testing
		_groupTweens = new LTDescr[ 1200 ];
		_groupGOs = new GameObject[ _groupTweens.Length ];
		_groupTweensCnt = 0;
		var descriptionMatchCount = 0;
		for(var i = 0; i < _groupTweens.Length; i++){
			var cube = Instantiate( _boxNoCollider );
			cube.name = "c"+i;
			cube.transform.position = new Vector3(0,0,i*3);
			
			_groupGOs[i] = cube;
		}

		yield return new WaitForEndOfFrame();

		var hasGroupTweensCheckStarted = false;
		var setOnStartNum = 0;
		for(var i = 0; i < _groupTweens.Length; i++){
			_groupTweens[i] = LeanTween.move(_groupGOs[i], transform.position + Vector3.one*3f, 3f ).setOnStart( ()=>{
				setOnStartNum++;
			}).setOnComplete( ()=>{
				if(hasGroupTweensCheckStarted==false){
					hasGroupTweensCheckStarted = true;
					LeanTween.delayedCall(gameObject, 0.1f, ()=>{
						LeanTest.expect( setOnStartNum == _groupTweens.Length, "SETONSTART CALLS", "expected:"+_groupTweens.Length+" was:"+setOnStartNum);
						LeanTest.expect( _groupTweensCnt==_groupTweens.Length, "GROUP FINISH", "expected "+_groupTweens.Length+" tweens but got "+_groupTweensCnt);
					});
				}
				_groupTweensCnt++;
			});

			if(LeanTween.description(_groupTweens[i].id).trans==_groupTweens[i].trans)
				descriptionMatchCount++;
		}

		while (LeanTween.tweensRunning<_groupTweens.Length)
			yield return null;

		LeanTest.expect( descriptionMatchCount==_groupTweens.Length, "GROUP IDS MATCH" );
		LeanTest.expect( LeanTween.maxSearch<=_groupTweens.Length+5, "MAX SEARCH OPTIMIZED", "maxSearch:"+LeanTween.maxSearch );
		LeanTest.expect( LeanTween.isTweening(), "SOMETHING IS TWEENING" );

		// resume item before calling pause should continue item along it's way
		var previousXlt4 = Cube4.transform.position.x;
		_lt4 = LeanTween.moveX( Cube4, 5.0f, 1.1f).setOnComplete( ()=>{
			LeanTest.expect( Cube4!=null && previousXlt4!=Cube4.transform.position.x, "RESUME OUT OF ORDER", "cube4:"+Cube4+" previousXlt4:"+previousXlt4+" cube4.transform.position.x:"+(Cube4!=null ? Cube4.transform.position.x : 0));
		});
		_lt4.resume();

		_rotateRepeat = _rotateRepeatAngle = 0;
		LeanTween.rotateAround(Cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(RotateRepeatFinished).setOnCompleteOnRepeat(true).setDestroyOnComplete(true);
		yield return new WaitForEndOfFrame();
		LeanTween.delayedCall(0.1f*8f+1f, RotateRepeatAllFinished);

		var countBeforeCancel = LeanTween.tweensRunning;
		LeanTween.cancel( _lt1Id );
		LeanTest.expect( countBeforeCancel==LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", "expected "+countBeforeCancel+" but got "+LeanTween.tweensRunning);
		LeanTween.cancel(Cube2);

		var tweenCount = 0;
		for(var i = 0; i < _groupTweens.Length; i++)
		{
			if(LeanTween.isTweening( _groupGOs[i] ))
				tweenCount++;
			switch (i%3)
			{
				case 0:
					LeanTween.pause( _groupGOs[i] );
					break;
				case 1:
					_groupTweens[i].pause();
					break;
				default:
					LeanTween.pause( _groupTweens[i].id );
					break;
			}
		}
		LeanTest.expect( tweenCount==_groupTweens.Length, "GROUP ISTWEENING", "expected "+_groupTweens.Length+" tweens but got "+tweenCount );

		yield return new WaitForEndOfFrame();

		tweenCount = 0;
		for(var i = 0; i < _groupTweens.Length; i++)
		{
			switch (i%3)
			{
				case 0:
					LeanTween.resume( _groupGOs[i] );
					break;
				case 1:
					_groupTweens[i].resume();
					break;
				default:
					LeanTween.resume( _groupTweens[i].id );
					break;
			}

			if(i%2==0 ? LeanTween.isTweening( _groupTweens[i].id ) : LeanTween.isTweening( _groupGOs[i] ) )
				tweenCount++;
		}
		LeanTest.expect( tweenCount==_groupTweens.Length, "GROUP RESUME" );

		LeanTest.expect( LeanTween.isTweening(Cube1)==false, "CANCEL TWEEN LTDESCR" );
		LeanTest.expect( LeanTween.isTweening(Cube2)==false, "CANCEL TWEEN LEANTWEEN" );

		LeanTest.expect( pauseCount==0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got "+pauseCount);

		yield return new WaitForEndOfFrame();
		Time.timeScale = 0.25f;
		const float tweenTime = 0.2f;
		var expectedTime = tweenTime * (1f/Time.timeScale);
		var start = Time.realtimeSinceStartup;
		var onUpdateWasCalled = false;
		LeanTween.moveX(Cube1, -5f, tweenTime).setOnUpdate( (float val)=>{
			onUpdateWasCalled = true;
		}).setOnComplete( ()=>{
			var end = Time.realtimeSinceStartup;
			var diff = end - start;
			
			LeanTest.expect( Mathf.Abs( expectedTime - diff) < 0.05f, "SCALED TIMING DIFFERENCE", "expected to complete in roughly "+expectedTime+" but completed in "+diff );
			LeanTest.expect( Mathf.Approximately(Cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at "+Cube1.transform.position.x);
			LeanTest.expect( onUpdateWasCalled, "ON UPDATE FIRED" );
		});

		yield return new WaitForSeconds( expectedTime );
		Time.timeScale = 1f;

		var ltCount = 0;
		var allGos = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGos) {
            if(go.name == "~LeanTween")
		     	ltCount++;
        }
		LeanTest.expect( ltCount==1, "RESET CORRECTLY CLEANS UP" );


		LotsOfCancels();
	}

	private IEnumerator LotsOfCancels(){
		yield return new WaitForEndOfFrame();

		Time.timeScale = 4f;
		const int cubeCount = 10;

		var tweensA = new int[ cubeCount ];
		var aGOs = new GameObject[ cubeCount ];
		for(var i = 0; i < aGOs.Length; i++){
			var cube = Instantiate( _boxNoCollider );
			cube.transform.position = new Vector3(0,0,i*2f);
			cube.name = "a"+i;
			aGOs[i] = cube;
			tweensA[i] = LeanTween.move(cube, cube.transform.position + new Vector3(10f,0,0), 0.5f + 1f * (1.0f/aGOs.Length) ).id;
			LeanTween.color(cube, Color.red, 0.01f);
		}

		yield return new WaitForSeconds(1.0f);

		var tweensB = new int[ cubeCount ];
		var bGOs = new GameObject[ cubeCount ];
		for(var i = 0; i < bGOs.Length; i++){
			var cube = Instantiate( _boxNoCollider );
			cube.transform.position = new Vector3(0,0,i*2f);
			cube.name = "b"+i;
			bGOs[i] = cube;
			tweensB[i] = LeanTween.move(cube, cube.transform.position + new Vector3(10f,0,0), 2f).id;
		}

		for(var i = 0; i < aGOs.Length; i++){
			LeanTween.cancel( aGOs[i] );
			var cube = aGOs[i];
			tweensA[i] = LeanTween.move(cube, new Vector3(0,0,i*2f), 2f).id;
		}

		yield return new WaitForSeconds(0.5f);

		for(var i = 0; i < aGOs.Length; i++){
			LeanTween.cancel( aGOs[i] );
			var cube = aGOs[i];
			tweensA[i] = LeanTween.move(cube, new Vector3(0,0,i*2f) + new Vector3(10f,0,0), 2f ).id;
		}

		for(var i = 0; i < bGOs.Length; i++){
			LeanTween.cancel( bGOs[i] );
			var cube = bGOs[i];
			tweensB[i] = LeanTween.move(cube, new Vector3(0,0,i*2f), 2f ).id;
		}

		yield return new WaitForSeconds(2.1f);

		var inFinalPlace = true;
		for(var i = 0; i < aGOs.Length; i++){
			if(Vector3.Distance( aGOs[i].transform.position, new Vector3(0,0,i*2f) + new Vector3(10f,0,0) ) > 0.1f)
				inFinalPlace = false;
		}

		for(var i = 0; i < bGOs.Length; i++){
			if(Vector3.Distance( bGOs[i].transform.position, new Vector3(0,0,i*2f) ) > 0.1f)
				inFinalPlace = false;
		}

		LeanTest.expect(inFinalPlace,"AFTER LOTS OF CANCELS");
	}

	private void RotateRepeatFinished(){
		if( Mathf.Abs(Cube3.transform.eulerAngles.z)<0.0001f )
			_rotateRepeatAngle++;
		_rotateRepeat++;
	}

	private void RotateRepeatAllFinished(){
		LeanTest.expect( _rotateRepeatAngle==3, "ROTATE AROUND MULTIPLE", "expected 3 times received "+_rotateRepeatAngle+" times" );
		LeanTest.expect( _rotateRepeat==3, "ROTATE REPEAT" );
		LeanTest.expect( Cube3==null, "DESTROY ON COMPLETE", "cube3:"+Cube3 );
	}

	private void EventGameObjectCalled( LTEvent e ){
		_eventGameObjectWasCalled = true;
	}

	private void EventGeneralCalled( LTEvent e ){
		_eventGeneralWasCalled = true;
	}

}
