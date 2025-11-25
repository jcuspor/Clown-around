using UnityEngine;
using UnityEngine.InputSystem;

public class WallButtonInteract : MonoBehaviour
{
    public GameObject wall; // Debes asignar este objeto en el Inspector
    private Animator anim;
    private GameObject jugadorEnZona;
    private bool dentro;

    void Start()
    {
        anim = wall.GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && dentro && jugadorEnZona != null)
        {
            if (jugadorEnZona.CompareTag("Player"))
            {
                anim.SetBool("IsUp", !anim.GetBool("IsUp"));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dentro = true;
            jugadorEnZona = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject == jugadorEnZona)
        {
            dentro = false;
            jugadorEnZona = null;
        }
    }
}
