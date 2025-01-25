using JetBrains.Annotations;
using UnityEngine;

public enum PopState
{
    NORMAL, POPPING, POPPED
}

public class HexTile : MonoBehaviour
{
    private Transform CircleCenter;
    private ShrinkingCircle Circle;

    private Material DefaultMaterial;

    public Material RedMaterial;
    public Material YellowMaterial;

    public float IsRedDuration = 5.0f;

    private bool IsRed = false;
    private float IsRedT = 0.0f;

    private PopState PopState = PopState.NORMAL;
    private float PoppingT = 0.0f;
    private float RegenT = 0.0f;

    public float PoppingDuration = 1.0f;
    public float RegenDuration = 3.0f;

    private MeshRenderer Renderer;
    private MeshCollider Collider;

    public bool IsInCircle {
        get {
            return Vector3.Distance(transform.position, CircleCenter.position) <= Circle.Radius;
        }
    }

    public void Awake() {
        Renderer = GetComponent<MeshRenderer>();
        Collider = GetComponent<MeshCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GameObject circle = GameObject.Find("Center");
        CircleCenter = circle.transform;
        Circle = circle.GetComponent<ShrinkingCircle>();

        if (!IsInCircle) {
            Destroy(gameObject);
        }

        DefaultMaterial = Renderer.material;
    }

    private void UpdateCircle() {
        if (!IsRed) {
            if (!IsInCircle) {
                IsRed = true;
                Renderer.material = RedMaterial;
            }
        }

        if (IsRed) {
            IsRedT += Time.deltaTime;

            if (IsRedT >= IsRedDuration) {
                Destroy(gameObject);
            }
        }
    }

    private void UpdatePopping() {
        if (PopState == PopState.POPPING) {
            PoppingT += Time.deltaTime;
            if (PoppingT >= PoppingDuration) {
                Renderer.enabled = false;
                Collider.enabled = false;
                PopState = PopState.POPPED;
                PoppingT = 0.0f;
            }
        } else if (PopState == PopState.POPPED) {
            RegenT += Time.deltaTime;
            if (RegenT >= RegenDuration) {
                Renderer.enabled = true;
                Collider.enabled = true;
                if (IsRed) {
                    Renderer.material = RedMaterial;
                } else {
                    Renderer.material = DefaultMaterial;
                }
                PopState = PopState.NORMAL;
                RegenT = 0.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCircle();
        UpdatePopping();
    }

    public void ContactedPlayer() {
        if (PopState == PopState.NORMAL) {
            PopState = PopState.POPPING;
            if (!IsRed) {
                Renderer.material = YellowMaterial;
            }
        }
    }
}
