using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class Globals
{

	private static BirdData[] players;
	public static Material[] birdMaterials;
	public static PoseList[] poses;

	public static void Init(TextAsset poseAsset)
	{
		players = new BirdData[4];

		birdMaterials = new Material[6];
		for (int ii = 0; ii < 6; ii++)
		{
			birdMaterials[ii] = Resources.Load<Material>("Birds/Materials/BirdMat"+(ii+1));
		}
		LoadPoses (poseAsset);

		//Log.Bobn (poses.Length);
	}

	private static void LoadPoses(TextAsset posesAsset)
	{
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.

		xmlDoc.LoadXml(posesAsset.text); // load the file.
		XmlNodeList poseList = xmlDoc.GetElementsByTagName("pose"); // array of the level nodes.

		poses = new PoseList[poseList.Count];
		int idx = 0;

		foreach (XmlNode poseInfo in poseList)
		{
			PoseList list = new PoseList();
			list.count = int.Parse (poseInfo.Attributes ["phases"].Value);
			list.uiTexture = Resources.Load<Sprite>(poseInfo.Attributes ["texture"].Value);
			list.poses = new PoseData[list.count];

			foreach (XmlNode item in poseInfo)   // levels itens nodes.
			{
				if (item.Name == "leftWing")
				{
					for (int ii = 0; ii < list.count; ii++)
					{
						XmlNode dat = item.ChildNodes [ii];
						list.poses[ii].leftWing = float.Parse(dat.Attributes["value"].Value);
					}
				}
				if (item.Name == "rightWing")
				{
					for (int ii = 0; ii < list.count; ii++)
					{
						XmlNode dat = item.ChildNodes [ii];
						list.poses[ii].rightWing = float.Parse(dat.Attributes["value"].Value);
					}
				}
				if (item.Name == "head")
				{
					for (int ii = 0; ii < list.count; ii++)
					{
						XmlNode dat = item.ChildNodes [ii];
						Vector2 vec = new Vector2 ();
						vec.x = float.Parse(dat.Attributes["x"].Value);
						vec.y = float.Parse(dat.Attributes["y"].Value);
						list.poses[ii].head = vec;
					}
				}
				if (item.Name == "tail")
				{
					for (int ii = 0; ii < list.count; ii++)
					{
						XmlNode dat = item.ChildNodes [ii];
						Vector2 vec = new Vector2 ();
						vec.x = float.Parse(dat.Attributes["x"].Value);
						vec.y = float.Parse(dat.Attributes["y"].Value);
						list.poses[ii].tail = vec;
					}
				}
			}
			poses [idx++] = list;
		}
	}

	public static void ResetPlayerData()
	{
		for (int ii = 0; ii < 4; ii++)
		{
			players [ii].id = ii;
			players [ii].isUsed = false;
			players [ii].birdKind = 0;
			players [ii].score = 0;
		}
	}

	public static BirdData[] GetPlayerData()
	{
		BirdData[] result = new BirdData[4];

		for (int ii = 0; ii < 4; ii++)
		{
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

		for (int ii = 0; ii < 4; ii++)
		{
			if (players [ii].isUsed)
				count++;
		}

		BirdData[] result = new BirdData[count];

		int idx = 0;
		for (int ii = 0; ii < 4; ii++)
		{
			if (players [ii].isUsed)
			{
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
