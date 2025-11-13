using UnityEngine;

public class Meta2 : MonoBehaviour
{
    private FinNivel fin_nivel;

    void Start()
    {
        fin_nivel = Object.FindFirstObjectByType<FinNivel>();
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("El jugador 2 ha llegado a la meta");
        fin_nivel.PlayerReachedGoal(2);
    }

    void OnTriggerExit(Collider other)
    {
        fin_nivel.PlayerLeftGoal(2);
    }
}

