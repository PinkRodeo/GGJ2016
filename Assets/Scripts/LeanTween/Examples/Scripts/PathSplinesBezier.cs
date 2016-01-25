using UnityEngine;

public class PathSplinesBezier : MonoBehaviour {

	public Transform[] Trans;

	private LTBezierPath _cr;
	private GameObject _avatar1;

	private void OnEnable(){
		// create the path
		_cr = new LTBezierPath( new[] {Trans[0].position, Trans[2].position, Trans[1].position, Trans[3].position, Trans[3].position, Trans[5].position, Trans[4].position, Trans[6].position} );
	}

	private void Start () {
		_avatar1 = GameObject.Find("Avatar1");

		// Tween automatically
		var descr = LeanTween.move(_avatar1, _cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
		Debug.Log("length of path 1:"+_cr.length);
		Debug.Log("length of path 2:"+descr.path.length);
	}
	
	private float _iter;

	private void Update () {
		// Or Update Manually
		//cr.place2d( sprite1.transform, iter );

		_iter += Time.deltaTime*0.07f;
		if(_iter>1.0f)
			_iter = 0.0f;
	}

	private void OnDrawGizmos(){
		// Debug.Log("drwaing");
		if(_cr!=null)
			OnEnable();
		Gizmos.color = Color.red;
		if(_cr!=null)
			_cr.gizmoDraw(); // To Visualize the path, use this method
	}
}
