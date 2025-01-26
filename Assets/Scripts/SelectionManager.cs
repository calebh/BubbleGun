using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public int NumPlayers = 0;
    public int NumReady = 0;

    public GameObject[] SelectionPoints;
    public GameObject[] ReadyText;

    public GameObject ReadyToPlayText;

    public Material[] DuckMaterials;
    public bool[] DuckColorInUse;

    public bool ReadyToPlay {
        get {
            return NumReady >= NumPlayers && NumPlayers >= 2;
        }
    }

    public List<GameObject> AvailableSelectionPoints = new List<GameObject>();

    public static SelectionManager Instance {
        get {
            return GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        }
    }

    public void Awake() {
        DuckColorInUse = new bool[DuckMaterials.Length];
    }

    public void Start() {
        foreach (GameObject point in SelectionPoints) {
            AvailableSelectionPoints.Add(point);
        }
    }

    public void RemovePlayer(GameObject selectionPoint, Material duckMaterial) {
        NumPlayers--;
        AvailableSelectionPoints.Add(selectionPoint);

        for (int i = 0; i < DuckMaterials.Length; i++) {
            if (DuckMaterials[i] == duckMaterial) {
                DuckColorInUse[i] = false;
                break;
            }
        }
    }

    private Material AllocateDuckColor() {
        for (int i = 0; i < DuckMaterials.Length; i++) {
            if (!DuckColorInUse[i]) {
                DuckColorInUse[i] = true;
                return DuckMaterials[i];
            }
        }

        Debug.LogError("Unable to allocate duck color to player. Using default yellow.");
        return DuckMaterials[0];
    }

    private Material AdvanceDuckColor(Material currentMaterial, int delta) {
        int currentIdx = 0;
        for (int i = 0; i < DuckMaterials.Length; i++) {
            if (currentMaterial == DuckMaterials[i]) {
                currentIdx = i;
                break;
            }
        }

        int potentialIdx = currentIdx;
        while (true) {
            potentialIdx = (potentialIdx + delta + DuckMaterials.Length) % DuckMaterials.Length;
            if (potentialIdx == currentIdx) {
                return currentMaterial;
            } else {
                if (!DuckColorInUse[potentialIdx]) {
                    DuckColorInUse[currentIdx] = false;
                    DuckColorInUse[potentialIdx] = true;
                    return DuckMaterials[potentialIdx];
                }
            }
        }
    }

    public Material NextMaterial(Material currentMaterial) {
        return AdvanceDuckColor(currentMaterial, 1);
    }

    public Material PrevMaterial(Material currentMaterial) {
        return AdvanceDuckColor(currentMaterial, -1);
    }

    public Tuple<GameObject, Material> AddPlayer() {
        if (AvailableSelectionPoints.Count > 0) {
            GameObject point = AvailableSelectionPoints[0];
            AvailableSelectionPoints.RemoveAt(0);
            NumPlayers++;
            Material mat = AllocateDuckColor();
            return Tuple.Create(point, mat);
        } else {
            return null;
        }
    }

    private void SetReadyUp(GameObject selectionPoint, bool ready) {
        for (int i = 0; i < SelectionPoints.Count(); i++) {
            if (SelectionPoints[i] == selectionPoint) {
                ReadyText[i].SetActive(ready);
                break;
            }
        }
    }

    public void CancelReadyUp(GameObject selectionPoint) {
        NumReady--;
        SetReadyUp(selectionPoint, false);
    }

    public void ReadyUp(GameObject selectionPoint) {
        NumReady++;
        SetReadyUp(selectionPoint, true);
    }

    public void StartGame() {
        if (ReadyToPlay) {
            SceneManager.LoadScene("Game");
        }
    }

    public void Update() {
        ReadyToPlayText.SetActive(ReadyToPlay);
    }
}
