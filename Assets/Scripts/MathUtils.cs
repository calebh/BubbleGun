using UnityEngine;

public static class MathUtils
{
    public static float MapRange(float s, float a1, float a2, float b1, float b2) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

    public static Vector2 DampVector3(Vector3 a, Vector3 b, float lambda, float dt) {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static float Damp(float a, float b, float lambda, float dt) {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
