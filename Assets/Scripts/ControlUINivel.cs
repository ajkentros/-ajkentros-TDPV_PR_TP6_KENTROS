using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlUINivel : MonoBehaviour
{
    [Header("Panel Nivel")]
    [SerializeField] private GameObject panelNivel;                     // Referencia al panel menu
    [SerializeField] private TextMeshProUGUI textoNivelCompleto;        // Referencia al componente Text de la dimension del juego
    [SerializeField] private TextMeshProUGUI numeroNivelCompleto;       // Referencia al componente Text de la dimension del juego
    [SerializeField] private Button botonSiguienteNivel;                // Referencia al bot�n siguiente nivel
    [SerializeField] private Button botonRepetirNivel;                  // Referencia al bot�n repetir nivel

    [Header("Panel Juego")]
    [SerializeField] private TextMeshProUGUI tiempoCronometro;      // Referencia al componente Text del cron�metro
    [SerializeField] private TextMeshProUGUI nivel;                 // Referencia al componente Text de la dimension del juego
    [SerializeField] private TextMeshProUGUI puntos;                // Referencia al componente Text de los puntos

    private bool colisionSuelo;     // Referencia al control si colisiona con el suelo
    private bool colisionAgujero;   // Referencia al control si colisiona con el agujero
    private bool tiempoSuperado;    // Referencia al control si el tiempo del nivel se ha superado

    // Start is called before the first frame update
    void Start()
    {
        // Activa panel del nivel
        panelNivel.SetActive(false);

        // Configura los flags de inicio
        colisionSuelo = false;
        colisionAgujero = false;
        tiempoSuperado = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Actualiza el panel del juego
        ActualizaPanelJuego();

        // Actualiza panel de nivel
        ActualizaPanelNivel();

        // Controla el teclado (ENTRER)
        ControlaTeclado();

        
    }

    // Gestiona la actualizaci�n del panel juego
    private void ActualizaPanelJuego()
    {
        // Obtiene el tiempo de juego actual del GameManager y actualiza el texto del cron�metro
        float tiempoActual = GameManager.gameManager.ObtieneTiempoRestante();

        // Actualiza el TextMeshPro con el tiempo formateado 
        tiempoCronometro.text = FormateaTiempo(tiempoActual);

        // Actualiza el texto de los puntos alcanzados
        puntos.text = GameManager.gameManager.ObtienePuntos().ToString();

        // Actualiza el texto de los puntos alcanzados
        nivel.text = GameManager.gameManager.ObtieneNivelAcumulado().ToString();

    }

    // Gestiona la actualizaci�n del panel nivel
    private void ActualizaPanelNivel()
    {
        // Actualiza controles desde GameManager
        colisionSuelo = GameManager.gameManager.ObtieneColisionSuelo();
        colisionAgujero = GameManager.gameManager.ObtieneColisionAgujero();
        tiempoSuperado = GameManager.gameManager.ObtieneTiempoSuperado();

        // Si colisiona con el suelo => ActualizaPaneNivelSiguiente()
        if (colisionSuelo)
        {
            ActualizaPaneNivelSiguiente();
        }
        else if(colisionAgujero || tiempoSuperado)      // Si No Si colisionAgujero = true o tiempoSuperado = true => ActualizaPaneNivelRepite();
        {
            ActualizaPaneNivelRepite();
        }
    }
    
    // Actualiza el Panel Nivel cuando colisiona con el suelo
    private void ActualizaPaneNivelSiguiente()
    {
        // activa el PanelNivel
        panelNivel.SetActive(true);

        // Activa el bot�n Siguiente y desactiva el bot�n Repetir
        botonSiguienteNivel.gameObject.SetActive(true);
        botonRepetirNivel.gameObject.SetActive(false);

        // Deshabilita el interactable del bot�n Repetir
        botonSiguienteNivel.interactable = true;
        botonRepetirNivel.interactable = false;

        // completa campos de numeroNivelCompleto y textoNivelCompleto
        numeroNivelCompleto.text = GameManager.gameManager.ObtieneNivelAcumulado().ToString();
        textoNivelCompleto.text = "Completo";

    }

    // Actualiza el Panel Nivel cuando colisiona con el Agujero
    private void ActualizaPaneNivelRepite()
    {
        // activa el PanelNivel
        panelNivel.SetActive(true);

        // Activa el bot�n Siguiente y desactiva el bot�n Repetir
        botonSiguienteNivel.gameObject.SetActive(false);
        botonRepetirNivel.gameObject.SetActive(true);

        // Deshabilita el interactable del bot�n Repetir
        botonSiguienteNivel.interactable = false;
        botonRepetirNivel.interactable = true;

        // completa campos de numeroNivelCompleto y textoNivelCompleto
        numeroNivelCompleto.text = GameManager.gameManager.ObtieneNivelAcumulado().ToString();
        textoNivelCompleto.text = "Incompleto";

        // Si el tiempo se ha superado => 
        if (tiempoSuperado)
        {
            tiempoCronometro.text = "AGOTADO";
        }

    }

    // Controla la entrada del teclado
    private void ControlaTeclado()
    {
        // Si clic en la tecla entre o return => habilita los botones
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Verifica si el panel est� activo y si el bot�n Siguiente est� interactuable
            if (panelNivel.activeSelf && botonSiguienteNivel.gameObject.activeSelf && botonSiguienteNivel.interactable)
            {
                // Ejecuta el m�todo asociado al bot�n Siguiente
                BotonSiguienteNivel();
            }
            // Verifica si el panel est� activo y si el bot�n Repetir est� interactuable
            else if (panelNivel.activeSelf && botonRepetirNivel.gameObject.activeSelf && botonRepetirNivel.interactable)
            {
                // Ejecuta el m�todo asociado al bot�n Repetir
                BotonRepetirNivel();
            }
        }
    }
    // Gestiona el bot�n Jugar
    public void BotonMenu()
    {
        // Reinicia las variables del juego
        GameManager.gameManager.IniciaMenu();


    }

    // Gestiona el bot�n Continuar
    public void BotonSiguienteNivel()
    {
        // Llama al m�todo en Game Manager que cambia el estado del falg ganaNivel
        GameManager.gameManager.GanaNivel();
        panelNivel.SetActive(false);
        
    }

    // Gestiona el bot�n Continuar
    public void BotonRepetirNivel()
    {
        // Llama al m�todo en Game Manager que cambia el estado del falg repiteNivel
        GameManager.gameManager.RepiteNivel();
        panelNivel.SetActive(false); ;

    }

    // Gestiona el formarto de tiempo para mostrar en minutos y segundos
    private string FormateaTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);

        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
