using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Intervals")]
    public float enemySpawnInterval; // interval between spawning ships 
    public float waveInterval; // same for wave, intervall between 2spawning whole waves;

    private int currentWave;
    private int shipOneID = 0;
    private int shipTwoID = 0;
    private int shipThreeID = 0;

    [Header("Prefab to spawn:")]
    public GameObject shipOne;
    public GameObject shipTwo;
    public GameObject shipThree;

    [Header("Formations")]
    public Formation enemyOneFormation; // formation and number of total spawnd ships
    public Formation enemyTwoFormation;
    public Formation enemyThreeFormation;

    [System.Serializable]
    public class Wave
    {

        public int shipOneAmmount; // number of active number of ships in secene
        public int shipTwoAmmount;
        public int shipThreeAmmount;

        public GameObject[] pathPrefabs;       

    }

    [Header("SPREAD")]
    public bool activateSpread;

    [Header("Weaves")]
    public List<Wave> waveList = new List<Wave>();

    List<Path> activePathList = new List<Path>();


    public List<GameObject> spawnedEnemies = new List<GameObject>();

    

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartSpawn", 3f);
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waveList.Count)
        {
            for (int i = 0; i < waveList[currentWave].pathPrefabs.Length; i++)
            {
                GameObject newPathObj = Instantiate(waveList[currentWave].pathPrefabs[i], transform.position, transform.rotation) as GameObject;
                Path newPath = newPathObj.GetComponent<Path>();
                activePathList.Add(newPath);
            }

            //shipOne spawn..
            for (int i = 0; i < waveList[currentWave].shipOneAmmount; i++)
            {
                GameObject newShipOne = Instantiate(shipOne, transform.position, Quaternion.identity) as GameObject;
                NewEnemyBeh shipOneBehaviour = newShipOne.GetComponent<NewEnemyBeh>();

                shipOneBehaviour.SpawnSetup(activePathList[PathPingPong()], shipOneID, enemyOneFormation);
                shipOneID++;

                spawnedEnemies.Add(newShipOne);

                // report to game manager spawned enemy to count number of remaining enemies in scene
                GameManager.instance.AddEnemy();
                print("added");

                //wait for next enemy spawn in scene
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            //shipTwo spawn..
            for (int i = 0; i < waveList[currentWave].shipTwoAmmount; i++)
            {
                GameObject newShipTwo = Instantiate(shipTwo, transform.position, Quaternion.identity) as GameObject;
                NewEnemyBeh shipTwoBehaviour = newShipTwo.GetComponent<NewEnemyBeh>();

                shipTwoBehaviour.SpawnSetup(activePathList[PathPingPong()], shipTwoID, enemyTwoFormation);
                shipTwoID++;

                spawnedEnemies.Add(newShipTwo);


                // report to game manager spawned enemy to count number of remaining enemies in scene
                GameManager.instance.AddEnemy();
                print("added");
                //wait for next enemy spawn in scene
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            //shipThree spawn..
            for (int i = 0; i < waveList[currentWave].shipThreeAmmount; i++)
            {
                GameObject newShipThree = Instantiate(shipThree, transform.position, Quaternion.identity) as GameObject;
                NewEnemyBeh shipThreeBehaviour = newShipThree.GetComponent<NewEnemyBeh>();

                shipThreeBehaviour.SpawnSetup(activePathList[PathPingPong()], shipThreeID, enemyThreeFormation);
                shipThreeID++;

                spawnedEnemies.Add(newShipThree);

                // report to game manager spawned enemy to count number of remaining enemies in scene
                GameManager.instance.AddEnemy();
                print("added");
                //wait for next enemy spawn in scene
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            yield return new WaitForSeconds(waveInterval);

            currentWave++;

            foreach(Path p in activePathList)
            {
                Destroy(p.gameObject);
            }

            activePathList.Clear();
        }
        
        Invoke("CheckEnemyState", 1f);
    }
    
    void CheckEnemyState()
    {
        bool inFormation = false;
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if(spawnedEnemies[i].GetComponent<NewEnemyBeh>().enemyStates != NewEnemyBeh.EnemyStates.IDLE)
            {
                inFormation = false;
                Invoke("CheckEnemyState", 1f);
                break;
            }
        }

        inFormation = true;

        if (inFormation)
        {
            //spread ACTIVE
            if(activateSpread)
            {
                StartCoroutine(enemyOneFormation.ActivateSpread());
                StartCoroutine(enemyTwoFormation.ActivateSpread());
                StartCoroutine(enemyThreeFormation.ActivateSpread());
                
            }
            CancelInvoke("CheckEnemyState");
        }
    }

    void StartSpawn()
    {
        GameManager.instance.enemyAmmount = 0;
        StartCoroutine(SpawnWaves());
        CancelInvoke("StartSpawn");
    }


    int PathPingPong()
    {

        return (shipOneID + shipTwoID + shipThreeID) % activePathList.Count; 
    }
}
