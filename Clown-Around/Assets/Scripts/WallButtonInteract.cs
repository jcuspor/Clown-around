using UnityEngine;
using UnityEngine.InputSystem; 

public class WallButtonInteract : MonoBehaviour
{
    private Animator anim;
    public GameObject wall;
    private bool dentro;

    void Start()
    {
        anim = wall.GetComponent<Animator>();
    }
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && dentro)
        {
            if (anim.GetBool("IsUp") == false)
            {
                anim.SetBool("IsUp", true);
            }
            else
            {
                anim.SetBool("IsUp", false);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dentro = true;
        }else{
            dentro = false;
        }
    }
}