using UnityEngine;

public class OldGUIExamplesCS : MonoBehaviour {
	public Texture2D Grumpy;
	public Texture2D Beauty;

	private float _w;
	private float _h;

	private LTRect _buttonRect1;
	private LTRect _buttonRect2;
	private LTRect _buttonRect3;
	private LTRect _buttonRect4;
	private LTRect _grumpyRect;
	private LTRect _beautyTileRect;


	// Use this for initialization
	private void Start () {
		_w = Screen.width;
		_h = Screen.height;
		_buttonRect1 = new LTRect(0.10f*_w, 0.8f*_h, 0.2f*_w, 0.14f*_h );
		_buttonRect2 = new LTRect(1.2f*_w, 0.8f*_h, 0.2f*_w, 0.14f*_h );
		_buttonRect3 = new LTRect(0.35f*_w, 0.0f*_h, 0.3f*_w, 0.2f*_h, 0f );
		_buttonRect4 = new LTRect(0.0f*_w, 0.4f*_h, 0.3f*_w, 0.2f*_h, 1.0f, 15.0f );
		
		_grumpyRect = new LTRect(0.5f*_w - Grumpy.width*0.5f, 0.5f*_h - Grumpy.height*0.5f, Grumpy.width, Grumpy.height );
		_beautyTileRect = new LTRect(0.0f,0.0f,1.0f,1.0f );

		LeanTween.move( _buttonRect2, new Vector2(0.55f*_w, _buttonRect2.rect.y), 0.7f ).setEase(LeanTweenType.easeOutQuad);
	}

	public void CatMoved(){
		Debug.Log("cat moved...");
	}
	
	// Update is called once per frame
	private void OnGUI () {
		GUI.DrawTexture( _grumpyRect.rect, Grumpy);

		var staticRect = new Rect(0.0f*_w, 0.0f*_h, 0.2f*_w, 0.14f*_h);
		if(GUI.Button( staticRect, "Move Cat")){
			if(LeanTween.isTweening(_grumpyRect)==false){ // Check to see if the cat is already tweening, so it doesn't freak out
				var orig = new Vector2( _grumpyRect.rect.x, _grumpyRect.rect.y );
				LeanTween.move( _grumpyRect, new Vector2( 1.0f*Screen.width - Grumpy.width, 0.0f*Screen.height ), 1.0f).setEase(LeanTweenType.easeOutBounce).setOnComplete(CatMoved);
				LeanTween.move( _grumpyRect, orig, 1.0f ).setDelay(1.0f).setEase( LeanTweenType.easeOutBounce);
			}
		}

		if(GUI.Button(_buttonRect1.rect, "Scale Centered")){
			LeanTween.scale( _buttonRect1, new Vector2(_buttonRect1.rect.width, _buttonRect1.rect.height) * 1.2f, 0.25f ).setEase( LeanTweenType.easeOutQuad );
			LeanTween.move( _buttonRect1, new Vector2(_buttonRect1.rect.x-_buttonRect1.rect.width*0.1f, _buttonRect1.rect.y-_buttonRect1.rect.height*0.1f), 0.25f ).setEase(LeanTweenType.easeOutQuad);
		}

		if(GUI.Button(_buttonRect2.rect, "Scale")){
			LeanTween.scale( _buttonRect2, new Vector2(_buttonRect2.rect.width, _buttonRect2.rect.height) * 1.2f, 0.25f ).setEase(LeanTweenType.easeOutBounce);
		}

		staticRect = new Rect(0.76f*_w, 0.53f*_h, 0.2f*_w, 0.14f*_h);
		if(GUI.Button( staticRect, "Flip Tile")){
			LeanTween.move( _beautyTileRect, new Vector2( 0f, _beautyTileRect.rect.y + 1.0f ), 1.0f ).setEase(LeanTweenType.easeOutBounce);
		}

		GUI.DrawTextureWithTexCoords( new Rect(0.8f*_w, 0.5f*_h - Beauty.height*0.5f, Beauty.width*0.5f, Beauty.height*0.5f), Beauty, _beautyTileRect.rect);


		if(GUI.Button(_buttonRect3.rect, "Alpha")){
			LeanTween.alpha( _buttonRect3, 0.0f, 1.0f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha( _buttonRect3, 1.0f, 1.0f).setDelay(1.0f).setEase( LeanTweenType.easeInQuad);

			LeanTween.alpha( _grumpyRect, 0.0f, 1.0f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha( _grumpyRect, 1.0f, 1.0f).setDelay(1.0f).setEase(LeanTweenType.easeInQuad);
		}
		GUI.color = new Color(1.0f,1.0f,1.0f,1.0f); // Reset to normal alpha, otherwise other gui elements will be effected

		if(GUI.Button(_buttonRect4.rect, "Rotate")){
			LeanTween.rotate( _buttonRect4, 150.0f, 1.0f ).setEase(LeanTweenType.easeOutElastic);
			LeanTween.rotate( _buttonRect4, 0.0f, 1.0f ).setDelay(1.0f).setEase(LeanTweenType.easeOutElastic);
		}
		GUI.matrix = Matrix4x4.identity;
	}
}
