using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public static int NumPlayers = 0;
    public static int NumReady = 0;

    private bool IsReady = false;

    private bool ReadyForActions = false;
    private float ReadyForActionsT = 0.0f;

    public bool InGame = false;

    private GameObject SelectionPoint;
    private GameObject SpawnPoint;

    public void Awake() {
        NumPlayers++;
    }

    public void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SelectionPoint = SelectionManager.Instance.AddPlayer();
        if (SelectionPoint == null) {
            Destroy(gameObject);
        } else {
            transform.position = SelectionPoint.transform.position;
        }
    }

    private void StartedGame() {
        InGame = true;
        GetComponent<Rigidbody>().isKinematic = false;
        SpawnPoint = SpawnManager.Instance.TakeRandomSpawn();
        transform.position = SpawnPoint.transform.position;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1) {
        if (scene.name == "Game") {
            StartedGame();
        } else {
            Destroy(gameObject);
        }
    }

    public void OnReady(InputAction.CallbackContext context) {
        if (!ReadyForActions) {
            return;
        }

        if (context.action.triggered) {
            if (IsReady) {
                SelectionManager.Instance.StartGame();
            } else {
                IsReady = true;
                SelectionManager.Instance.ReadyUp();
            }
        }
    }

    public void Update() {
        if (!ReadyForActions) {
            ReadyForActionsT += Time.deltaTime;
            if (ReadyForActionsT > 0.25f) {
                ReadyForActions = true;
            }
        }
    }
}
