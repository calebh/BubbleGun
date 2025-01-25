using UnityEngine;

[ExecuteInEditMode]
public class SelectionPointCanvas : MonoBehaviour
{
    public Camera Cam;
    public Transform SelectionPoint;

    private RectTransform CanvasRect;

    private RectTransform RectTrans;

    public void Awake() {
        RectTrans = GetComponent<RectTransform>();
        CanvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void Update() {
        Vector3 viewportPosition = Cam.WorldToViewportPoint(SelectionPoint.position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
            viewportPosition.x * CanvasRect.sizeDelta.x,
            -viewportPosition.y * CanvasRect.sizeDelta.y);

        RectTrans.anchoredPosition = WorldObject_ScreenPosition;
    }
}
