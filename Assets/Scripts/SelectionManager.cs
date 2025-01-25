using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public int NumPlayers = 0;
    public int NumReady = 0;

    public GameObject[] SelectionPoints;

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

    public void ReadyUp() {
        NumReady++;
    }

    public void StartGame() {
        if (NumReady >= NumPlayers && NumPlayers >= 2) {
            SceneManager.LoadScene("Game");
        }
    }

    
}
