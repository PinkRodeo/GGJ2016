using UnityEngine;
using System.Collections;

public class Globals {

	private static BirdData[] players;
	public static Material[] birdMaterials; 

	public static void Init()
	{
		players = new BirdData[4];

		birdMaterials = new Material[6];
		for (int ii = 0; ii < 6; ii++) {
			birdMaterials[ii] = Resources.Load<Material>("Birds/Materials/BirdMat"+(ii+1));
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
