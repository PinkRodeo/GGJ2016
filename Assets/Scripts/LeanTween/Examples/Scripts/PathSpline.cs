using UnityEngine;

public class ExampleSpline : MonoBehaviour {

	public Transform[] Trans;

	private LTSpline _spline;
	private GameObject _ltLogo;
	private GameObject _ltLogo2;

	private void Start () {
		_spline = new LTSpline(Trans[0].position, Trans[1].position, Trans[2].position, Trans[3].position, Trans[4].position);
		_ltLogo = GameObject.Find("LeanTweenLogo1");
		_ltLogo2 = GameObject.Find("LeanTweenLogo2");

		LeanTween.moveSpline( _ltLogo2, _spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);

		var zoomInPathLt = LeanTween.moveSpline(_ltLogo2, new[]{Vector3.zero, Vector3.zero, new Vector3(1,1,1), new Vector3(2,1,1), new Vector3(2,1,1)}, 1.5f);
		zoomInPathLt.setUseEstimatedTime(true);
	}
	
	private float _iter;

	private void Update () {
		// Iterating over path
		_ltLogo.transform.position = _spline.point( _iter /*(Time.time*1000)%1000 * 1.0 / 1000.0 */);

		_iter += Time.deltaTime*0.1f;
		if(_iter>1.0f)
			_iter = 0.0f;
	}

	private void OnDrawGizmos(){
		if(_spline!=null) 
			_spline.gizmoDraw(); // debug aid to be able to see the path in the scene inspector
	}
}
