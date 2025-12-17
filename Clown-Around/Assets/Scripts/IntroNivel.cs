using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem; // <--- ESTA ES LA LÍNEA QUE TE FALTA

public class IntroNivel : MonoBehaviour
{
    public GameObject canvasHistoria; // El Canvas que creamos
    public Image displayImage;        // El objeto VinyetaDisplay
    public List<Sprite> vinyetas;     // Tus imágenes de historia
    public float tiempoPorImagen = 5f;
    
    // OPCIONAL: Arrastra aquí a tu jugador para que no se mueva durante la intro
    public MonoBehaviour scriptMovimientoJugador; 

    private int indexActual = 0;
    private float timer = 0f;

    void Start()
    {
        // Bloqueamos el movimiento si lo hemos asignado
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = false;

        if (vinyetas.Count > 0)
        {
            canvasHistoria.SetActive(true);
            displayImage.sprite = vinyetas[indexActual];
        }
        else
        {
            TerminarIntro();
        }
    }

    void Update()
        {
            timer += Time.deltaTime;

            // Versión para el NUEVO Input System
            // Detecta si se ha pulsado cualquier tecla o botón del ratón en este frame
            bool teclaPulsada = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
            bool clicPulsado = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

            if (timer >= tiempoPorImagen || teclaPulsada || clicPulsado)
            {
                PasarSiguiente();
            }
        }

    void PasarSiguiente()
    {
        indexActual++;
        timer = 0f;

        if (indexActual < vinyetas.Count)
        {
            displayImage.sprite = vinyetas[indexActual];
        }
        else
        {
            TerminarIntro();
        }
    }

    void TerminarIntro()
    {
        // Desactivamos toda la capa de la historia
        canvasHistoria.SetActive(false);
        
        // Reactivamos al jugador
        if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = true;

        // Destruimos este objeto para que no gaste memoria
        Destroy(gameObject);
    }
}