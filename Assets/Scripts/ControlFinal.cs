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
        // Define la variable que verifica si el juego termin�
        gameOver = GameManager.gameManager.ObtieneEstadoGameOver();

        // Actualiza el men� final
        ActualizaUIMenuFinal();


    }

    // Actuliza el canvas del men� final
    private void ActualizaUIMenuFinal()
    {

        // Sino si el juego est� corriendo y gameOver = false => el jugador gan�
        if (gameOver)
        {

            // Actualiza el texto de los puntos alcanzados
            puntosFinal.text = GameManager.gameManager.ObtienePuntos().ToString();

            // Actualiza el texto de la cantidad de 
            nivelesFinal.text = GameManager.gameManager.ObtieneNivelAcumulado().ToString();
        }

    }


    // Gestiona la accci�n del bot�n Men�
    public void BotonMenu()
    {
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();

    }
}
