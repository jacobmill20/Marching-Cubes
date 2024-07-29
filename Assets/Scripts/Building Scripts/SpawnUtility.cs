using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUtility : MonoBehaviour, UtilityInterface
{
    public float spawnTime = 5f;

    public GameObject troop;

    public bool airTroop;
    public bool isEnemy;

    private GameObject spawn;
    private GameObject airSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        if (isEnemy)
            spawn = spawn = GameObject.FindGameObjectWithTag("Enemy Spawn Point");
        else
            spawn = GameObject.FindGameObjectWithTag("Friendly Spawn Point");

        if (isEnemy)
            airSpawn = GameObject.FindGameObjectWithTag("Enemy Air Spawn Point");
        else
            airSpawn = GameObject.FindGameObjectWithTag("Friendly Air Spawn Point");
    }

    private void Start()
    {
        if (isEnemy)
        {
            StartCoroutine("SpawnTroop");
        }
    }
    public void StartUtility()
    {
        StartCoroutine("SpawnTroop");
    }

    IEnumerator SpawnTroop()
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject newguy;
        if (airTroop)
        {
            newguy = Instantiate(troop, airSpawn.transform);
            if (isEnemy)
            {
                newguy.GetComponent<FlyingScript>().isEnemy = true;
                newguy.layer = 3;
                newguy.transform.parent = GameManager.instance.flyingEnemies;
            }
            else
                newguy.transform.parent = GameManager.instance.flyingFriendlies;
        }
        else
        {
            newguy = Instantiate(troop, spawn.transform);
            if (isEnemy)
            {
                newguy.GetComponent<GroundScript>().isEnemy = true;
                newguy.layer = 3;
                newguy.transform.parent = GameManager.instance.groundEnemies;
            }
            else
                newguy.transform.parent = GameManager.instance.groundFriendlies;
        }

        StartCoroutine("SpawnTroop");
    }

    public void DisableUtility()
    {
        this.enabled = false;
    }
}