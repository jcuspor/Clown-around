using UnityEngine;

public class Meta1 : MonoBehaviour
{
    private FinNivel fin_nivel;
    [SerializeField] private GameObject pieza;
    private Animator animator;

    void Start()
    {
        fin_nivel = Object.FindFirstObjectByType<FinNivel>();
        animator = pieza.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("El jugador 1 ha llegado a la meta");
        fin_nivel.PlayerReachedGoal(1);
        animator.speed = 1.8f;
    }

    void OnTriggerExit(Collider other)
    {
        fin_nivel.PlayerLeftGoal(1);  
        animator.speed = 1;  
    }
}
