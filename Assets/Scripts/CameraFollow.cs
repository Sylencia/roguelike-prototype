using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;

	public float followSpeed;
	public Vector3 offset;

	void LateUpdate()
	{
		Vector3 desiredPosition = followTarget.position + offset;
		Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
		transform.position = smoothedPos;
	}
}
