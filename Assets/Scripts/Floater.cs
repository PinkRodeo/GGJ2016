using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

public class Floater : MonoBehaviour
{
	// Use this for initialization
	void Start () {
	    //TextInMiddle("FIETSBEL!!!!", new Vector2(100,100),Color.red);
        Frequency.getInstance().startTimer();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Log.Arny(Frequency.getInstance().freq);
	}

    public void TextInMiddle(string text, Vector2 pos, Color color)
    {
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform, false);
        
        newGO.AddComponent<Outline>();



        Text myText = newGO.AddComponent<Text>();
        myText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        myText.text = text;
        myText.color = color;
        newGO.AddComponent<FloatingText>();
        newGO.GetComponent<RectTransform>().position = pos;
        newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100,20);
    }


    public void TextInMiddle(string text, Vector2 pos, Color color, Color outlineColor)
    {

        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform, false);
        newGO.AddComponent<Outline>();

        Text myText = newGO.AddComponent<Text>();
        myText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        GetComponent<Outline>().effectColor = outlineColor;
        myText.text = text;
        myText.color = color;
        newGO.AddComponent<FloatingText>();
        newGO.GetComponent<RectTransform>().position = pos;
    }
}
