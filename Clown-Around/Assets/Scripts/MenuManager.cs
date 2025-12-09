using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;



public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelSelectorNiveles;
    public GameObject panelOpciones;
    public GameObject splash;
    public GameObject creditos;
    public Button[] botonesNiveles; // Array con los botones de niveles en orden (asigna desde el inspector)
    public float waitTime = 10.0f;
    private bool splashSkipped = false;
    public Button defaultButton;
    

    void Start() {
        
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        // Al iniciar, sólo muestra el menú principal
        panelMenuPrincipal.SetActive(false);
        panelSelectorNiveles.SetActive(false);
        panelOpciones.SetActive(false);
        splash.SetActive(true);
        creditos.SetActive(false);
        
        
        if (PlayerPrefs.HasKey("VolverSelectorNiveles") && PlayerPrefs.GetInt("VolverSelectorNiveles") == 1) {
            panelMenuPrincipal.SetActive(false);
            panelSelectorNiveles.SetActive(true);
            panelOpciones.SetActive(false);
            splash.SetActive(false);
            PlayerPrefs.DeleteKey("VolverSelectorNiveles"); // Limpia para la próxima vez
        
        }

        int nivelDesbloqueado = PlayerPrefs.GetInt("NivelMaxDesbloqueado", 1); // Por defecto solo el 1 desbloqueado


        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            if (i < nivelDesbloqueado){
                botonesNiveles[i].interactable = true; // Desbloqueado
            }
            else{
                botonesNiveles[i].interactable = false; // Bloqueado
            }

        }

        if (splash.activeInHierarchy)
        {
            StartCoroutine(WaitAndLoadNextLevel());
        }
    }


    private void Update()
    {
        if (!splashSkipped && splash.activeInHierarchy && 
        (Keyboard.current.anyKey.wasPressedThisFrame || 
         Mouse.current.leftButton.wasPressedThisFrame))  
        {
            LoadNextLevel();
            splashSkipped = true;
        }
    }


    public void SalirClicked() {

        PlayerPrefs.DeleteAll();     // Borra todas las claves guardadas
        PlayerPrefs.Save(); 
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void CargarNivel1() {
        SceneManager.LoadScene("Scenes/Mar-Nivel_prueba");
        raton();
    }

    public void CargarNivel2() {
        SceneManager.LoadScene("Scenes/Nivel2");
        raton();
    }

    public void CargarNivel4() {
        SceneManager.LoadScene("Scenes/Nivel4");
        raton();
    }

     IEnumerator WaitAndLoadNextLevel()
    {
        yield return new WaitForSeconds(waitTime);
        if(splash.activeInHierarchy)
        
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        splash.SetActive(false);
        panelMenuPrincipal.SetActive(true);
        splashSkipped = true;
    }

    public void atras(){
        panelMenuPrincipal.SetActive(true);
        panelSelectorNiveles.SetActive(false);
    }

    public void raton()
    {
        Cursor.lockState = CursorLockMode.Locked; // deja el ratón en el centro de la ventana
        Cursor.visible = false; // pulsa Esc para desbloquearlo
    }
    
}
