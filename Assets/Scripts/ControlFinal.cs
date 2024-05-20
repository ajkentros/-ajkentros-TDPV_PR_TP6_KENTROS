using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlFinal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntosFinal;           // Referencia variable al texto de los puntos finales alcanzados
    [SerializeField] private TextMeshProUGUI nivelesFinal;       // Referencia variable al texto de cantidad de cuerpos alcanzados

    private bool gameOver;      // Referencia al control de game over

    // Update is called once per frame
    void Update()
    {
        // Define la variable que verifica si el juego terminó
        gameOver = GameManager.gameManager.ObtieneEstadoGameOver();

        // Actualiza el menú final
        ActualizaUIMenuFinal();


    }

    // Actuliza el canvas del menú final
    private void ActualizaUIMenuFinal()
    {

        // Sino si el juego está corriendo y gameOver = false => el jugador ganó
        if (gameOver)
        {

            // Actualiza el texto de los puntos alcanzados
            puntosFinal.text = GameManager.gameManager.ObtienePuntos().ToString();

            // Actualiza el texto de la cantidad de 
            nivelesFinal.text = GameManager.gameManager.ObtieneNivelAcumulado().ToString();
        }

    }


    // Gestiona la accción del botón Menú
    public void BotonMenu()
    {
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();

    }
}
