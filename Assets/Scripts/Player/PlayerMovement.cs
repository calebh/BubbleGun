using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerSelection))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController Controller;
    private PlayerSelection Selection;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float PlayerSpeed = 2.0f;
    public float JumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Vector2 Movement = Vector2.zero;
    private Vector2 Aiming = new Vector2(1.0f, 0.0f);

    public AudioSource Audio;
    public AudioClip JumpAudio;

    public bool CanJump = true;
    private bool Jump = false;

    private bool _IsTrappedInBubble = false;
    public bool IsTrappedInBubble {
        get {
            return _IsTrappedInBubble;
        }
        set {
            _IsTrappedInBubble = value;
            Controller.enabled = !value;
        }
    }

    private void Start() {
        Controller = GetComponent<CharacterController>();
        Selection = GetComponent<PlayerSelection>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context) {
        Aiming = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) {
        Jump = context.action.triggered;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit) {
        HexTile tile = hit.gameObject.GetComponent<HexTile>();
        if (tile != null) {
            tile.ContactedPlayer();
        }
    }

    void Update() {
        if (!Selection.InGame || IsTrappedInBubble) {
            return;
        }

        groundedPlayer = Controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        if (groundedPlayer) {
            CanJump = true;
        }

        if (Movement.magnitude > 0.1f) {
            Vector3 move = new Vector3(Movement.x, 0.0f, Movement.y);
            Controller.Move(move * Time.deltaTime * PlayerSpeed);
        }

        if (Jump && CanJump) {
            CanJump = false;
            playerVelocity.y += Mathf.Sqrt(JumpHeight * -2.0f * gravityValue);
            Audio.PlayOneShot(JumpAudio);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        Controller.Move(playerVelocity * Time.deltaTime);

        if (Aiming.magnitude > 0.1f) {
            transform.rotation = Quaternion.LookRotation(new Vector3(Aiming.x, 0.0f, Aiming.y));
        }
    }
}