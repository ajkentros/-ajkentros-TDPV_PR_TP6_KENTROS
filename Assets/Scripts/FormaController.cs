using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class FormaController : MonoBehaviour
{
    [SerializeField] private float velocidadCaida = 5f;         // Referencia a la velocidad de caída de la forma
    [SerializeField] private float velocidadRotacion = 50f;     // Referencia a la velocidad de rotación de la forma.
    [SerializeField] private float velocidadMovimiento = 2f;    // Referencia a la velocidad de movimiento lateral de la forma
    
    private bool estaCayendo = false;       // Referencia a la variable que controla si está cayendo la forma

    private int nivel;      // Referencia al nivel de juego

    private Vector3 posicionOriginal;           // Referencia posición original de la forma
    private Quaternion rotacionOriginal;        // Referencia rotación original de la forma
    private Vector3 escalaOriginal;             // Referencia escala original de la forma


    private void Awake()
    {
        // Obtener el nivel actual del GameManager
        nivel = GameManager.gameManager.ObtieneNivelActual();

        // Almacenar la posición, rotación y escala originales del prefab cuando el nivel es 1
        if (nivel == 1)
        {
            posicionOriginal = transform.position;
            rotacionOriginal = transform.rotation;
            escalaOriginal = transform.localScale;
        }
    }
    void Start()
    {

        // Suscríbete al evento OnActualizaNivel
        GameManager.gameManager.OnActualizaNivel.AddListener(CambiaForma);


    }

    void Update()
    {
        // Llama al método de rotación
        ActualizaRotacionForma();

        // LLama al método para el desplazamiento en x
        ActualizaDesplazamientoForma();

        // Llama al método para la caída de la forma en y
        ActualizaCaidaForma();
        
    }

   
    // Gestiona la rotación de la forma
    private void ActualizaRotacionForma()
    {
        // Si se presiona la tecla X y simultáneamente se presiona la flecha arriba o abajo, se rota en el eje X
        if (Input.GetKey(KeyCode.X) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            Debug.Log("x");
            float rotacionX = Input.GetAxis("Vertical") * velocidadRotacion * Time.deltaTime;
            transform.Rotate(Vector3.right, rotacionX);
        }
        else // Si se presiona la tecla X y simultáneamente se presiona la flecha arriba o abajo, se rota en el eje X
        if (Input.GetKey(KeyCode.Y) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            Debug.Log("y");
            float rotacionY = Input.GetAxis("Vertical") * velocidadRotacion * Time.deltaTime;
            transform.Rotate(Vector3.up, rotacionY);
        }
        else// Si se presiona la tecla X y simultáneamente se presiona la flecha arriba o abajo, se rota en el eje X
        if (Input.GetKey(KeyCode.Z) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            Debug.Log("z");
            float rotacionZ = Input.GetAxis("Vertical") * velocidadRotacion * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotacionZ);
        }
    }

    // Gestiona el desplazamiento en x de la forma
    private void ActualizaDesplazamientoForma()
    {
        // Si se presiona la flecha izquierda o derecha, se mueve lateralmente en el eje X
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float movimientoX = Input.GetAxis("Horizontal") * velocidadMovimiento * Time.deltaTime;
            transform.Translate(Vector3.right * movimientoX);
        }

    }

    // Gestiona la caída de la forma
    private void ActualizaCaidaForma()
    {
        // Si clic en space => la forma cae
        if (Input.GetKeyDown(KeyCode.Space) && !estaCayendo)
        {
            estaCayendo = true;
        }

        // Si la forma está habilitada a caer =>  movimiento de caída vertical
        if (estaCayendo)
        {
            // Calcula la posición de la forma
            Vector3 newPosition = transform.position + Time.deltaTime * velocidadCaida * Vector3.down;

            // Mueve la forma hacia la nueva posición (movimiento hacia abajo)
            transform.position = newPosition;
        }

    }
    // Gestiona las colisiones
    void OnTriggerEnter(Collider collision)
    {
        
        // Si la forma colisiona con el Suelo => se gana un punto (en función del nivel) + se pasa al siguiente Nivel + nueva Forma
        if (collision.gameObject.CompareTag("Suelo"))
        {

            //Debug.Log("¡Coincidencia exacta!");

            // Pausa juego
            GameManager.gameManager.PausarJuego();

            // Agrega puntos por colisionar con el suelo
            GameManager.gameManager.AgregaPuntos();

            // Activa el evento de colisión con el suelo
            GameManager.gameManager.CambiaColisionSuelo(true);

            // Cambia el flag estaCayendo
            estaCayendo = false;
           
        }

        // Si la forma colisiona con alguna parte de Agujero => se repite el nivel
        if (collision.gameObject.CompareTag("Agujero"))
        {
            
            //Debug.Log("¡No hay coincidencia!");

            // Pausa juego
            GameManager.gameManager.PausarJuego();

            // Repite nivel
            GameManager.gameManager.CambiaColisionAgujero(true);

            // Detiene la caída
            estaCayendo = false;
            


        }
    }

    private void CambiaForma()
    {
        // Actualiza el valor de nivel desde el Game Manager
        nivel = GameManager.gameManager.ObtieneNivelActual();

        // Define variables locales de la posicion y escala a ajustar de la forma
        float ajustePosicionX;
        float ajustePosicionY;
        float ajustePosicionZ;
        float ajusteEscalaX;
        float ajusteEscalaY;
        float ajusteEscalaZ;

        switch (nivel)
        {
            case 1: // estado original del prefabForma
                // Posición
                transform.position = posicionOriginal;
                // Rotación
                transform.rotation = rotacionOriginal;
                // Escala
                transform.localScale = escalaOriginal;
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                // Posición
                ajustePosicionX = Random.Range(-1.5f, 1.5f) + posicionOriginal.x;
                ajustePosicionY = posicionOriginal.y;
                ajustePosicionZ = posicionOriginal.z;
                transform.position = new Vector3(ajustePosicionX, ajustePosicionY, ajustePosicionZ);
                // Rotación
                transform.rotation = rotacionOriginal;
                // Escala
                ajusteEscalaX = 4.722f * (nivel - 1) + escalaOriginal.x;
                ajusteEscalaY = escalaOriginal.y;
                ajusteEscalaZ = escalaOriginal.z;
                transform.localScale = new Vector3(ajusteEscalaX, ajusteEscalaY, ajusteEscalaZ);
                break;
            
            
        }
       
        
    }
}
