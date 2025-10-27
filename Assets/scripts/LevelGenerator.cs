using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Asignar el prefab de plataforma desde el Inspector
    public GameObject platformPrefab;

    // Altura donde se generará la siguiente plataforma
    // Se inicializa en 0, pero se ajustará en el Start
    private float nextSpawnY = 0f;

    // Distancia vertical entre plataformas
    [SerializeField] private float verticalGap = 3f;

    // La coordenada Y mínima que la cámara sigue
    // Ya no es necesaria, usaremos la posición fija de la cámara
    // private float cameraYMin = 0f; 

    // Velocidad a la que el nivel se mueve hacia arriba (creando la ilusión de caída)
    public float scrollSpeed = 2f; 
    
    // Lista para almacenar y reciclar plataformas ya generadas
    private System.Collections.Generic.List<GameObject> activePlatforms = new System.Collections.Generic.List<GameObject>();
    
    // Punto donde las plataformas viejas deben ser recicladas (ej: 15 unidades por encima de la cámara)
    private float recyclePointY; 


    void Start()
    {
        // Calcular el punto Y donde debemos reciclar (parte superior de la pantalla + un margen)
        // Esto asume que el jugador/cámara está en Y=0
        recyclePointY = Camera.main.transform.position.y + Camera.main.orthographicSize + 5f; 

        // Generar algunas plataformas iniciales para empezar el juego
        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }
    }

    // Única y correcta función Update
    void Update()
    {
        // 1. Mover todo el objeto generador (que contiene las plataformas) hacia arriba
        // Esto crea la ilusión de que el jugador está cayendo
        transform.position += Vector3.up * scrollSpeed * Time.deltaTime;

        // 2. Comprobar si necesitamos generar MÁS plataformas ABAJO de la vista
        // La generación debe ocurrir cuando la posición de la última plataforma generada (nextSpawnY)
        // pasa el límite inferior de la cámara (por ejemplo, Y = -15)
        float cameraYMax = Camera.main.transform.position.y + Camera.main.orthographicSize;
        
        // Generamos cuando el punto de generación (que se mueve con el generador) llega a un punto alto.
        // Usaremos un valor fijo bajo, ya que el generador se está moviendo hacia arriba.
        // Si el generador sube a 10, y generamos a -5, la plataforma aparece en 5.
        
        // En lugar de usar nextSpawnY (que está fijo y se arrastra hacia arriba), 
        // comprobamos la posición de la última plataforma que generamos.

        // Si no hay plataformas o si la última plataforma generada está demasiado lejos del borde inferior
        if (activePlatforms.Count == 0 || activePlatforms[activePlatforms.Count - 1].transform.position.y < cameraYMax - 5f) 
        {
            SpawnPlatform();
        }

        // 3. Reciclaje de Plataformas (Cuando pasan por encima de la cámara)
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.y > recyclePointY)
        {
            RecyclePlatform(activePlatforms[0]);
        }
    }

    void SpawnPlatform()
    {
        // 1. Calcular la posición horizontal aleatoria
        float randomX = Random.Range(-5f, 5f);

        // 2. Calcular la posición vertical
        //nextSpawnY es la Y donde la plataforma debe aparecer ABAJO de la vista
        
        // La posición vertical de la nueva plataforma debe ser DEBAJO de la anterior.
        nextSpawnY -= verticalGap + Random.Range(0f, 2f); 
        
        // 3. Crear el objeto en la posición calculada (relativa al generador)
        GameObject newPlatform = Instantiate(platformPrefab, transform); 
        
        // Establecer la posición relativa al padre (el generador)
        newPlatform.transform.localPosition = new Vector2(randomX, nextSpawnY);

        // 4. Añadir a la lista para rastreo
        activePlatforms.Add(newPlatform);
    }
    
    void RecyclePlatform(GameObject platformToRecycle)
    {
        // 1. Eliminar la plataforma de la parte superior de la lista
        activePlatforms.Remove(platformToRecycle);
        
        // 2. Mover la plataforma a una nueva posición muy por debajo de la última plataforma generada
        // Esto la "reinicia" en la parte inferior para ser arrastrada hacia arriba nuevamente.
        nextSpawnY -= verticalGap + Random.Range(0f, 2f); 
        
        platformToRecycle.transform.localPosition = new Vector2(Random.Range(-5f, 5f), nextSpawnY);
        
        // 3. Re-añadir al final de la lista
        activePlatforms.Add(platformToRecycle);
    }
}