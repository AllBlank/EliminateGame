using UnityEngine;
using System.Collections;

public class BackgroundScaler : MonoBehaviour {

	public Camera targetCamera;
	public float minSize = 5;
	public float multiplier = 0.2f;
	
	void LateUpdate () {
		transform.localScale = Vector3.one * Mathf.Max (minSize, targetCamera.orthographicSize) * multiplier;
	}
}
