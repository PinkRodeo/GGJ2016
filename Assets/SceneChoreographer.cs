using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


[System.Serializable]
public struct SceneGeographerBalcony
{
	public GameObject balconyBird;
	public Light balconySpotlight;
}

public class SceneChoreographer : MonoBehaviour
{
	public Transform curtainTransform;
	public GameObject LogoGameObject;

	public BeatGUIBar beatGUIBar;
	private StageCamera stageCamera;

	public CheerScript crowd;

	public SceneGeographerBalcony balconyObjects;

	private ControllerInput debugControllerInput;

	Vector3 targetBalconyBirdPos;
	float targetBalconyLightIntensity;
	float targetBalconyLightSpotAngle;

	//private Vector3 curtainDownPos;
	private Vector3 initialLogoLocalPosition;
	private GameSceneMaster gameManager;
	private bool ended;
	private bool startPressed;

	// Use this for initialization
	void Start ()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameSceneMaster>();
		initialLogoLocalPosition = LogoGameObject.transform.localPosition;
		//curtainDownPos = curtainTransform.position;

		debugControllerInput = new ControllerInput(1);

		stageCamera = GetComponent<StageCamera>();

		targetBalconyBirdPos = balconyObjects.balconyBird.transform.localPosition;
		targetBalconyLightIntensity = balconyObjects.balconySpotlight.intensity;
		targetBalconyLightSpotAngle = balconyObjects.balconySpotlight.spotAngle;


		stageCamera.cameraFocalPoint.position= LogoGameObject.transform.position;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			ResetScene();
		}
		if ((debugControllerInput.GetKeyUp(ControllerAction.START) || Input.GetKeyDown(KeyCode.E)) && !startPressed)
		{
			StartGame();
		}

		if (Input.GetKeyDown(KeyCode.V) && !startPressed)
		{
			StartGameSkipAnimation();

			curtainTransform.Translate(0,100,0);
			LogoGameObject.transform.Translate(50f,0,0);
		}

		if (gameManager.end && !ended)
		{
			ended = true;
			crowd.StartEndCheer();
			Invoke("Outro", 8);
		}
	}

	private void StartGameSkipAnimation()
	{
		startPressed = true;

		gameManager.InitBirdControls();

		stageCamera.setZoomedInOnVeranda(0);

		beatGUIBar.StartTheMusic();
		crowd.canPlayCheers = true;
	}

	private void StartGame()
	{
		startPressed = true;

		DoThingsEnterStage();
		gameManager.InitBirdControls();
	}

	private static void ResetScene()
	{
		ScoreHandler.GetInstance().SetScore(1, 0);
		ScoreHandler.GetInstance().SetScore(2, 0);
		ScoreHandler.GetInstance().SetScore(3, 0);
		ScoreHandler.GetInstance().SetScore(4, 0);
		SceneManager.LoadScene(0);
	}

	private void Outro()
	{
		gameManager.ExitStage();
		DoThingsExitStage();
		Invoke("StopCheer", 5);
	}

	private void StopCheer()
	{
		crowd.StopEndCheer();
	}

	public void DoThingsEnterStage()
	{
		balconyObjects.balconyBird.gameObject.SetActive(false);
		balconyObjects.balconySpotlight.intensity = 0f;

		foreach (var sceneProp in GameObject.FindGameObjectsWithTag("TheaterProp"))
		{
			Vector3 curPosition = sceneProp.transform.localPosition;
			sceneProp.transform.position = sceneProp.transform.position + Vector3.up * 40f;
			LeanTween.moveLocal(sceneProp, curPosition, 1f + Random.value * 2f)
			.setDelay(Random.value * 0.5f + 1f)
			.setEase(LeanTweenType.easeOutQuad);
		}


		LeanTween.move(stageCamera.cameraFocalPoint.gameObject, stageCamera.MainStageFocustPoint.transform.position, 2.5f)
		.setEase(LeanTweenType.easeOutCirc)
		.setDelay(0.2f);

		LeanTween.moveLocalX(LogoGameObject, initialLogoLocalPosition.x + 20f, 1f)
		.setEase(LeanTweenType.easeInBack)
		.setOnComplete(() =>
		{
			LeanTween.moveY(curtainTransform.gameObject, 11.1f, 1f)
			.setEase(LeanTweenType.easeInBack)
			.setOnComplete(() =>
			{
				LeanTween.delayedCall(3f, () =>
				{
					balconyObjects.balconyBird.gameObject.SetActive(true);

					balconyObjects.balconyBird.transform.localPosition += Vector3.up * 40f;


					LeanTween.moveLocal(balconyObjects.balconyBird, targetBalconyBirdPos, 3f)
					.setEase(LeanTweenType.easeOutCirc);

					LeanTween.value(balconyObjects.balconySpotlight.gameObject, 0, 1, .8f)
					.setOnUpdate((Action<float>)(f =>
					{
						balconyObjects.balconySpotlight.intensity = Mathf.Lerp(0, targetBalconyLightIntensity, f);
						balconyObjects.balconySpotlight.spotAngle = Mathf.Lerp(0, targetBalconyLightSpotAngle, f);

					}));

					LeanTween.value(gameObject, 0, 1, .8f)
					.setEase(LeanTweenType.easeOutBack)
					.setOnUpdate((Action<float>)(f =>
					{
						stageCamera.setZoomedInOnVeranda(f);

					}))
					.setOnComplete(() =>
					{
						LeanTween.value(gameObject, 1, 0, 1.5f)
						.setOnUpdate((Action<float>)(f =>
						{
							stageCamera.setZoomedInOnVeranda(f);

						}))
						.setEase(LeanTweenType.easeInExpo)

						.setDelay(2.9f)

						.setOnComplete(() =>
						{
							LeanTween.delayedCall(0f, () =>
							{
								beatGUIBar.StartTheMusic();
								crowd.canPlayCheers = true;
							});
						});
					});
				});

			});
		});
	}

	public void DoThingsExitStage()
	{
		LeanTween.reset();

		float currentCameraZoom = stageCamera.zoomedInOnVeranda;

		LeanTween.value(gameObject, currentCameraZoom, 0f, currentCameraZoom)
		.setEase(LeanTweenType.easeOutBack)
		.setOnUpdate((Action<float>) (f =>
		{
			stageCamera.setZoomedInOnVeranda(f);

		})).setOnComplete(() =>
		{
			LeanTween.moveLocalY(curtainTransform.gameObject, -1.6f, 1f)
			.setEase(LeanTweenType.easeInBack)
			.setOnComplete(() =>
			{
				LeanTween.moveLocalX(LogoGameObject, initialLogoLocalPosition.x, 3f)
				.setEase(LeanTweenType.easeInBack)
				.setOnComplete(() =>
				{

				});
			});
		});
	}
}
