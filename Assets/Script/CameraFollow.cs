using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)] public float smoothFactor;
    public Vector3 minValues, MaxValue;

    void FixedUpdate()
    {
        Follow();
    }

    // Update is called once per frame
    void Follow()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, MaxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, MaxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, MaxValue.z)
            );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;

    }
}
