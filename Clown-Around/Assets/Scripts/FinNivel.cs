
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

            if (nivelActual == 0){
                StartCoroutine(EsperarYCambiarMenu());
                ratonMenuVolver();
                SceneManager.LoadScene("Scenes/Mar-Nivel_prueba"); // Nivel 1
            }
            else if (nivelActual == 1){
                StartCoroutine(EsperarYCambiarMenu());
                ratonMenuVolver();
                SceneManager.LoadScene("Scenes/Nivel2");           // Nivel 2
            }
            else if (nivelActual == 2){
                SceneManager.LoadScene("Scenes/Nivel3");           // Nivel 3
            }
            else if (nivelActual == 3){
                SceneManager.LoadScene("Scenes/Nivel4");           // Nivel 4 🎯
            }
            else{
                StartCoroutine(EsperarYCambiarMenu());
                ratonMenu();
                SceneManager.LoadScene("Scenes/UI");               // Menú
            }

        }
    }



    System.Collections.IEnumerator EsperarYCambiarMenu()
    {
        yield return new WaitForSeconds(1f); // Espera 1 segundo
        
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
        else if(Keyboard.current.escapeKey.wasPressedThisFrame && menu){
            panel.SetActive(false);
            ratonMenuVolver();
            menu = false;
        }
    }

    public void salirMenu()
    {
        panel.SetActive(false);
        ratonMenuVolver();
        menu = false;
    }

    public void seleccionNiveles()
    {
        SceneManager.LoadScene("Scenes/UI");
        ratonMenu();
    }
}
