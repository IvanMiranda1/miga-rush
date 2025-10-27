using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] hace que esta variable aparezca en el Inspector para que puedas ajustarla.
    [SerializeField] private float moveSpeed = 7f;

    // No necesitamos Rigidbody2D en el código si el tipo de cuerpo es Kinematic.

    void Update()
    {
        // 1. Obtiene la entrada horizontal. Será 1, -1, o 0.
        float horizontalInput = Input.GetAxis("Horizontal");

        // 2. Calcula la distancia que debe moverse el personaje.
        // Distancia = Velocidad * Tiempo
        float displacementX = horizontalInput * moveSpeed * Time.deltaTime;

        // 3. Aplica el movimiento directamente al Transform (movimiento cinemático).
        // Solo modificamos la posición X; la Y y Z se mantienen fijas.
        transform.position = new Vector3(
            transform.position.x + displacementX, // Nueva posición X
            transform.position.y,                 // Posición Y se mantiene (no cae)
            transform.position.z                  // Posición Z se mantiene
        );

        // --- Limitación de Bordes (Opcional, pero recomendado) ---
        // Esto evita que el personaje salga de la pantalla.

        // Calcula la mitad del ancho visible de la cámara.
        float camHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;

        // Define un margen (ajusta 0.5f al tamaño de tu personaje).
        float margin = 0.5f;
        float minX = -camHalfWidth + margin;
        float maxX = camHalfWidth - margin;

        // Limita la posición X.
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);

        // Asigna la posición limitada.
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
    
    void OnTriggerEnter2D(Collider2D other) // <- El parámetro cambia a Collider2D
    {
        // Comprobar si el objeto colisionado tiene la etiqueta "Hazard"
        // 'other.gameObject' es el objeto que tocó el jugador (la plataforma)
        if (other.gameObject.CompareTag("Hazzard")) 
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("¡Juego Terminado! El personaje tocó un bloque.");
        
        // 1. Detener el tiempo del juego (pausar todo movimiento)
        Time.timeScale = 0f;

        // 2. Aquí es donde pondrías la lógica para:
        //    - Mostrar una pantalla de Game Over (UI)
        //    - Detener la música
        
        // Opcional: Destruir el jugador (si no se va a reiniciar la escena)
        // Destroy(gameObject); 
    }
}