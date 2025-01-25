using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerSelection : MonoBehaviour
{
    public MeshRenderer Renderer;

    private bool IsReady = false;

    private bool ReadyForActions = false;
    private float ReadyForActionsT = 0.0f;

    public bool InGame = false;

    private GameObject SelectionPoint;
    private GameObject SpawnPoint;

    private CharacterController Controller;

    public float SelectionRotationSpeed = 10.0f;

    private Material _CurrentMaterial;
    private Material CurrentMaterial {
        get {
            return _CurrentMaterial;
        }
        set {
            _CurrentMaterial = value;
            Renderer.material = _CurrentMaterial;
        }
    }

    public void Awake() {
        Controller = GetComponent<CharacterController>();
    }

    public void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        var playerInfo = SelectionManager.Instance.AddPlayer();
        SelectionPoint = playerInfo.Item1;
        CurrentMaterial = playerInfo.Item2;

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
        Controller.enabled = false;
        transform.position = SpawnPoint.transform.position;
        Controller.enabled = true;
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

        if (context.action.triggered && !InGame) {
            if (IsReady) {
                SelectionManager.Instance.StartGame();
            } else {
                IsReady = true;
                SelectionManager.Instance.ReadyUp(SelectionPoint);
            }
        }
    }

    public void OnNext(InputAction.CallbackContext context) {
        if (!ReadyForActions) {
            return;
        }

        if (context.action.triggered && !InGame && !IsReady) {
            CurrentMaterial = SelectionManager.Instance.NextMaterial(CurrentMaterial);
        }
    }

    public void OnPrev(InputAction.CallbackContext context) {
        if (!ReadyForActions) {
            return;
        }

        if (context.action.triggered && !InGame && !IsReady) {
            CurrentMaterial = SelectionManager.Instance.PrevMaterial(CurrentMaterial);
        }
    }

    public void Update() {
        if (!ReadyForActions) {
            ReadyForActionsT += Time.deltaTime;
            if (ReadyForActionsT > 0.25f) {
                ReadyForActions = true;
            }
        }

        if (!InGame) {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y += SelectionRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}
