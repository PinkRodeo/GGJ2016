using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;


public class Floater : MonoBehaviour
{
    public Font quicksand;
    List<Vector2> Locations = new List<Vector2>();
    public GameObject locationHolder;
	// Use this for initialization

	void Start () {
	    Locations.Add(locationHolder.transform.position);
	    for (int ii = 0; ii < locationHolder.transform.childCount; ii++)
	    {
	        Vector2 loc = new Vector2(locationHolder.transform.GetChild(ii).transform.position.x,
                                      locationHolder.transform.GetChild(ii).transform.position.y);
	        Locations.Add(loc);
	    }
	}

	// Update is called once per frame
	void Update ()
	{
        //if (Input.GetKeyDown(KeyCode.Space)) SpawnFloater("FIETSBEL!!!!", 0, Color.red);
	}

    public void SpawnFloater(string text, int pos, Color color)
    {
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform, false);
        
        newGO.AddComponent<Outline>();

        Text myText = newGO.AddComponent<Text>();
        myText.font = quicksand;
        myText.text = text;
        myText.color = color;
        newGO.AddComponent<FloatingText>();
        newGO.GetComponent<RectTransform>().position = Locations[pos];
        newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100,20);

	}

    public void SpawnFloater(string text, int pos, Color color, Color outlineColor)
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
        newGO.GetComponent<RectTransform>().position = Locations[pos];
    }
}
