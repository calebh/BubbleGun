using UnityEngine;

public class DuckRotator : MonoBehaviour
{
    public float RotationSpeed = 120.0f;

    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
