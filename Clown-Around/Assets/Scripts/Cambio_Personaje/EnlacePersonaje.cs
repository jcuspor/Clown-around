using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.Experimental.GraphView;

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
    [SerializeField] GameObject particulas1;
    [SerializeField] GameObject particulas2;
    private Vector3 direction1;
    private Vector3 direction2;

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
        enlaceAction.started += ctx => ToggleEnlace();; 
    }

    void Update()
    {
        if (!enlazados) return;
        AplicarInputEspejo();
            if(cambioPersonaje.Activo == 1)
            {
                direction1 = particulas2.transform.position - particulas1.transform.position;
                particulas1.transform.rotation = Quaternion.LookRotation(direction1); 
            }
            else
            {
                particulas2.transform.rotation = Quaternion.LookRotation(direction2);
                direction2 = particulas1.transform.position - particulas2.transform.position;  
            }
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
            particulas1.SetActive(false);
            particulas2.SetActive(false);

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
            particulas1.SetActive(false);
            particulas2.SetActive(false);
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
            particulas1.SetActive(true);
        }
        else
        {
            movActivo = movimiento2;
            movPasivo = movimiento1;
            particulas2.SetActive(true);
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
