using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;


public class Floater : MonoBehaviour
{
    public List<Sprite> spriteList= new List<Sprite>(); 
    private float guitime = 6;

    public GameObject uiHolder;
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
        for (int ii = 0; ii < uiHolder.transform.childCount; ii++)
        {
            Vector2 loc = new Vector2(uiHolder.transform.GetChild(ii).transform.position.x,
                                      uiHolder.transform.GetChild(ii).transform.position.y);
            Locations.Add(loc);
        }
       // SpawnFloater(2, spriteList[0]);


    }

	// Update is called once per frame
	void Update ()
	{
        //if (Input.GetKeyDown(KeyCode.Space)) SpawnFloater("FIETSBEL!!!!", 0, Color.red);
        //
	    if (ScoreHandler.GetInstance().GetScore(1) > 10)
	    {
            guitime -= Time.deltaTime;
        }
	    
        
        if (guitime < 0)
	    {
	        guitime = 7;
            SpawnFloater(Random.Range(1, 4), spriteList[Random.Range(0, 13)]);
        }

	}

    public void SpawnFloater(int pos, Sprite img)
    {
        GameObject newGO = new GameObject("myTextGO");
        newGO.transform.SetParent(this.transform, false);

        newGO.AddComponent<Outline>();

        Image myText = newGO.AddComponent<Image>();
        myText.sprite = img;

        newGO.AddComponent<FloatingText>();
        if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
        newGO.GetComponent<RectTransform>().position = Locations[pos];
        newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);

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
        if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
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
        if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
        newGO.GetComponent<RectTransform>().position = Locations[pos];
    }
}
