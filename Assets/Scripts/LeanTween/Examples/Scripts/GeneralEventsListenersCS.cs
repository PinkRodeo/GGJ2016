#if !UNITY_FLASH
using UnityEngine;

public class GeneralEventsListenersCS : MonoBehaviour {
	private Vector3 _towardsRotation;
	private float _turnForLength = 0.5f;
	private float _turnForIter;
	private Color _fromColor;

	// It's best to make this a public enum that you use throughout your project, so every class can have access to it
	public enum MyEvents{ 
		ChangeColor,
		Jump,
		Length
	}

	private void Awake(){
		LeanTween.LISTENERS_MAX = 100; // This is the maximum of event listeners you will have added as listeners
		LeanTween.EVENTS_MAX = (int)MyEvents.Length; // The maximum amount of events you will be dispatching

		_fromColor = GetComponent<Renderer>().material.color;
	}

	private void Start () {
		// Adding Listeners, it's best to use an enum so your listeners are more descriptive but you could use an int like 0,1,2,...
		LeanTween.addListener(gameObject, (int)MyEvents.ChangeColor, ChangeColor);
		LeanTween.addListener(gameObject, (int)MyEvents.Jump, JumpUp);
	}

	// ****** Event Listening Methods

	private void JumpUp( LTEvent e ){
		GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	private void ChangeColor( LTEvent e ){
		var tran = (Transform)e.data;
		var distance = Vector3.Distance( tran.position, transform.position);
		var to = new Color(Random.Range(0f,1f),0f,Random.Range(0f,1f));
		LeanTween.value( gameObject, _fromColor, to, 0.8f ).setLoopPingPong(1).setDelay(distance*0.05f).setOnUpdate(
			(Color col)=>{
				GetComponent<Renderer>().material.color = col;
			}
		);
	}

	// ****** Physics / AI Stuff

	private void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer!=2)
			_towardsRotation = new Vector3(0f, Random.Range(-180, 180), 0f);
    }

	private void OnCollisionStay(Collision collision) {
		if (collision.gameObject.layer == 2) return;
		_turnForIter = 0f;
		_turnForLength = Random.Range(0.5f, 1.5f);
	}

	private void FixedUpdate(){
		if(_turnForIter < _turnForLength){
			GetComponent<Rigidbody>().MoveRotation( GetComponent<Rigidbody>().rotation * Quaternion.Euler(_towardsRotation * Time.deltaTime ) );
			_turnForIter += Time.deltaTime;
		}

		GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	// ****** Key and clicking detection

	private void OnMouseDown(){
		if(Input.GetKey( KeyCode.J )){ // Are you also pressing the "j" key while clicking
			LeanTween.dispatchEvent((int)MyEvents.Jump);
		}else{
			LeanTween.dispatchEvent((int)MyEvents.ChangeColor, transform); // with every dispatched event, you can include an object (retrieve this object with the *.data var in LTEvent)
		}
	}
}
#endif
