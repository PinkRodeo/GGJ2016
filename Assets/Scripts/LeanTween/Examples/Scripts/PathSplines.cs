using UnityEngine;

public class PathSplines : MonoBehaviour {

	public Transform[] Trans;

	private LTSpline _cr;
	private GameObject _avatar1;

	private void OnEnable(){
		// create the path
		_cr = new LTSpline(Trans[0].position, Trans[1].position, Trans[2].position, Trans[3].position, Trans[4].position);
		// cr = new LTSpline( new Vector3[] {new Vector3(-1f,0f,0f), new Vector3(0f,0f,0f), new Vector3(4f,0f,0f), new Vector3(20f,0f,0f), new Vector3(30f,0f,0f)} );
	}

	private void Start () {
		_avatar1 = GameObject.Find("Avatar1");

		// Tween automatically
		LeanTween.moveSpline(_avatar1, _cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1).setDirection(-1f);
	}
	
	private float _iter;

	private void Update () {
		// Or Update Manually
		// cr.place( avatar1.transform, iter );

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
