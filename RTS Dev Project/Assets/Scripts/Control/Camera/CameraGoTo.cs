using UnityEngine;
using System.Collections;

public class CameraGoTo : MonoBehaviour {

	[SerializeField] private Transform defaultTransform;
	[SerializeField] private float goToSpeed = 10f;

	void Start() {
		transform.position = defaultTransform.position;
		transform.rotation = defaultTransform.rotation;
		transform.localScale = defaultTransform.localScale;
	}

	//Smooth movement of the camera to the position given as argument
	IEnumerator SmoothMovement(Transform newTransform, float smoothTime)
	{
		
		float timeSinceStarted = 0f;
		Vector3 originalPosition = transform.position;
		Quaternion originalRotation = transform.rotation;
		
		while (true)
		{
			timeSinceStarted += Time.deltaTime / smoothTime;
			transform.position = Vector3.Lerp(originalPosition, newTransform.position, timeSinceStarted);
			transform.rotation = Quaternion.Lerp(originalRotation, newTransform.rotation, timeSinceStarted);
			
			/*// If the object has arrived to newPosition with some margin(deltaMovement), stop the coroutine
			if ((Mathf.Abs(transform.position.x - newPosition.x) <= deltaMovement) && (Mathf.Abs(transform.position.z - newPosition.z) <= deltaMovement))
			{
				
				yield break;
			}*/
				
				if (timeSinceStarted >= smoothTime)
					yield break;
			
			// Otherwise, continue next frame
			yield return null;
		}
	}
	
	public void goToSmooth(Transform goToTransform){
		StartCoroutine(SmoothMovement(goToTransform, goToSpeed));
	}
}
