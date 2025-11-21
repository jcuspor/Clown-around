
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
            PlayerPrefs.SetInt("VolverSelectorNiveles", 1); // 1 = ir al selector de niveles
            PlayerPrefs.Save();
            Debug.Log("¡Nivel completado!");
            int nextLevel = 2;
            PlayerPrefs.SetInt("NivelMaxDesbloqueado", nextLevel);
            PlayerPrefs.Save();
            StartCoroutine(EsperarYCambiarMenu());
        }
    }

    System.Collections.IEnumerator EsperarYCambiarMenu()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        SceneManager.LoadScene("Scenes/UI");
    }
}
