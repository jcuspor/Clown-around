using UnityEngine;
using UnityEngine.InputSystem;

public class EnlacePersonaje : MonoBehaviour
{
    public Transform personaje1;
    public Transform personaje2;
    public Movimiento movimiento1;
    public Movimiento movimiento2;

    public CambioPersonaje cambioPersonaje;
    public InputAction enlaceAction;

    private bool enlazados = false;
    private Movimiento movActivo;
    private Movimiento movPasivo;

    public bool Enlazados => enlazados;

    void OnEnable()
    {
        enlaceAction.Enable();
    }

    void OnDisable()
    {
        enlaceAction.Disable();
    }

    void Start()
    {
        enlaceAction.performed += ctx => ToggleEnlace();
    }

    void ToggleEnlace()
    {
        enlazados = !enlazados;
        Debug.Log($"Enlace: {(enlazados ? "ACTIVADO" : "DESACTIVADO")}");

        if (enlazados)
        {
            ActualizarRoles();

            if (movPasivo != null)
                movPasivo.enabled = true;
        }
        else
        {
            if (movPasivo != null)
            {
                movPasivo.LimpiarInputForzado();
                movPasivo.enabled = false;
                movPasivo = null;
            }
        }
    }

    void Update()
    {
        if (!enlazados) return;
        AplicarInputEspejo();
    }

    void ActualizarRoles()
    {
        if (cambioPersonaje.Activo == 1)
        {
            movActivo = movimiento1;
            movPasivo = movimiento2;
        }
        else
        {
            movActivo = movimiento2;
            movPasivo = movimiento1;
        }

        if (movPasivo != null)
            movPasivo.LimpiarInputForzado();
    }

    void AplicarInputEspejo()
    {
        if (movActivo == null || movPasivo == null) return;

        // Si el ACTIVO está bloqueado por una pared, el pasivo también se para
        if (movActivo.EstaBloqueado())
        {
            movPasivo.SetInputForzado(Vector2.zero);
            return;
        }

        // Input del activo
        Vector2 inputActivo = movActivo.MoveInput;

        // Espejo lateral: X invertida, Z igual
        Vector2 inputPasivo = new Vector2(-inputActivo.x, inputActivo.y);

        // Comprobar si el pasivo puede moverse (no hay obstáculo delante)
        if (PuedeMoverse(movPasivo, inputPasivo))
        {
            movPasivo.SetInputForzado(inputPasivo);
        }
        else
        {
            movPasivo.SetInputForzado(Vector2.zero);
        }
    }

    bool PuedeMoverse(Movimiento mov, Vector2 input)
    {
        if (mov == null || !mov.enabled)
            return false;

        CharacterController cc = mov.GetComponent<CharacterController>();
        if (cc == null)
            return true;

        Vector3 dir = new Vector3(input.x, 0f, input.y);
        if (dir.sqrMagnitude < 0.0001f) return false;

        if (mov.CameraTransform != null)
        {
            dir = Quaternion.Euler(0, mov.CameraTransform.eulerAngles.y, 0) * dir;
        }

        float speed = mov.speed;
        Vector3 movimientoFrame = dir.normalized * speed * Time.deltaTime;

        RaycastHit hitInfo;
        bool hayObstaculo = Physics.CapsuleCast(
            cc.bounds.center + Vector3.up * (cc.height / 2f),
            cc.bounds.center + Vector3.down * (cc.height / 2f),
            cc.radius,
            movimientoFrame.normalized,
            out hitInfo,
            movimientoFrame.magnitude
        );

        return !hayObstaculo;
    }

    public void NotificarCambioActivo()
    {
        if (!enlazados) return;
        ActualizarRoles();
    }
}
