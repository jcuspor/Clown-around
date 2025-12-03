using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public Transform CameraTransform => cameraTransform;

    public float speed = 5f;
    public float rotSpeed = 15.0f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    private float verticalVelocity = 0f;
    private Vector3 velocity;
    private Animator animator;

    private bool usandoInputForzado = false;
    private Vector2 inputForzado;

    public bool IgnorarGravedad { get; set; } = false;

    public float GravityAmount { get; set; } = -9.81f;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
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

    public bool EstaBloqueado()
    {
        bool intentandoMoverse = moveInput.sqrMagnitude > 0.01f;
        bool chocandoPared = (controller.collisionFlags & CollisionFlags.Sides) != 0;
        return intentandoMoverse && chocandoPared;
    }
    /*
    private void Update()
    {
        Vector2 input = usandoInputForzado ? inputForzado : moveInput;

        Vector3 horizVel = new Vector3(input.x, 0.0f, input.y) * speed;
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
    */
    private void Update()
    {
        Vector2 input = usandoInputForzado ? inputForzado : moveInput;

        Vector3 horizVel = new Vector3(input.x, 0.0f, input.y) * speed;
        animator.SetFloat("Speed", horizVel.sqrMagnitude);

        if (!Mathf.Approximately(horizVel.magnitude, 0.0f))
        {
            Quaternion rot = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            horizVel = rot * horizVel;
            Quaternion direction = Quaternion.LookRotation(horizVel);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        // APLICA GRAVEDAD SOLO SI NO ESTÁ IGNORÁNDOSE
        if (!IgnorarGravedad)
        {
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -1f;
            }
            else
            {
                velocity.y += GravityAmount * Time.deltaTime;
                if (velocity.y < -20f) velocity.y = -20f;
            }
        }
        else
        {
            // Sin gravedad: mantiene altura Y constante
            velocity.y = 0f;
        }

        controller.Move((horizVel + velocity) * Time.deltaTime);
    }

    public bool EstaSobrePlataforma()
    {
        return false;
    }
}
