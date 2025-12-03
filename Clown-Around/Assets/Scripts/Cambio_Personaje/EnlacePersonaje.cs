using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EnlacePersonaje : MonoBehaviour
{
    public Transform personaje1;
    public Transform personaje2;

    public Movimiento movimiento1;
    public Movimiento movimiento2;

    public CambioPersonaje cambioPersonaje;

    public InputAction enlaceAction; // Q

    private bool enlazados = false;
    private Movimiento movActivo;
    private Movimiento movPasivo;

    private Coroutine caidaCorutina;

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
        enlaceAction.started += ctx => ToggleEnlace(); 
    }

    void Update()
    {
        if (!enlazados) return;
        AplicarInputEspejo();
    }

    // ======================================================
    // 🔥 TOGGLE ENLACE (Q)
    // ======================================================
    private void ToggleEnlace()
    {
        enlazados = !enlazados;

        if (enlazados)
        {
            Debug.Log("🟢 Enlace ACTIVADO");

            ActualizarRoles();

            if (movPasivo != null)
            {
                movPasivo.enabled = true;
                movPasivo.IgnorarGravedad = true;
            }
        }
        else
        {
            Debug.Log("🔴 Enlace DESACTIVADO");

            if (movPasivo != null)
            {
                movPasivo.IgnorarGravedad = false;
                movPasivo.LimpiarInputForzado();

                if (caidaCorutina != null) StopCoroutine(caidaCorutina);
                caidaCorutina = StartCoroutine(DesactivarTrasCaida(movPasivo, 1f));
            }

            movPasivo = null;
            movActivo = null;
        }
    }

    // ======================================================
    // 🔥 FORZAR DESENLACE TOTAL (R)
    // ======================================================
    public void ForzarDesenlaceTotal()
    {
        if (!enlazados) return;

        Debug.Log("🔴 Enlace DESACTIVADO por cambio de personaje (R)");

        enlazados = false;

        if (movPasivo != null)
        {
            movPasivo.IgnorarGravedad = false;
            movPasivo.LimpiarInputForzado();
            movPasivo.enabled = false;
        }

        movPasivo = null;
        movActivo = null;
    }

    // ======================================================
    // LOGICA PRINCIPAL
    // ======================================================
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
        {
            movPasivo.LimpiarInputForzado();
            movPasivo.IgnorarGravedad = true;
        }
    }

    IEnumerator DesactivarTrasCaida(Movimiento mov, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (mov != null)
            mov.enabled = false;
    }

    void AplicarInputEspejo()
    {
        if (movActivo == null || movPasivo == null) return;

        if (movActivo.EstaBloqueado())
        {
            movPasivo.SetInputForzado(Vector2.zero);
            return;
        }

        Vector2 input = movActivo.MoveInput;

        // Mismo movimiento
        Vector2 inputPasivo = new Vector2(input.x, input.y);

        movPasivo.SetInputForzado(inputPasivo);
    }
}
