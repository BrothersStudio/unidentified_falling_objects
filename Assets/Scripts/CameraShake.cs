using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	private Transform camTransform;
	
	// How long the object should shake for.
	private float shakeDuration = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.2f;
	private float decreaseFactor = 1.0f;
	
	Vector3 originalPos;
	
	void Awake()
	{
	    camTransform = GetComponent(typeof(Transform)) as Transform;
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
    
    public void ShakeCamera(float seconds)
    {
        shakeDuration = seconds;
    }

	void Update()
	{
        if (shakeDuration > 0)
        {
            gameObject.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
            shakeAmount -= Time.deltaTime * decreaseFactor;
            if (shakeAmount <= 0) shakeAmount = 0;
        }
        else
        {
            shakeDuration = 0f;
            gameObject.transform.localPosition = originalPos;
        }
    }
}