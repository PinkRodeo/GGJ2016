using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Credits3D : MonoBehaviour
{
	public float textScrollSpeed = 0.2f;
	public bool finished = false;

	private TextMesh mesh;
	public Material material;
	public Font font;

	private void Start()
	{
		mesh = gameObject.GetComponent<TextMesh>();
		Debug.Log(material);
		mesh.anchor = TextAnchor.UpperCenter;
		mesh.alignment = TextAlignment.Center;
		mesh.characterSize = 0.03f;
		mesh.font = font;
		Init();
	}

	private void Init()
	{
		addCreditsLine("Credits:");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Arnoud Poll Jonker");
		addCreditsLine("Steff Kempink");
		addCreditsLine("Gerben Pasjes");
		addCreditsLine("Valentinas Rimeika");
		addCreditsLine("Weikie Yeh");
		addCreditsLine("Robin Zaagsma");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("");


		StartCoroutine(TextScroll());
	}

	IEnumerator TextScroll()
	{
		while (!finished)
		{
			gameObject.transform.Translate(0.0f, textScrollSpeed * Time.deltaTime, 0.0f);
			yield return null;
		}
		StopCoroutine("TextScroll");
		GetComponent<CreditSceneMaster>().Next();
	}

	private void addCreditsLine(string str)
	{
		mesh.text += str;
		mesh.text += "\n";
	}

	public void Stop()
	{
		StopAllCoroutines();
		Destroy(mesh);
	}
}
