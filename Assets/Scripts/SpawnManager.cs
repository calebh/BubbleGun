using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject CountdownText;
    public GameObject GameOverCanvas;

    public GameObject[] SpawnPoints;

    private List<GameObject> AvailableSpawns = new List<GameObject>();
    private List<GameObject> Players = new List<GameObject>();

    public GameObject WinningDuck;

    public static SpawnManager Instance {
        get {
            return GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }
    }

    private bool GameOver = false;
    private float GameOverCountdown = 1.0f;

    public void Awake() {
        AvailableSpawns.AddRange(SpawnPoints);
    }

    public void OnEnable() {
        InputSystem.onAnyButtonPress.CallOnce(HandleButton);
    }

    private void HandleButton(InputControl ctrl) {
        if (!GameOver || GameOverCountdown >= 0.0f || (ctrl.device is Keyboard or Mouse)) {
            InputSystem.onAnyButtonPress.CallOnce(HandleButton);
        } else {
            SceneManager.LoadScene("CharacterSelection");
        }
    }

    public GameObject TakeRandomSpawn(GameObject player) {
        int idx = Random.Range(0, AvailableSpawns.Count);
        GameObject spawn = AvailableSpawns[idx];
        AvailableSpawns.RemoveAt(idx);
        Players.Add(player);
        return spawn;
    }

    public void Update() {
        if (Players.Count > 0) {
            int playersLeft = Players.Count;
            GameObject winningPlayer = null;
            foreach (GameObject player in Players) {
                if (player.transform.position.y < -5.0f) {
                    playersLeft--;
                } else {
                    winningPlayer = player;
                }
            }

            if (playersLeft == 1) {
                Material winningMaterial = winningPlayer.GetComponent<PlayerSelection>().CurrentMaterial;
                WinningDuck.GetComponent<MeshRenderer>().material = winningMaterial;
                WinningDuck.SetActive(true);
            }

            if (playersLeft <= 1) {
                GameOver = true;
                GameOverCanvas.SetActive(true);
                foreach (GameObject player in Players) {
                    Destroy(player);
                }
                Players.Clear();
            }
        }

        if (GameOver) {
            GameOverCountdown -= Time.deltaTime;
            if (GameOverCountdown <= 0.0f) {
                CountdownText.SetActive(true);
            }
        }
    }
}
