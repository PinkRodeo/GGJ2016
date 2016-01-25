using UnityEngine;

public class PathSpline2D : MonoBehaviour {

	public Transform[] Trans;
	public Texture2D SpriteTexture;

	private LTSpline _cr;
	private GameObject _sprite1;
	private GameObject _sprite2;

	private void Start () {
		_cr = new LTSpline(Trans[0].position, Trans[1].position, Trans[2].position, Trans[3].position, Trans[4].position);
		_sprite1 = GameObject.Find("sprite1");
		_sprite2 = GameObject.Find("sprite2");
		#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
			_sprite1.AddComponent<SpriteRenderer>();
			_sprite1.GetComponent<SpriteRenderer>().sprite = Sprite.Create( SpriteTexture, new Rect(0.0f,0.0f,100.0f,100.0f), new Vector2(50.0f,50.0f), 10.0f);
			_sprite2.AddComponent<SpriteRenderer>();
			_sprite2.GetComponent<SpriteRenderer>().sprite = Sprite.Create( SpriteTexture, new Rect(0.0f,0.0f,100.0f,100.0f), new Vector2(0.0f,0.0f), 10.0f);
		#endif
		// LeanTween.moveSpline( ltLogo2, new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position}, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);

		var zoomInPathLt = LeanTween.moveSpline(_sprite2, new[]{Vector3.zero, Vector3.zero, new Vector3(1,1,1), new Vector3(2,1,1), new Vector3(2,1,1)}, 1.5f).setOrientToPath2d(true);
		zoomInPathLt.setUseEstimatedTime(true);
	}
	
	private float _iter;

	private void Update () {
		_cr.place2d( _sprite1.transform, _iter );

		_iter += Time.deltaTime*0.1f;
		if(_iter>1.0f)
			_iter = 0.0f;
	}

	private void OnDrawGizmos(){
		if(_cr!=null)
			_cr.gizmoDraw();
	}
}
