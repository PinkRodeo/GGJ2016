using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


[System.Serializable]
public struct SceneGeographerBalcony
{
	public GameObject balconyBird;
	public Light balconySpotlight;
}

public class SceneChoreographer : MonoBehaviour
{
	public BeatGUIBar beatGUIBar;
	private StageCamera stageCamera;

	public SceneGeographerBalcony balconyObjects;


	// Use this for initialization
	void Start ()
	{
		stageCamera = GetComponent<StageCamera>();

		Vector3 targetBalconyBirdPos = balconyObjects.balconyBird.transform.localPosition;
		float targetBalconyLightIntensity = balconyObjects.balconySpotlight.intensity;
		float targetBalconyLightSpotAngle = balconyObjects.balconySpotlight.spotAngle;

		balconyObjects.balconyBird.gameObject.SetActive(false);


		balconyObjects.balconySpotlight.intensity = 0f;



		foreach (var sceneProp in GameObject.FindGameObjectsWithTag("TheaterProp"))
		{
			Vector3 curPosition = sceneProp.transform.localPosition;
			sceneProp.transform.position = sceneProp.transform.position + Vector3.up*40f;
			LeanTween.moveLocal(sceneProp, curPosition, 1f + Random.value*2f)
				.setDelay(Random.value*0.5f+ 1f)
				.setEase(LeanTweenType.easeOutQuad);


		}

		LeanTween.delayedCall(3f, () =>
		{


			balconyObjects.balconyBird.gameObject.SetActive(true);

			balconyObjects.balconyBird.transform.localPosition += Vector3.up*40f;


			LeanTween.moveLocal(balconyObjects.balconyBird, targetBalconyBirdPos, 3f)
				.setEase(LeanTweenType.easeOutCirc);

			LeanTween.value(balconyObjects.balconySpotlight.gameObject, 0, 1, .8f)
				.setOnUpdate((Action<float>) (f =>
				{
					balconyObjects.balconySpotlight.intensity = Mathf.Lerp(0, targetBalconyLightIntensity, f);
					balconyObjects.balconySpotlight.spotAngle = Mathf.Lerp(0, targetBalconyLightSpotAngle, f);

				}));

			LeanTween.value(gameObject, 0, 1, .8f)
				.setEase(LeanTweenType.easeOutBack)
				.setOnUpdate((Action<float>) (f =>
				{
					stageCamera.zoomedInOnVeranda = f;

				}))
				.setOnComplete(() =>
				{
					LeanTween.value(gameObject, 1, 0, 1.5f)
						.setOnUpdate((Action<float>) (f =>
						{
							stageCamera.zoomedInOnVeranda = f;

						}))
						.setEase(LeanTweenType.easeInElastic)

						.setDelay(2.9f)

						.setOnComplete(() =>
						{
							LeanTween.delayedCall(0f, () =>
							{
								beatGUIBar.StartTheMusic();
							});
						});
				});
		});



	}
}
