using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; 
    public float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;
    public float rotSpeed = 15.0f; 
    public float gravity = -9.81f;
    private float verticalVelocity = 0f;
    private Vector3 velocity;

    private ControllerColliderHit _contact;

    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move: " + moveInput);
    }

    private void Update() {
        var horizVel = new Vector3(moveInput.x, 0.0f, moveInput.y) * speed;
        if (!Mathf.Approximately(horizVel.magnitude, 0.0f))
        {
            Quaternion rot = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            horizVel = rot * horizVel;
            Quaternion direction = Quaternion.LookRotation(horizVel);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            if (velocity.y < -20f) velocity.y = -20f;
        }

        controller.Move((horizVel + velocity) * Time.deltaTime);
    }

    (Vector3 from, Vector3 to) StepCheck() {
        return (transform.position + Vector3.up * controller.radius / 2,
        transform.position + Vector3.down * controller.radius / 2);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
} // <- Esta llave cierra la clase correctamente

