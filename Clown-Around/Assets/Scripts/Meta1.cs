using UnityEngine;

public class Meta1 : MonoBehaviour
{
    private FinNivel fin_nivel;

    void Start()
    {
        fin_nivel = Object.FindFirstObjectByType<FinNivel>();
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("El jugador 1 ha llegado a la meta");
        fin_nivel.PlayerReachedGoal(1);
    }

    void OnTriggerExit(Collider other)
    {
        fin_nivel.PlayerLeftGoal(1);    
    }
}
