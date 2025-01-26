using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Camera _mainCamera;

    [Header("Configuración de Spawn")]

    public GameObject pulpoPre;
    public GameObject pezPre;
    public GameObject pezLuzPre;
    public GameObject pezEspadaPre;

    public GameObject pulpoHolder;
    public GameObject pezHolder;
    public GameObject pezLuzHolder;
    public GameObject pezEspadaHolder;

    public Transform[] spawnPointsLeft; // Puntos donde pueden aparecer los enemigos
    public Transform[] spawnPointsRight; // Puntos donde pueden aparecer los enemigos
    public Transform[] spawnPointBottom; // Puntos donde pueden aparecer los enemigos
    public Transform[] spawnPointsCorners; // Puntos donde pueden aparecer los enemigos

    [SerializeField] private List<Transform> _pulpoSpawners;
    [SerializeField] private List<Transform> _pecesSpawners;
    [SerializeField] private List<Transform> _pezLuzSpawners;
    [SerializeField] private List<Transform> _pezEspadaSpawners;

    public int maxEnemies = 10; // Máximo número de enemigos permitidos
    public float spawnInterval; // Intervalo entre spawns en segundos
    public float respawnDelay; // Tiempo de espera antes de spawnear de nuevo tras liberar espacio

    private int currentEnemyCount = 0; // Número actual de enemigos en escena
    private bool canSpawn = true; // Controla si se puede spawnear nuevos enemigos

    public List <GameObject>Pulpos;
    public List <GameObject>Peces;
    public List<GameObject> PezLuz;
    public List<GameObject> PezEspada;

    private void Awake()
    {
        _pulpoSpawners = new List<Transform>(spawnPointsLeft);
        for (int i = 0; i < spawnPointsRight.Length; i++)
        {
            _pulpoSpawners.Add(spawnPointsRight[i]);
        }

        _pecesSpawners = new List<Transform>(_pulpoSpawners);

        _pezLuzSpawners = new List<Transform>(spawnPointsCorners);

        _pezEspadaSpawners = new List<Transform>(_pulpoSpawners);
        for (int i = 0; i < spawnPointBottom.Length; i++)
        {
            _pezEspadaSpawners.Add(spawnPointBottom[i]);
        }

        _mainCamera = FindAnyObjectByType<Camera>();
    }

    private void Update()
    {
        this.gameObject.transform.position = _mainCamera.transform.position;
        this.gameObject.transform.rotation = _mainCamera.transform.rotation;

        if(Input.GetMouseButtonDown(2)) 
        {
            Debug.Log("PRESS");
            StartCoroutine(SpawnFishLightRoutine());
            StartCoroutine(SpawnFishRoutine());
            StartCoroutine(SpawnPulpoRoutine());
            
        }
    }

    private IEnumerator SpawnPulpoRoutine()
    {
            if (canSpawn && (currentEnemyCount < maxEnemies))
            {
                SpawnPulpo();
            }
            yield return new WaitForSeconds(spawnInterval);
    }

    private void SpawnPulpo()
    {
        // Si ya alcanzamos el límite de enemigos, no hacemos nada
        if (currentEnemyCount >= maxEnemies)
        {
            return;
        }

        // Selecciona un punto de spawn aleatorio
        int randomIndex = Random.Range(0, _pulpoSpawners.Count);
        Transform spawnPoint = _pulpoSpawners[randomIndex];

        // Busca un enemigo inactivo para reactivarlo si existe
        GameObject enemy = FindInactivePulpo(spawnPoint);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);
            currentEnemyCount++;
        }
    }

    private GameObject FindInactivePulpo(Transform spawn)
    {
        for (int i = 0; i < Pulpos.Count; i++)
        {
            if (!Pulpos[i].activeInHierarchy)
            {
                return Pulpos[i];
            }
        }

        GameObject pulpos = Instantiate(pulpoPre, spawn.position, Quaternion.identity);
        pulpos.transform.parent = pulpoHolder.transform;
        Pulpos.Add(pulpos);
        return pulpos;
    }

    private IEnumerator SpawnFishRoutine()
    {
        if (canSpawn && (currentEnemyCount < maxEnemies))
        {
            SpawnFish();
        }
        yield return new WaitForSeconds(spawnInterval);
    }

    private void SpawnFish()
    {
        // Si ya alcanzamos el límite de enemigos, no hacemos nada
        if (currentEnemyCount >= maxEnemies)
        {
            return;
        }

        // Selecciona un punto de spawn aleatorio
        int randomIndex = Random.Range(0, _pecesSpawners.Count);
        Transform spawnPoint = _pecesSpawners[randomIndex];

        // Busca un enemigo inactivo para reactivarlo si existe
        GameObject enemy = FindInactiveFish(spawnPoint);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);
            currentEnemyCount++;
        }
    }

    private GameObject FindInactiveFish(Transform spawn)
    {
        for (int i = 0; i < Peces.Count; i++)
        {
            if (!Peces[i].activeInHierarchy)
            {
                return Peces[i];
            }
        }

        GameObject pez = Instantiate(pezPre, spawn.position, Quaternion.identity);
        pez.transform.parent = pezHolder.transform;
        Peces.Add(pez);
        return pez;
    }

    private IEnumerator SpawnFishLightRoutine()
    {
        if (canSpawn && (currentEnemyCount < maxEnemies))
        {
            SpawnLightFish();
        }
        yield return new WaitForSeconds(spawnInterval);
    }

    private void SpawnLightFish()
    {
        // Si ya alcanzamos el límite de enemigos, no hacemos nada
        if (currentEnemyCount >= maxEnemies)
        {
            return;
        }

        // Selecciona un punto de spawn aleatorio
        int randomIndex = Random.Range(0, _pezLuzSpawners.Count);
        Transform spawnPoint = _pezLuzSpawners[randomIndex];

        // Busca un enemigo inactivo para reactivarlo si existe
        GameObject enemy = FindInactiveFishLight(spawnPoint);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);
            currentEnemyCount++;
        }
    }

    private GameObject FindInactiveFishLight(Transform spawn)
    {
        for (int i = 0; i < PezLuz.Count; i++)
        {
            if (!PezLuz[i].activeInHierarchy)
            {
                return PezLuz[i];
            }
        }

        GameObject pezLuz = Instantiate(pezLuzPre, spawn.position, Quaternion.identity);
        pezLuz.transform.parent = pezLuzHolder.transform;
        Peces.Add(pezLuz);
        return pezLuz;
    }


    private IEnumerator SpawnFishSwordRoutine()
    {
        if (canSpawn && (currentEnemyCount < maxEnemies))
        {
            SpawnSwordFish();
        }
        yield return new WaitForSeconds(spawnInterval);
    }

    private void SpawnSwordFish()
    {
        // Si ya alcanzamos el límite de enemigos, no hacemos nada
        if (currentEnemyCount >= maxEnemies)
        {
            return;
        }

        // Selecciona un punto de spawn aleatorio
        int randomIndex = Random.Range(0, _pezEspadaSpawners.Count);
        Transform spawnPoint = _pezEspadaSpawners[randomIndex];

        // Busca un enemigo inactivo para reactivarlo si existe
        GameObject enemy = FindInactiveFishSword(spawnPoint);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);
            currentEnemyCount++;
        }
    }

    private GameObject FindInactiveFishSword(Transform spawn)
    {
        for (int i = 0; i < PezLuz.Count; i++)
        {
            if (!PezLuz[i].activeInHierarchy)
            {
                return PezLuz[i];
            }
        }

        GameObject pezEspada = Instantiate(pezEspadaPre, spawn.position, Quaternion.identity);
        pezEspada.transform.parent = pezEspadaHolder.transform;
        Peces.Add(pezEspada);
        return pezEspada;
    }

    // Método para reducir el contador cuando un enemigo se "desactiva"
    public void OnEnemyDeactivated()
    {
        currentEnemyCount--;
        StartCoroutine(WaitToAllowSpawn());
    }

    private IEnumerator WaitToAllowSpawn()
    {
        canSpawn = false; // Evita que se spawneen nuevos enemigos inmediatamente
        yield return new WaitForSeconds(respawnDelay);
        canSpawn = true;
    }
}
