using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    [SerializeField] private Transform camera;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(Mathf.Abs(jumpHeight * 2 * gravity));
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move: " + moveInput);
    }

    public float rotSpeed = 15.0f;
    private void Update()
    {
        var horizVel = new Vector3(moveInput.x, 0.0f, moveInput.y) * speed;
        if (!Mathf.Approximately(horizVel.magnitude, 0.0f))
        {
            Quaternion rot = Quaternion.Euler(0, camera.eulerAngles.y, 0);
            horizVel = rot * horizVel;
            Quaternion direction = Quaternion.LookRotation(horizVel);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);

        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // Para mantenernos tocando el suelo
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            if (velocity.y < -20f) velocity.y = -20f; // Velocidad terminal
        }

        controller.Move((horizVel + velocity) * Time.deltaTime);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 70, 30), controller.isGrounded ? "GROUNDED" : "FALLING");
    }
}
