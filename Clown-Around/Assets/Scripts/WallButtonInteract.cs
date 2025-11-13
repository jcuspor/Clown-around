using UnityEngine;
using UnityEngine.InputSystem; // <-- 1. ADD THIS

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
        // 2. CHANGE THIS LINE
        if (Keyboard.current.eKey.wasPressedThisFrame && dentro)
        {
            // This line might also give an error if 'anim' is not assigned.
            // Make sure you have 'anim = GetComponent<Animator>();' in an Awake() or Start() method.
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("player2"))
        {
            dentro = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("player2"))
        {
            dentro = false;
        } 
    }
}