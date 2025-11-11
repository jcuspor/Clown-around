using UnityEngine;
using UnityEngine.InputSystem; // <-- 1. ADD THIS

public class WallButtonInteract : MonoBehaviour, IInteractable
{
    private Animator anim;
    
    public void Interact()
    {

    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update() {
        // 2. CHANGE THIS LINE
        if (Keyboard.current.eKey.wasPressedThisFrame)
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
}