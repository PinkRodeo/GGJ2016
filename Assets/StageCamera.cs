using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class StageCamera : MonoBehaviour
{
	private Camera _camera;

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
		_camera = GetComponent<Camera>();
		initialFOV = _camera.fieldOfView;

		initialRotation = transform.localRotation;

		fullLookatQuaternion =
			Quaternion.LookRotation((VerandaFocusPoint.position - transform.position).normalized, Vector3.up);

	}

	public void setZoomedInOnVeranda(float newAmount)
	{
		zoomedInOnVeranda = Mathf.Clamp01(newAmount);

		Vector3 newFocusPoint = Vector3.LerpUnclamped(MainStageFocustPoint.localPosition, VerandaFocusPoint.localPosition, zoomedInOnVeranda);

		cameraFocalPoint.localPosition = newFocusPoint;

		transform.localRotation = Quaternion.LerpUnclamped(initialRotation, fullLookatQuaternion, zoomedInOnVeranda);

		_camera.fieldOfView = Mathf.LerpUnclamped(initialFOV, balconyTargetPOV, zoomedInOnVeranda);
	}
}
