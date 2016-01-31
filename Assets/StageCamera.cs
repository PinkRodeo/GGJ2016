using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class StageCamera : MonoBehaviour
{
	public Transform cameraFocalPoint;

	[Range(0f, 1f)]
	public float zoomedInOnVeranda = 0f;

	public Transform VerandaFocusPoint;
	public Transform MainStageFocustPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 newFocusPoint = Vector3.Lerp(MainStageFocustPoint.localPosition, VerandaFocusPoint.localPosition, zoomedInOnVeranda);

		cameraFocalPoint.localPosition = newFocusPoint;
	}
}
