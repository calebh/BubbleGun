using JetBrains.Annotations;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private Transform CircleCenter;
    private ShrinkingCircle Circle;

    public Material RedMaterial;

    public float IsRedDuration = 5.0f;

    private bool IsRed = false;
    private float IsRedT = 0.0f;
    

    public bool IsInCircle {
        get {
            return Vector3.Distance(transform.position, CircleCenter.position) <= Circle.Radius;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject circle = GameObject.Find("Center");
        CircleCenter = circle.transform;
        Circle = circle.GetComponent<ShrinkingCircle>();

        if (!IsInCircle) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRed) {
            if (!IsInCircle) {
                IsRed = true;
                GetComponent<MeshRenderer>().material = RedMaterial;
            }
        }

        if (IsRed) {
            IsRedT += Time.deltaTime;

            if (IsRedT >= IsRedDuration) {
                Destroy(gameObject);
            }
        }
    }
}
