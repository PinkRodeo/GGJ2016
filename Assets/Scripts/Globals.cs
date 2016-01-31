using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class Globals {

	private static BirdData[] players;
	public static Material[] birdMaterials; 
	public static PoseData[] poses;

	public static void Init(TextAsset poseAsset)
	{
		players = new BirdData[4];

		birdMaterials = new Material[6];
		for (int ii = 0; ii < 6; ii++) {
			birdMaterials[ii] = Resources.Load<Material>("Birds/Materials/BirdMat"+(ii+1));
		}
		LoadPoses (poseAsset);

		Log.Bobn (poses.Length);
	}

	private static void LoadPoses(TextAsset posesAsset) {
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.

		xmlDoc.LoadXml(posesAsset.text); // load the file.
		XmlNodeList poseList = xmlDoc.GetElementsByTagName("pose"); // array of the level nodes.

		poses = new PoseData[poseList.Count];
		int idx = 0;

		foreach (XmlNode poseInfo in poseList)
		{
			PoseData pose = new PoseData();
			pose.phases = int.Parse (poseInfo.Attributes ["phases"].Value);
			pose.uiTexture = Resources.Load<Texture2D>(poseInfo.Attributes ["texture"].Value);

			foreach (XmlNode item in poseInfo) { // levels itens nodes.
				if (item.Name == "leftWing") {
					pose.leftWing = new float[pose.phases];
					for (int ii = 0; ii < pose.phases; ii++) {
						XmlNode dat = item.ChildNodes [ii];
						pose.leftWing[ii] = int.Parse(dat.Attributes["value"].Value);
					}
				}
				if (item.Name == "rightWing") {
					pose.rightWing = new float[pose.phases];
					for (int ii = 0; ii < pose.phases; ii++) {
						XmlNode dat = item.ChildNodes [ii];
						pose.rightWing[ii] = int.Parse(dat.Attributes["value"].Value);
					}
				}
				if (item.Name == "head") {
					pose.head = new Vector2[pose.phases];
					for (int ii = 0; ii < pose.phases; ii++) {
						XmlNode dat = item.ChildNodes [ii];
						Vector2 vec = new Vector2 ();
						vec.x = float.Parse(dat.Attributes["x"].Value);
						vec.y = float.Parse(dat.Attributes["y"].Value);
						pose.head [ii] = vec;
					}
				}
				if (item.Name == "tail") {
					pose.tail = new Vector2[pose.phases];
					for (int ii = 0; ii < pose.phases; ii++) {
						XmlNode dat = item.ChildNodes [ii];
						Vector2 vec = new Vector2 ();
						vec.x = float.Parse(dat.Attributes["x"].Value);
						vec.y = float.Parse(dat.Attributes["y"].Value);
						pose.tail [ii] = vec;
					}
				}
			}
			poses [idx++] = pose;
		}
	}

	public static void ResetPlayerData()
	{
		for (int ii = 0; ii < 4; ii++) {
			players [ii].id = ii;
			players [ii].isUsed = false;
			players [ii].birdKind = 0;
			players [ii].score = 0;
		}
	}

	public static BirdData[] GetPlayerData()
	{
		BirdData[] result = new BirdData[4];

		for (int ii = 0; ii < 4; ii++) {
			result [ii].id = players [ii].id;
			result [ii].isUsed = result [ii].isUsed;
			result [ii].birdKind = result [ii].birdKind ;
			result [ii].score = result [ii].score;
		}

		return result;
	}

	public static BirdData GetPlayerData(int id)
	{
		BirdData result;

		result.id = players [id].id;
		result.isUsed = players [id].isUsed;
		result.birdKind = players [id].birdKind ;
		result.score = players [id].score;

		return result;
	}

	public static BirdData[] GetActivePlayerData()
	{
		int count = 0;

		for (int ii = 0; ii < 4; ii++) {
			if (players [ii].isUsed)
				count++;
		}

		BirdData[] result = new BirdData[count];

		int idx = 0;
		for (int ii = 0; ii < 4; ii++) {
			if (players [ii].isUsed) {
				result [idx].id = players [ii].id;
				result [idx].isUsed = result [ii].isUsed;
				result [idx].birdKind = result [ii].birdKind;
				result [idx].score = result [ii].score;

				idx++;
			}
		}

		return result;
	}

	public static void SetPlayerData(int id, BirdData data) 
	{
		players [id].id = data.id;
		players [id].isUsed = data.isUsed;
		players [id].birdKind = data.birdKind;
		players [id].score = data.score;
	}
}
