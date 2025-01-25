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
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Vector2 Movement = Vector2.zero;

    private void Start() {
        Controller = GetComponent<CharacterController>();
        Selection = GetComponent<PlayerSelection>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Movement = context.ReadValue<Vector2>();
    }

    void Update() {
        if (!Selection.InGame) {
            return;
        }

        groundedPlayer = Controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Movement.x, 0.0f, Movement.y);
        Controller.Move(move * Time.deltaTime * PlayerSpeed);

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
        }

        // Makes the player jump
        /*
        if (Input.GetButtonDown("Jump") && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }
        */

        playerVelocity.y += gravityValue * Time.deltaTime;
        Controller.Move(playerVelocity * Time.deltaTime);
    }
}