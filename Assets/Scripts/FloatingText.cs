using UnityEngine;
using System.Collections;
using System.Net.Mime;
using System.Text;
using UnityEditor;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private float Timer;
    private float journeyLength;
    public float speed = 120;
    public float startTime = 0;
    private Vector3 startpos;
    private Vector3 endpos;
    private bool scaleSet = false;
    private Vector3 startScale;
    private Vector3 endScale;
    private float scaleJourney;
    public bool isScore = false;
    // Use this for initialization
    void Start () {
        float floatup = Screen.height / 100;
        floatup *= 20;
        startpos = transform.position;
        endpos = transform.position + new Vector3(0, floatup, 0);
        journeyLength = Vector3.Distance(transform.position, endpos);
        speed = journeyLength;
        
    }
	
	// Update is called once per frame
	void Update () {
	    if (!isScore)
	    {
	        Timer += Time.deltaTime;
	        if (Timer < 0.8)
	        {
	            if (!scaleSet)
	            {
	                startTime = Time.time;
	                startScale = transform.localScale;
	                endScale = startScale*3.5f;
	                scaleJourney = Vector3.Distance(transform.localScale, endScale);
	                speed = scaleJourney/2;
	                scaleSet = true;
	            }

	            float distCovered = (Time.time - startTime)*speed;
	            float t = distCovered/scaleJourney;
	            t = Mathf.Sin(t*Mathf.PI*2.1f);
	            transform.localScale = Vector3.Lerp(startScale, endScale, t);

	        }
	        if (Timer > 0.8f)
	        {
	            if (startTime < 0.1) startTime = Time.time;
	            float distCovered = (Time.time - startTime)*120;
	            float t = distCovered/journeyLength;
	            t = t*t*t;
	            transform.position = Vector3.Lerp(startpos, endpos, t);
	            float normalCovered = distCovered/journeyLength;
	            Color newcolor = new Color();
	            if (transform.GetComponent<Text>())
	            {
	                newcolor = gameObject.GetComponent<Text>().color;
	                normalCovered = 1 - normalCovered;
	                newcolor.a = normalCovered;
	                gameObject.GetComponent<Text>().color = newcolor;
	            }
	            if (normalCovered < 0 || normalCovered > 1.5)
	            {
	                Destroy(this.gameObject);
	            }
	        }
        }
        else
	    {
            if (startTime < 0.1) startTime = Time.time;
            float distCovered = (Time.time - startTime) * 120;
            float t = distCovered / journeyLength;
            t = t * t * t;
            transform.position = Vector3.Lerp(startpos, endpos, t);
            float normalCovered = distCovered / journeyLength;
            Color newcolor = gameObject.GetComponent<Text>().color;
            normalCovered = 1 - normalCovered;
            newcolor.a = normalCovered;
            gameObject.GetComponent<Text>().color = newcolor;
            if (normalCovered < 0)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
