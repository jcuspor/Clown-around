using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : MonoBehaviour
{
    [SerializeField] private Transform camera1; 
    public float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;
    public float rotSpeed = 15.0f; 
    public float gravity = -9.81f; // Valor habitual de gravedad
    private float verticalVelocity = 0f; // Nueva variable para la velocidad vertical

    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move: " + moveInput);
    }  

    private void Update() {
        Vector3 horizVel = new Vector3(moveInput.x, 0, moveInput.y) * speed;

        // --- GRAVEDAD ---
        if (controller.isGrounded && verticalVelocity < 0f) {
            verticalVelocity = -2f; // Es importante que no sea 0 para pegar el personaje al suelo
        } else {
            verticalVelocity += gravity * Time.deltaTime;
        }
        // --- FIN GRAVEDAD ---
        
        Vector3 move = horizVel;
        move.y = verticalVelocity; // Suma la velocidad vertical al movimiento total
        controller.Move(move * Time.deltaTime);

        if (!Mathf.Approximately(horizVel.magnitude, 0.0f)) {
            Quaternion rot = Quaternion.Euler(0, camera1.eulerAngles.y, 0);
            horizVel = rot * horizVel;
            Quaternion direction = Quaternion.LookRotation(horizVel);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }
    }

    private ControllerColliderHit _contact;
    (Vector3 from, Vector3 to) StepCheck() {
        return (transform.position + Vector3.up * controller.radius / 2,
        transform.position + Vector3.down * controller.radius / 2);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
}

