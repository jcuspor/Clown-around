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
    private Movimiento movPasivo;
    private Movimiento movActivo;
    public bool Enlazados => enlazados;  // Para que CambioPersonaje sepa si están enlazados


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
            
            // ← AQUÍ: ACTIVAR inmediatamente el script del pasivo
            if (movPasivo != null)
            {
                movPasivo.enabled = true;
            }
        }
        else
        {
            if (movPasivo != null) 
            {
                movPasivo.LimpiarInputForzado();
                movPasivo.enabled = false;  // ← DESACTIVAR al desenlazar
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
        Movimiento movActivo;
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

        // Limpiar input anterior del pasivo
        movPasivo.LimpiarInputForzado();
    }
/*
    void AplicarInputEspejo()
    {
        if (cambioPersonaje == null) return;
        
        // Saber quién está activo por el índice en cambioPersonaje
        bool esActivoPrimero = (cambioPersonaje.Activo == 1);

        Vector2 inputActivo = esActivoPrimero ? movimiento1.MoveInput : movimiento2.MoveInput;

        // Espejamos la X
        Vector2 inputPasivo = new Vector2(-inputActivo.x, inputActivo.y);

        // Aplicamos input forzado solo al pasivo
        Movimiento movPasivo = esActivoPrimero ? movimiento2 : movimiento1;
        movPasivo.SetInputForzado(inputPasivo);
    }
*/
/*
    void AplicarInputEspejo()
    {
        Vector2 inputActivo = movActivo.MoveInput;
        Vector2 inputPasivo = new Vector2(-inputActivo.x, inputActivo.y); // Espejo

        if (PuedeMoverse(movPasivo, inputPasivo))
        {
            movPasivo.SetInputForzado(inputPasivo);
        }
        else
        {
            // Impide que el pasivo se mueva, pasa input cero
            movPasivo.SetInputForzado(Vector2.zero);
        }
    }
*/
    void AplicarInputEspejo()
    {
        if (movActivo.EstaBloqueado())
        {
            movPasivo.SetInputForzado(Vector2.zero); // no puede avanzar
            return;
        }

        Vector2 inputActivo = movActivo.MoveInput;
        Vector2 inputPasivo = new Vector2(-inputActivo.x, inputActivo.y); // Espejo
        movPasivo.SetInputForzado(inputPasivo);
    }


    bool PuedeMoverse(Movimiento mov, Vector2 input)
    {
        if (mov == null || !mov.enabled)
            return false;

        CharacterController cc = mov.GetComponent<CharacterController>();
        if (cc == null)
            return true;

        Vector3 direccionMovimiento = new Vector3(input.x, 0, input.y);
        direccionMovimiento = mov.CameraTransform != null ? 
    Quaternion.Euler(0, mov.CameraTransform.eulerAngles.y, 0) * direccionMovimiento : direccionMovimiento;

        float speed = mov.speed;
        Vector3 movimientoFrame = direccionMovimiento.normalized * speed * Time.deltaTime;

        // Consulta si este movimiento golpearía algo
        // CapsuleCast con el tamaño y posición del CharacterController:
        RaycastHit hitInfo;
        bool hayObstaculo = Physics.CapsuleCast(
            cc.bounds.center + Vector3.up * (cc.height / 2),      // Punto superior
            cc.bounds.center + Vector3.down * (cc.height / 2),    // Punto inferior
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
