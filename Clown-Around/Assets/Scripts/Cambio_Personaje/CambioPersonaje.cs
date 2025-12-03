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

    public InputAction switchCharacterAction;

    private int activo = 1;
    public int Activo => activo;

    public EnlacePersonaje enlace;

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
        if (activo == 1)
        {
            movimiento1.enabled = false;
            movimiento2.enabled = true;

            freeLookCam.Follow = camTarget2;
            freeLookCam.LookAt = camTarget2;

            activo = 2;
            personaje2.tag = "Player";
            personaje1.GetComponent<Animator>().SetFloat("Speed", 0f);
            personaje1.tag = "player2";

            if (enlace != null) enlace.NotificarCambioActivo();

            AjustarPosicionPersonaje(personaje2);
        }
        else
        {
            movimiento2.enabled = false;
            movimiento1.enabled = true;

            freeLookCam.Follow = camTarget1;
            freeLookCam.LookAt = camTarget1;

            activo = 1;
            personaje1.tag = "Player";
            personaje2.tag = "player2";
            personaje2.GetComponent<Animator>().SetFloat("Speed", 0f);

            if (enlace != null) enlace.NotificarCambioActivo();

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
