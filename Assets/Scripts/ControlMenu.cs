using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;               // Referencia al panel menu
    [SerializeField] private GameObject panelInstrucciones;      // Referencia al panel instrucciones

    private void Start()
    {
        // Configura paneles
        panelMenu.SetActive(true);
        panelInstrucciones.SetActive(false);
    }
    // Gestiona el botón Jugar
    public void BotonJugar()
    {
        // Reinicia las variables del kuego
        GameManager.gameManager.IniciaJuego();

    }

    // Gestiona el cierre de la aplicación
    public void SaleJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Gestiona el btoón Instrucciones
    public void BotonInstrucciones()
    {
        panelMenu.SetActive(false);
        panelInstrucciones.SetActive(true);
    }

    // Gestiona el botón Volver
    public void BotonVolver()
    {
        panelMenu.SetActive(true);
        panelInstrucciones.SetActive(false);
    }
}
