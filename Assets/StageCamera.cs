using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class StageCamera : MonoBehaviour
{
	private Camera camera;

	public Transform cameraFocalPoint;

	[Range(0f, 1f)]
	public float zoomedInOnVeranda = 0f;

	public Transform VerandaFocusPoint;
	public Transform MainStageFocustPoint;


	private float initialFOV;
	private Quaternion initialRotation;

	public float balconyTargetPOV = 60f;


	private Quaternion fullLookatQuaternion;

	// Use this for initialization
	void Start ()
	{
		camera = GetComponent<Camera>();
		initialFOV = camera.fieldOfView;

		initialRotation = transform.localRotation;

		fullLookatQuaternion =
			Quaternion.LookRotation((VerandaFocusPoint.position - transform.position).normalized, Vector3.up);

	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 newFocusPoint = Vector3.Lerp(MainStageFocustPoint.localPosition, VerandaFocusPoint.localPosition, zoomedInOnVeranda);

		cameraFocalPoint.localPosition = newFocusPoint;

		transform.localRotation = Quaternion.Lerp(initialRotation, fullLookatQuaternion, zoomedInOnVeranda);

		camera.fieldOfView = Mathf.Lerp(initialFOV, balconyTargetPOV, zoomedInOnVeranda);

	}
}
