using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CambioPersonaje : MonoBehaviour
{
    public GameObject personaje1;
    public GameObject personaje2;

    public Movimiento movimiento1;
    public Movimiento movimiento2;

    public Transform camTarget1;
    public Transform camTarget2;
    public CinemachineCamera freeLookCam;

    public InputAction switchCharacterAction; // R
    public EnlacePersonaje enlace;

    private int activo = 1;
    public int Activo => activo;

    void OnEnable()
    {
        switchCharacterAction.Enable();
    }

    void OnDisable()
    {
        switchCharacterAction.Disable();
    }

    void Start()
    {
        switchCharacterAction.performed += ctx => CambiaPersonaje();

        movimiento1.enabled = true;
        movimiento2.enabled = false;

        freeLookCam.Follow = camTarget1;
        freeLookCam.LookAt = camTarget1;

        personaje1.tag = "Player";
        personaje2.tag = "player2";
    }

    private void CambiaPersonaje()
    {
        // 🔥 SIEMPRE DESENLAZAR AL CAMBIAR
        if (enlace != null)
            enlace.ForzarDesenlaceTotal();

        if (activo == 1)
        {
            // Cambiar a personaje 2
            movimiento1.acabarCaminando();
            movimiento1.enabled = false;
            movimiento2.enabled = true;

            freeLookCam.Follow = camTarget2;
            freeLookCam.LookAt = camTarget2;

            activo = 2;

            personaje2.tag = "Player";
            personaje1.tag = "player2";

            personaje1.GetComponent<Animator>().SetFloat("Speed", 0f);

            AjustarPosicionPersonaje(personaje2);
        }
        else
        {
            // Cambiar a personaje 1
            movimiento2.acabarCaminando();
            movimiento2.enabled = false;
            movimiento1.enabled = true;

            freeLookCam.Follow = camTarget1;
            freeLookCam.LookAt = camTarget1;

            activo = 1;

            personaje1.tag = "Player";
            personaje2.tag = "player2";

            personaje2.GetComponent<Animator>().SetFloat("Speed", 0f);

            AjustarPosicionPersonaje(personaje1);
        }
    }

    void AjustarPosicionPersonaje(GameObject personaje)
    {
        CharacterController cc = personaje.GetComponent<CharacterController>();
        if (cc != null)
        {
            Vector3 pos = personaje.transform.position;

            RaycastHit hit;
            float rayDistance = 0.5f;

            if (Physics.Raycast(pos + Vector3.up * 0.1f, Vector3.down, out hit, rayDistance))
            {
                pos.y = hit.point.y + cc.height / 2;
            }
            else
            {
                pos.y += cc.skinWidth;
            }

            personaje.transform.position = pos;
        }
    }
}
