using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelSelectorNiveles;
    public GameObject panelOpciones;
    public Button[] botonesNiveles; // Array con los botones de niveles en orden (asigna desde el inspector)

    void Start() {
        // Al iniciar, sólo muestra el menú principal
        panelMenuPrincipal.SetActive(true);
        panelSelectorNiveles.SetActive(false);
        panelOpciones.SetActive(false);
        
        
        if (PlayerPrefs.HasKey("VolverSelectorNiveles") && PlayerPrefs.GetInt("VolverSelectorNiveles") == 1) {
            panelMenuPrincipal.SetActive(false);
            panelSelectorNiveles.SetActive(true);
            panelOpciones.SetActive(false);
            PlayerPrefs.DeleteKey("VolverSelectorNiveles"); // Limpia para la próxima vez
        
        }

        int nivelDesbloqueado = PlayerPrefs.GetInt("NivelMaxDesbloqueado", 1); // Por defecto solo el 1 desbloqueado
        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            if (i < nivelDesbloqueado)
                botonesNiveles[i].interactable = true; // Desbloqueado
            else
                botonesNiveles[i].interactable = false; // Bloqueado
        }
    }

    public void JugarClicked() {
        panelMenuPrincipal.SetActive(false);
        panelSelectorNiveles.SetActive(true);
    }

    public void OpcionesClicked() {
        panelMenuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void SalirClicked() {
        Application.Quit(); // Solo funciona fuera del editor
    }

    public void CargarNivel1() {
        SceneManager.LoadScene("Scenes/Mar-Nivel_prueba");
    }

    public void CargarNivel2() {
        SceneManager.LoadScene("Scenes/Nivel2");
    }
}
