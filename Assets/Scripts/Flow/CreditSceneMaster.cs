using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditSceneMaster : MonoBehaviour
{
	private ControllerInput input;
	private int endState;
	public Image canvasImage;
	public Sprite sponsorSprite;
	// Use this for initialization
	void Start ()
	{
		input = new ControllerInput(1);
	}

	// Update is called once per frame
	void Update ()
	{
		if (input.AnyButtonPressed())
		{
			switch (endState)
			{
			case 0:
				//show GGJ sponsor
				canvasImage.sprite = sponsorSprite;
				break;
			default:
				SceneManager.LoadScene(0);
				break;
			}
			++endState;
		}
	}
}
