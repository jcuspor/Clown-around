using UnityEngine;
using UnityEngine.InputSystem;

public class WallButtonInteract : MonoBehaviour
{
    public GameObject wall; // Debes asignar este objeto en el Inspector
    private Animator anim;
    private Animator animator;
    private GameObject jugadorEnZona;
    private bool dentro = false;

    void Start()
    {
        anim = wall.GetComponent<Animator>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && dentro && jugadorEnZona != null)
        {
            if (jugadorEnZona.CompareTag("Player"))
            {
                anim.SetBool("IsUp", !anim.GetBool("IsUp"));
                animator.SetTrigger("Press");
            }
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dentro = true;
            jugadorEnZona = other.gameObject;
        }
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dentro = true;
            jugadorEnZona = other.gameObject;
        }
        else
        {
            dentro = false;
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
