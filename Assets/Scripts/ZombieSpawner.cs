using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ZombieSpawner : MonoBehaviour
{
    private UIManager uIManager;
    public Zombie zombiePrefab;
    public Zombie zombiePrefab1;
    public Zombie zombiePrefab2;
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints;
    private GameManager gameManager;

    private readonly List<Zombie> zombies = new List<Zombie>();
    private readonly List<Zombie> zombiePool = new List<Zombie>();

    private int wave = 0;

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameController");
        gameManager = findGo.GetComponent<GameManager>();
        uIManager = gameManager.uIManager;

    }
    private void Update()
    {
        if(zombies.Count == 0)
        {
            SpawnWave();
        }
    }
    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for(int i = 0; i < count; ++i)
        {
            CreateZombie();
        }
        uIManager.SetWaveText(wave, zombies.Count);
    }
    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        int zombieType = Random.Range(0, 3);

        ZombieData data = zombieDatas[zombieType]; 

        Zombie zombie;

        if(zombiePool.Count == 0)
        {
            Zombie selectedPrefab = zombiePrefab;
            if (zombieType == 1) selectedPrefab = zombiePrefab1;
            else if (zombieType == 2) selectedPrefab = zombiePrefab2;

            zombie = Instantiate(selectedPrefab);
            zombie.gameObject.SetActive(false);
            zombiePool.Add(zombie);
            
            zombie.OnDead.AddListener(() => zombies.Remove(zombie));
            zombie.OnDead.AddListener(() => StartCoroutine(CoReturnToPoolZombie(zombie)));        
        }

        zombie = zombiePool[0];
        zombie.Setup(data);
        zombie.transform.position = point.position;
        zombie.transform.rotation = point.rotation;
        zombie.gameObject.SetActive(true);        
        zombies.Add(zombie);
        zombiePool.Remove(zombie);
    }
    private IEnumerator CoReturnToPoolZombie(Zombie zombie)
    {
        yield return new WaitForSeconds(2f);

        if (zombie != null && zombie.gameObject != null)
        {
            zombie.gameObject.SetActive(false);
        }
    }

    public void OnEndGame()
    {
        for(int i = zombies.Count - 1; i >= 0; --i)
        {
            zombies[i].Death();
        }
    }
}
