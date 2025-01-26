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
    private DuckRotator Rotator;

    private Material _CurrentMaterial;
    public Material CurrentMaterial {
        get {
            return _CurrentMaterial;
        }
        set {
            _CurrentMaterial = value;
            Renderer.material = _CurrentMaterial;
        }
    }

    private AudioSource AudioSource;
    public AudioClip UI1;
    public AudioClip UI2;

    public void Awake() {
        Controller = GetComponent<CharacterController>();
        AudioSource = GetComponent<AudioSource>();
        Rotator = GetComponent<DuckRotator>();
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

    public void OnDestroy() {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void StartedGame() {
        InGame = true;
        GetComponent<Rigidbody>().isKinematic = false;
        SpawnPoint = SpawnManager.Instance.TakeRandomSpawn(gameObject);
        Controller.enabled = false;
        transform.position = SpawnPoint.transform.position;
        Controller.enabled = true;
        Rotator.enabled = false;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1) {
        if (scene.name == "Game") {
            StartedGame();
        }
    }

    public void OnReady(InputAction.CallbackContext context) {
        AudioSource.PlayOneShot(UI1);

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
            AudioSource.PlayOneShot(UI2);
        }
    }

    public void OnPrev(InputAction.CallbackContext context) {
        if (!ReadyForActions) {
            return;
        }

        if (context.action.triggered && !InGame && !IsReady) {
            CurrentMaterial = SelectionManager.Instance.PrevMaterial(CurrentMaterial);
            AudioSource.PlayOneShot(UI2);
        }
    }

    public void OnBack(InputAction.CallbackContext context) {
        if (!ReadyForActions) {
            return;
        }

        if (context.action.triggered && !InGame) {
            if (IsReady) {
                IsReady = false;
                SelectionManager.Instance.CancelReadyUp(SelectionPoint);
            } else {
                if (SelectionPoint != null) {
                    SelectionManager.Instance.RemovePlayer(SelectionPoint, CurrentMaterial);
                    SelectionPoint = null;
                    Destroy(gameObject);
                }
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
