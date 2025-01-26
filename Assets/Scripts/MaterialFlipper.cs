using UnityEngine;

public class MaterialFlipper : MonoBehaviour
{
    public MeshRenderer Renderer;
    public Material[] Materials;
    private int CurrentMaterialIdx = 0;
    private Material CurrentMaterial {
        get {
            return Materials[CurrentMaterialIdx];
        }
    }

    public float SwapInterval = 2.0f;
    private float SwapAlarm = 2.0f;

    public void Start() {
        RefreshMaterial();
    }

    private void RefreshMaterial() {
        Renderer.material = CurrentMaterial;
    }

    public void Update() {
        SwapAlarm -= Time.deltaTime;

        if (SwapAlarm <= 0.0f) {
            SwapAlarm = SwapInterval;

            CurrentMaterialIdx = (CurrentMaterialIdx + 1) % Materials.Length;
            RefreshMaterial();
        }
    }
}
