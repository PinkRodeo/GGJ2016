using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Credits3D : MonoBehaviour
{
	public float textScrollSpeed = 0.2f;
	private bool finished = false;

	private TextMesh mesh;
	public Font font;

	private void Start()
	{
		mesh = gameObject.GetComponent<TextMesh>();
		mesh.anchor = TextAnchor.UpperCenter;
		mesh.alignment = TextAlignment.Center;
		mesh.characterSize = 0.03f;
		mesh.font = font;
		Init();
	}

	private void Init()
	{
		AddCreditsLine("Credits:");
		AddCreditsLine("");
		AddCreditsLine("");
		AddCreditsLine("Arnoud Poll Jonker");
		AddCreditsLine("Steff Kempink");
		AddCreditsLine("Gerben Pasjes");
		AddCreditsLine("Valentinas Rimeika");
		AddCreditsLine("Weikie Yeh");
		AddCreditsLine("Robin Zaagsma");


		StartCoroutine(TextScroll());
		Invoke("Finish", 12f);
	}

	IEnumerator TextScroll()
	{
		while (!finished)
		{
			gameObject.transform.Translate(0.0f, textScrollSpeed * Time.deltaTime, 0.0f);
			yield return null;
		}
		Finish();
	}

	private void Finish()
	{
		StopCoroutine("TextScroll");
		GetComponent<CreditSceneMaster>().Next();
	}

	private void AddCreditsLine(string str)
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
