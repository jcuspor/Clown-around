using UnityEngine;
using UnityEngine.InputSystem; // NUEVO Input System
// Cambia esto si tu cámara ha cambiado de tipo:
using Unity.Cinemachine;

public class CambioPersonaje : MonoBehaviour
{
    public GameObject personaje1;
    public GameObject personaje2;
    public Movimiento movimiento1;   // Cambia por el nombre real de tu script de movimiento
    public Movimiento movimiento2;
    public Transform camTarget1;     // GameObject hijo en el cuello de personaje1
    public Transform camTarget2;     // Igual pero para personaje2
    public CinemachineCamera freeLookCam; // Usa el tipo actual de tu componente de cámara, puede ser CinemachineFreeLook o CinemachineCamera según versión

    public InputAction switchCharacterAction; // Acción del Input System (debes crearla y asignarla en el inspector)

    private int activo = 1;

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
            personaje1.GetComponent<Animator>().SetFloat("Speed",0);
            personaje1.tag = "player2";

            // Ajustar elevación para evitar hundimiento
            AjustarPosicionPersonaje(personaje2);
        }
        else
        {
            movimiento1.enabled = true;
            movimiento2.enabled = false;
            freeLookCam.Follow = camTarget1;
            freeLookCam.LookAt = camTarget1;
            activo = 1;
            personaje1.tag = "Player";
            personaje2.tag = "player2";
            personaje2.GetComponent<Animator>().SetFloat("Speed",0);

            // Ajustar elevación para evitar hundimiento
            AjustarPosicionPersonaje(personaje1);
        }
    }

    void AjustarPosicionPersonaje(GameObject personaje)
    {
        CharacterController cc = personaje.GetComponent<CharacterController>();
        if (cc != null)
        {
            Vector3 pos = personaje.transform.position;

            // Raycast corto hacia abajo para chequear el suelo
            RaycastHit hit;
            float rayDistance = 0.5f;
            if (Physics.Raycast(pos + Vector3.up * 0.1f, Vector3.down, out hit, rayDistance))
            {
                pos.y = hit.point.y + cc.height / 2; // Posicionar justo encima del suelo
            }
            else
            {
                pos.y += cc.skinWidth; // En caso de fallo, subir un poco para evitar penetración
            }
            personaje.transform.position = pos;
        }
    }


}

