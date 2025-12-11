
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class FinNivel : MonoBehaviour
{
    private bool player1AtGoal = false;
    private bool player2AtGoal = false;
    [SerializeField] private GameObject Meta1;
    [SerializeField] private GameObject Meta2;
    public GameObject burstParticle;
    public GameObject panel;
    private bool menu = false;

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
            Instantiate(burstParticle, Meta1.transform.position, Quaternion.identity);
            Instantiate(burstParticle, Meta2.transform.position, Quaternion.identity);
            Destroy(Meta1);
            Destroy(Meta2);

            // Ejemplo: si usas el índice de la escena actual como identificador de nivel
            int nivelActual = SceneManager.GetActiveScene().buildIndex;

            int nivelMaxDesbloqueado = PlayerPrefs.GetInt("NivelMaxDesbloqueado", 1);

            // Solo actualiza el progreso si este nivel es el más alto logrado
            if (nivelActual >= nivelMaxDesbloqueado)
            {
                PlayerPrefs.SetInt("NivelMaxDesbloqueado", nivelActual + 1); // Desbloquea el siguiente nivel
                PlayerPrefs.Save();
            }

            if (nivelActual == 2 ){
                
                ratonMenu();
                StartCoroutine(EsperarYCambiarMenu());
            }else if (nivelActual == 0){
                
                ratonMenuVolver();
                SceneManager.LoadScene("Scenes/Mar-Nivel_prueba");
            }else if (nivelActual == 1){
                
                ratonMenuVolver();
                SceneManager.LoadScene("Scenes/Nivel2");
            }else{
                
                ratonMenu();
                StartCoroutine(EsperarYCambiarMenu());
            }
        }
    }



    System.Collections.IEnumerator EsperarYCambiarMenu()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        ratonMenu();
        SceneManager.LoadScene("Scenes/UI");
    }

    public void ratonMenu()
    {
        Cursor.lockState = CursorLockMode.None;// deja el ratón en el centro de la ventana
        Cursor.visible = true; // pulsa Esc para desbloquearlo
    }

    public void ratonMenuVolver()
    {
        Cursor.lockState = CursorLockMode.Locked;// deja el ratón en el centro de la ventana
        Cursor.visible = false; // pulsa Esc para desbloquearlo
    }


    public void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame && !menu){
            panel.SetActive(true);
            ratonMenu();
            menu = true;
        }
    }
}
