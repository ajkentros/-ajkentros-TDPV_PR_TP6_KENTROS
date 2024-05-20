using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;     // Referencia instancia estática del GameManager para acceder desde otros scripts

    [SerializeField] private int cantidadNiveles = 10;       // Referencia a la cantidad de niveles que tiene cada escena
    [SerializeField] private float tiempoNivel = 20f;       // Referencia al tiempo de duración del nivel 1

    private int puntos;                 // Referencia a los puntos alcanzados
    private int cantidadEscenas;        // Referencia a la cantidad de escenas del jeugo

    private int nivelActual;            // Referencia al contador de niveles jugados (escena)
    private int nivelAcumulado;         // Referencia a los niveles acumulados
    private int escena;                 // Referencia a la escena actual

    private float tiempoJuego = 0f;     // Referencia al tiempo actual de juego
    private float tiempoActualNivel;    // Referencia al tiempo actual de cada nivel

    private bool gameOver;              // Referencia controla el gameOver del juego (si termina o no)
    private bool juegoPausado;          // Referencia controla si el juego está pausado o no

    private bool colisionSuelo;         // Referencia controla si la forma colisiona con el suelo
    private bool colisionAgujero;       // Referencia controla si la forma colisiona con el agujero

    private bool ganaNivel;             // Referencia controla si el jugador gana el nivel actual
    private bool repiteNivel;           // Referencia controla si se repite el nivel

    private bool tiempoSuperado;        // Referencia controla si se supera el tiempo del nivel

    public UnityEvent OnActualizaNivel;     // Evento que se activa cuando la forma colisiona con el suelo


    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject); // Esto evitará que el objeto GameManager se destruya al cargar una nueva escena.
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        IniciaStart();

    }

    private void Update()
    {
        //Debug.Log("gameOver: " + gameOver +" " + "juegoPausado " + juegoPausado);
        // Si el juego no termino y no está pausado => 
        if (!gameOver && !juegoPausado)
        {
            // el tiempo esté en marcha al inicio 
            Time.timeScale = 1f;

            // Actualiza el tiempo de juego si el juego no ha terminado y no está pausado
            tiempoJuego += Time.deltaTime;

            // Verifica tiempo de juego no supere el tiempo del nivel (definido por nivel)
            VerificaTiempoJuego();

            // Verifica si se llega al final del jeugo y activa la escena final
            VerificaEscenaFinal();

        }
        else if (juegoPausado) // Si el jeugo está pausado
        {
            // Si colisiona con el suelo
            // Si colisiona con el agujero
            // Si se agotó el tiempo
            if (colisionSuelo)
            {
                ActualizaJuegoCuandoColisionSuelo();

            } 
            else if (colisionAgujero)
            {
                ActualizaJuegoCuandoColisionAgujero();

            }
            else if (tiempoSuperado)
            {
                ActualizaJuegoCuandoTiempoSuperado();

            }
            
        }
        else if (gameOver) // Si el jeugo está en game over
        {
            // Inicia la escena Menú Final
            SceneManager.LoadScene(escena);

        }

    }

    // Gestiona el inicio de las variables
    private void IniciaStart()
    {
        tiempoJuego = 0;
        tiempoActualNivel = tiempoNivel;
        puntos = 0;
        nivelActual = 1;          // Inicia en el nivel 1
        nivelAcumulado = 1;
        escena = 0;
        cantidadEscenas = SceneManager.sceneCountInBuildSettings;

        gameOver = false;
        juegoPausado = false;
        ganaNivel = false;
        colisionSuelo = false;
        colisionAgujero = false;
        repiteNivel = false;
        tiempoSuperado = false;



    }

    // Gestiona la colisión con el suelo llamando a la corrutina
    public void ActualizaJuegoCuandoColisionSuelo()
    {
       // Debug.Log("ActualizaJuegoCuandoColisionSuelo");
        StartCoroutine(EsperaGanaNivel());

    }


    // Espera a que ganeNivel (clic en el botón siguiente) sea verdadero antes de reanudar el tiempo de juego
    private IEnumerator EsperaGanaNivel()
    {
       
        // Espera a que ganeNivel = true
        while (!ganaNivel)
        {
            yield return null;
        }

        // Si el nivelActual + 1 > cantidadNiveles => 

        if (nivelActual + 1 > cantidadNiveles)
        {
            //Debug.Log("nueva escena " + "nivel " + nivelActual + "nivelAcumulado " + nivelAcumulado);
         
            // Verifica y cambia de escena
            CargaNuevaEscena();
        }
        else
        {
            //Debug.Log("nivel " + nivelActual + "nivelAcumulado " + nivelAcumulado);

            // Incrementa nivelActual y nivelAcumlado
            nivelActual += 1;
            nivelAcumulado += 1;

            // Incrementa tiempo nivel
            IncrementaTiempoNivel();

            // Reinicia el tiempo de juego
            tiempoJuego = 0;

            // Dispara el evento OnActualizaNivel (avisa a la FormaController)
            OnActualizaNivel.Invoke();

            // Cambia el flag ganaNivel
            ganaNivel = false;

            // Cambia el flag colisionSuelo
            colisionSuelo = false;

            // Cambia el flag de colisionSuelo
            CambiaColisionSuelo(false);

            // Cambia el flag juegoPausado
            juegoPausado = false;

            // Reanuda el tiempo de juego
            Time.timeScale = 1f;

        }
        

    }

    // Gestiona la colisión con el Agujero
    private void ActualizaJuegoCuandoColisionAgujero()
    {

        //Debug.Log("ActualizaJuegoCuandoColisionAgujero");

        // Espera a que repiteNivel sea verdadero antes de reanudar el tiempo de juego
        StartCoroutine(EsperaRepiteNivel());

    }

    // Gestiona la reanudación del juego
    private IEnumerator EsperaRepiteNivel()
    {
        // Espera a que repiteNivel = true
        while (!repiteNivel)
        {
            yield return null;
        }

        // Reinicia el tiempo de juego
        tiempoJuego = 0;

        // Dispara el evento de OnActualizaNivel (avisa a la FormaController)
        OnActualizaNivel.Invoke();

        // Cambia el flag repiteNivel
        repiteNivel = false;

        // Cambia el flag colisionAgujero
        colisionAgujero = false;

        // Cambia el flag tiempoSuperado
        tiempoSuperado = false;

        // Cambia el flag juegoPausado
        juegoPausado = false;

        // Reanuda el tiempo de juego
        Time.timeScale = 1f;


    }

    // Gestiona cuando el tiempoJuego > tiempoNivel
    private void ActualizaJuegoCuandoTiempoSuperado()
    {

        //Debug.Log("ActualizaJuegoCuandoTiempoSuperado");
  
        // Espera a que repiteNivel sea verdadero antes de reanudar el tiempo de juego
        StartCoroutine(EsperaRepiteNivel());

    }

    // Gestiona el estado de los puntos y la escena actual
    private void VerificaTiempoJuego()
    {
        // Define variable tiempoRestante = tiempoNivel - tiempoJuego
        float tiempoRestante = ObtieneTiempoRestante();

        // Si tiempoJuego > tiempoNivel => 
        if (tiempoRestante <= 0)
        {
            // Pausa el juego
            PausarJuego();

            // Cambia el flag
            tiempoSuperado = true;
        }

    }


    private void CargaNuevaEscena()
    {
        
        // Obtiene el valor de la escena actual
        escena = SceneManager.GetActiveScene().buildIndex;

        // Obtiene la cabtidad de escenas del jeugo
        cantidadEscenas = SceneManager.sceneCountInBuildSettings;

        // Asigna el valor de escena a la escena siguiente
        escena = (escena + 1) % cantidadEscenas;

        // Reinicia el tiempo de juego
        tiempoJuego = 0;

        // Reinicia la escenaActual = 1 e incrementa nivelAcumulado
        nivelActual = 1;
        nivelAcumulado += 1;

        // Dispara el evento de OnActualizaNivel (avisa a la FormaController)
        OnActualizaNivel.Invoke();

        // Cambia el flag ganaNivel
        ganaNivel = false;

        // Cambia el flag colisionSuelo
        repiteNivel = false;

        // Cambia el flag colisionSuelo
        colisionSuelo = false;

        // Cambia el flag colisionAgujero
        colisionAgujero = false;

        // Cambia el flag tiempoSuperado
        tiempoSuperado = false;

        // Cambia el flag juegoPausado
        juegoPausado = false;

        // Reanuda el tiempo de juego
        Time.timeScale = 1f;

        // Carga la nueva escena calculada en VerificaCantidadEscenas()
        SceneManager.LoadScene(escena);
        
    }

    // Gestiona si se ha llegado a la escena final (Menú Final)
    private void VerificaEscenaFinal()
    {

        // Si la escena = a la última => el juego terminó (ganaste)
        if (escena == SceneManager.sceneCountInBuildSettings - 1)
        {
            // Cambia a la escena del menú final
            escena = SceneManager.sceneCountInBuildSettings - 1;

            // Ajusta el valor de nivelAcumulado
            nivelAcumulado -= 1;

            // Cambia la bandera de gameOver = True para indicar que el juego finalizó
            gameOver = true;

            // Pausa el jeugo
            PausarJuego();

        }
    }
    // Gestiona el cambio de nivel
    public void GanaNivel()
    {
        ganaNivel = true;

    }

    // Gestiona la repeticiçon del nivel
    public void RepiteNivel()
    {
        repiteNivel = true;
    }

    // Método para obtener el tiempo de nivel según el nivel actual
    private float IncrementaTiempoNivel()
    {
        // Calcula el tiempo de nivel según el nivel actual
        tiempoActualNivel = tiempoNivel + nivelActual;
        
        return tiempoActualNivel; // Por ejemplo, 60 segundos para cada nivel
    }

    // Obtiene el nivel acumulado
    public int ObtieneNivelAcumulado()
    {
        return nivelAcumulado;
    }

    // Obtiene el nivel actual
    public int ObtieneNivelActual()
    {
        return nivelActual;
    }

    // Gestiona la pausa del juego
    public void PausarJuego()
    {
        // Detiene el tiempo de juego
        Time.timeScale = 0f;
        
        // Pausa el juego
        juegoPausado = true; 
    }

    // Gestiona el reinicio del jeugo
    public void IniciaJuego()
    {
        IniciaStart();

        // Carga la escena 1
        SceneManager.LoadScene(1);
    }

    // Gestiona el inicio del Menú de Inicio del juego
    public void IniciaMenu()
    {
        IniciaStart();

        // Carga la escena 1
        SceneManager.LoadScene(0);
    }

    // Devuelve el tiempo de juego actual
    public float ObtieneTiempoRestante()
    {
        return tiempoNivel - tiempoJuego;
    }

    // Vevuelve el puntaje actual
    public int ObtienePuntos()
    {
        return puntos;
    }

    // Agregar puntos al puntaje total
    public void AgregaPuntos()
    {
        puntos += nivelAcumulado;

    }

    // Obtiene el valor de la bandera ganaste
    public bool ObtieneEstadoGameOver()
    {
        return gameOver;
    }

    // Cambia el valor de verdad del control colisionSuelo
    public void CambiaColisionSuelo(bool _colisionSuelo)
    {
        colisionSuelo = _colisionSuelo;
    }

    // Obtiene el valor de colisionSuelo
    public bool ObtieneColisionSuelo()
    {
        return colisionSuelo;
    }

    // // Cambia el valor de verdad del control colisionAgujero
    public void CambiaColisionAgujero(bool _colisionAgujero)
    {
        colisionAgujero = _colisionAgujero;
    }

    // Obtiene el valor de colisionSuelo
    public bool ObtieneColisionAgujero()
    {
        return colisionAgujero;
    }

    // Obtiene el tiempoSuperado
    public bool ObtieneTiempoSuperado()
    {
        return tiempoSuperado;
    }
}
