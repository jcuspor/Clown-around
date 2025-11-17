using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelSelectorNiveles;
    public GameObject panelOpciones;

    void Start() {
        // Al iniciar, sólo muestra el menú principal
        panelMenuPrincipal.SetActive(true);
        panelSelectorNiveles.SetActive(false);
        panelOpciones.SetActive(false);
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
}
