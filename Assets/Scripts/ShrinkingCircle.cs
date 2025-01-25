using UnityEngine;

public class ShrinkingCircle : MonoBehaviour
{
    public float StartRadius = 10.0f;
    public float EndRadius = 2.0f;
    public float TimeLimit = 120.0f;

    private float T = 0.0f;

    public float Radius {
        get {
            return MathUtils.MapRange(T, 0.0f, TimeLimit, StartRadius, EndRadius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;
        T = Mathf.Clamp(T, 0.0f, TimeLimit);
    }
}
