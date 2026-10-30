using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ZombieSpawner : MonoBehaviour
{
    // private UIManager uIManager;
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
        // uIManager = gameManager.uIManager;

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
        // uIManager.SetWaveText(wave, zombies.Count);
    }
    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];

        Zombie zombie;
        if(zombiePool.Count == 0)
        {
            zombie = Instantiate(zombiePrefab);
            zombie.gameObject.SetActive(false);
            zombiePool.Add(zombie);
            

            zombie.OnDead.AddListener(() => zombies.Remove(zombie));
            zombie.OnDead.AddListener(() => StartCoroutine(CoReturnToPoolZombie(zombie)));        
            // zombie.OnDead.AddListener(() => uIManager.SetWaveText(wave, zombies.Count));
            // zombie.OnDead.AddListener(() => gameManager.AddScore(50));
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
        yield return new WaitForSeconds(5f);
        zombie.gameObject.SetActive(false);
        zombiePool.Add(zombie);
    }

    public void OnEndGame()
    {
        for(int i = zombies.Count - 1; i >= 0; --i)
        {
            zombies[i].Die();
        }
    }
}
