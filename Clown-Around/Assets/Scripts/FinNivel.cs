
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    private bool player1AtGoal = false;
    private bool player2AtGoal = false;

    public void PlayerReachedGoal(int playerNumber)
    {
        if (playerNumber == 1)
            player1AtGoal = true;
        else if (playerNumber == 2)
            player2AtGoal = true;

        CheckIfLevelComplete();
    }

    public void PlayerLeftGoal(int playerNumber)
    {
        if (playerNumber == 1)
            player1AtGoal = false;
        else if (playerNumber == 2)
            player2AtGoal = false;
    }

    
    void CheckIfLevelComplete()
    {
        if (player1AtGoal && player2AtGoal)
        {
            PlayerPrefs.SetInt("VolverSelectorNiveles", 1);
            PlayerPrefs.Save();
            Debug.Log("¡Nivel completado!");

            // Ejemplo: si usas el índice de la escena actual como identificador de nivel
            int nivelActual = SceneManager.GetActiveScene().buildIndex;

            int nivelMaxDesbloqueado = PlayerPrefs.GetInt("NivelMaxDesbloqueado", 1);

            // Solo actualiza el progreso si este nivel es el más alto logrado
            if (nivelActual >= nivelMaxDesbloqueado)
            {
                PlayerPrefs.SetInt("NivelMaxDesbloqueado", nivelActual + 1); // Desbloquea el siguiente nivel
                PlayerPrefs.Save();
            }

            StartCoroutine(EsperarYCambiarMenu());
        }
    }



    System.Collections.IEnumerator EsperarYCambiarMenu()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        SceneManager.LoadScene("Scenes/UI");
    }
}
