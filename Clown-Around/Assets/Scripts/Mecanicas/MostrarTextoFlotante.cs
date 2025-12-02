using UnityEngine;
using UnityEngine.InputSystem;


public class MostrarTextoFlotante : MonoBehaviour {

      public GameObject textoFlotante; // El objeto texto sobre el botón (Canvas World Space)
    public InputAction hideHelpTextAction; // Acción del Input System, arrástrala en el Inspector
    private bool mostrado = false;
    private bool puedeOcultarse = false;

    void Start() {
        textoFlotante.SetActive(false); // Texto invisible al empezar
        hideHelpTextAction.performed += ctx => OcultaTexto(); // Registra callback
    }

    void OnEnable() {
        hideHelpTextAction.Enable();
    }
    void OnDisable() {
        hideHelpTextAction.Disable();
    }

    private void OnTriggerEnter(Collider other) {
        if (!mostrado && other.CompareTag("Player")) {
            textoFlotante.SetActive(true);
            mostrado = true;        // Ya lo mostró una vez
            puedeOcultarse = true; // Permite ocultarlo al pulsar E
        }
    }

    private void OcultaTexto() {
        if (puedeOcultarse && textoFlotante.activeSelf) {
            textoFlotante.SetActive(false);
            puedeOcultarse = false; // No se podrá volver a mostrar
        }
    }
}

