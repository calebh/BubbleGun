using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool Loading = false;

    public void OnEnable() {
        InputSystem.onAnyButtonPress.CallOnce(HandleButton);
    }

    private void HandleButton(InputControl ctrl) {
        if (ctrl.device is Keyboard or Mouse) {
            InputSystem.onAnyButtonPress.CallOnce(HandleButton);
        } else {
            if (!Loading) {
                Loading = true;
                SceneManager.LoadScene("CharacterSelection");
            }
        }
    }
}
