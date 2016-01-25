using UnityEngine;

public class TestingPunch : MonoBehaviour {

    public AnimationCurve ExportCurve;

	private void Start () {
	   //LeanTween.rotateAround(gameObject, gameObject.transform.rotation.eulerAngles, 360f, 5f).setDelay(1f).setEase(LeanTweenType.easeOutBounce);
        Debug.Log( "exported curve:" + CurveToString(ExportCurve) );
	}

	private void Update () 
    {        
        LeanTween.dtManual = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //LeanTween.scale(this.gameObject, Vector3.one*3f, 1.0f).setEase(LeanTweenType.easeSpring).setUseManualTime(true);            
            //print("scale punch time independent!");

            LeanTween.moveLocalX(gameObject, 5, 1).setOnComplete( () => {
            Debug.Log("on complete move local X");
            }).setOnCompleteOnStart(true);

            var dirLight = GameObject.Find("DirectionalLight");
            var lt = dirLight.GetComponent<Light>(); 
        
            LeanTween.value(lt.gameObject, lt.intensity, 0.0f, 1.5f)
              .setEase(LeanTweenType.linear)
              .setLoopPingPong()
              .setRepeat(-1)
              .setOnUpdate((float val)=>{
                
            lt.intensity = val;
            });
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LeanTween.scale(gameObject, Vector3.one*3f, 1.0f).setEase(LeanTweenType.punch);            
            print("scale punch!");
        }
 
        if (Input.GetKeyDown(KeyCode.R))
        {
            // LeanTween.rotate(this.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.punch);
            LeanTween.rotateAroundLocal(gameObject, transform.forward, -80f, 5.0f).setPoint(new Vector3(1.25f, 0f, 0f));
            print("rotate punch!");
        }
 
        if (Input.GetKeyDown(KeyCode.M))
        {
            // LeanTween.move(this.gameObject, new Vector3(0f,0f,1f), 1.0f).setEase(LeanTweenType.punch);
            print("move punch!");
            Time.timeScale = 0.25f;
            var start = Time.realtimeSinceStartup;
            LeanTween.moveX( gameObject, 1f, 1f).setOnComplete( DestroyOnComp ).setOnCompleteParam( gameObject ).setOnComplete( ()=>{
                var end = Time.realtimeSinceStartup;
                var diff = end - start;
                Debug.Log("start:"+start+" end:"+end+" diff:"+diff+" x:"+gameObject.transform.position.x);
            }).setEase(LeanTweenType.easeInOutElastic).setOvershoot(8f).setPeriod(0.3f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LeanTween.color( gameObject, new Color(1f, 0f, 0f, 0.5f), 1f);

            var to = new Color(Random.Range(0f,1f),0f,Random.Range(0f,1f),0.0f);
            var l = GameObject.Find("LCharacter");
            LeanTween.color( l, to, 4.0f ).setLoopPingPong(1).setEase(LeanTweenType.easeOutBounce);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            LeanTween.delayedCall(gameObject,0.3f, DelayedMethod).setRepeat(4).setOnCompleteOnRepeat(true).setOnCompleteParam( "hi" );
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            LeanTween.value( gameObject, UpdateColor, new Color(1.0f,0.0f,0.0f,1.0f), Color.blue, 4.0f );//.setRepeat(2).setLoopPingPong().setEase(LeanTweenType.easeOutBounce);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LeanTween.delayedCall(0.05f, EnterMiniGameStart).setOnCompleteParam( new object[]{""+5} );
        }

        if(Input.GetKeyDown(KeyCode.U)){
            #if !UNITY_FLASH
            LeanTween.value(gameObject, (Vector2 val)=>{
                // Debug.Log("tweening vec2 val:"+val);
                transform.position = new Vector3(val.x, transform.position.y, transform.position.z);
            }, new Vector2(0f,0f), new Vector2(5f,100f), 1f ).setEase(LeanTweenType.easeOutBounce);

            var l = GameObject.Find("LCharacter");
            Debug.Log("x:"+l.transform.position.x+" y:"+l.transform.position.y);
            LeanTween.value(l, new Vector2( l.transform.position.x, l.transform.position.y), new Vector2( l.transform.position.x, l.transform.position.y+5), 1f ).setOnUpdate( 
            (Vector2 val)=>{
                Debug.Log("tweening vec2 val:"+val);
                 l.transform.position = new Vector3(val.x, val.y, transform.position.z);
            }

            );
            #endif
        }
	}

	private static void EnterMiniGameStart( object val ){
        var arr = (object [])val;
        var lvl = int.Parse((string)arr[0]);
        Debug.Log("level:"+lvl);
    }

	private void UpdateColor( Color c ){
        var l = GameObject.Find("LCharacter");
        // Debug.Log("new col:"+c);
        l.GetComponent<Renderer>().material.color = c;
    }

	private void DelayedMethod( object myVal ){
        var castBack = myVal as string;
        Debug.Log("delayed call:"+Time.time +" myVal:"+castBack);
    }

	private void DestroyOnComp( object p ){
      var g = (GameObject)p;
      Destroy( g );
    }

	private string CurveToString( AnimationCurve curve) {
        var str = "";
        for(var i = 0; i < curve.length; i++){
            str += "new Keyframe("+curve[i].time+"f, "+curve[i].value+"f)";
            if(i<curve.length-1)
                str += ", ";
        }
        return "new AnimationCurve( "+str+" )";
    }
}
