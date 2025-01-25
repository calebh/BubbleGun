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

    public void Start() {
        foreach (GameObject point in SelectionPoints) {
            AvailableSelectionPoints.Add(point);
        }
    }

    public void RemovePlayer(GameObject selectionPoint) {
        NumPlayers--;
        AvailableSelectionPoints.Add(selectionPoint);
    }

    public GameObject AddPlayer() {
        if (AvailableSelectionPoints.Count > 0) {
            GameObject point = AvailableSelectionPoints[AvailableSelectionPoints.Count - 1];
            AvailableSelectionPoints.RemoveAt(AvailableSelectionPoints.Count - 1);
            NumPlayers++;
            return point;
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
