using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; 
    public float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    public float rotSpeed = 15.0f; 
    public float gravity = -9.81f;
    private float verticalVelocity = 0f;
    private Vector3 velocity;
    private Animator animator;

    private bool usandoInputForzado = false;
    private Vector2 inputForzado;

    private ControllerColliderHit _contact;

    public Transform CameraTransform => cameraTransform;


    private void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        if (usandoInputForzado) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void SetInputForzado(Vector2 input)
    {
        usandoInputForzado = true;
        inputForzado = input;
    }

    public void LimpiarInputForzado()
    {
        usandoInputForzado = false;
    }

    private void Update() {
        Vector2 input = usandoInputForzado ? inputForzado : moveInput;

        var horizVel = new Vector3(input.x, 0.0f, input.y) * speed;
        animator.SetFloat("Speed", horizVel.sqrMagnitude);
        
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

    private bool estaSobrePlataforma = false;
    public bool EstaSobrePlataforma()
    {
        return estaSobrePlataforma;
    }

    public bool EstaBloqueado()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0;
        bool intentandoMoverse = moveInput.sqrMagnitude > 0.01f;
        bool sinMovimiento = horizontalVelocity.magnitude < 0.1f;
        return intentandoMoverse && sinMovimiento;
    }

}
