using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningScript : MonoBehaviour
{
    public float maxSpawnTime;
    public float minSpawnTime;
    
    public GameObject[] troops;
    
    public Transform[] GroundSpawns;
    public Transform[] AirSpawns;
    public GameObject[] Turrets;

    private int spawns;
    private int troopUnlocks = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");

        for(int i = 0; i < Turrets.Length; i++)
        {
            Turrets[i].GetComponent<PlaceableObject>().ManualPlace();
        }
    }

    IEnumerator Spawn()
    {
        //Determine Time between spawns
        float waitTime;
        if (spawns <= 120) {
           waitTime = maxSpawnTime - (spawns/(120/(maxSpawnTime-minSpawnTime)));
        } else
        {
            waitTime = minSpawnTime;
        }

        yield return new WaitForSeconds(waitTime);


        //Check troop unlocks
        if(spawns % 20 == 0 && troopUnlocks < 6)
        {
            troopUnlocks++;
        }

        //Randomly Choose Troop and spawn
        int troopNum = Random.Range(0, troopUnlocks);

        //Spawn Troop
        ISpawnable troopInfo = troops[troopNum].GetComponent<ISpawnable>();
        GameObject newguy;
        if (!troopInfo.GetIsFlying())
        {
            newguy = Instantiate(troops[troopNum], GroundSpawns[Random.Range(0, GroundSpawns.Length)]);
            newguy.transform.parent = GameManager.instance.groundEnemies;
        } else
        {
            newguy = Instantiate(troops[troopNum], AirSpawns[Random.Range(0, AirSpawns.Length)]);
            newguy.transform.parent = GameManager.instance.flyingEnemies;
        }
        newguy.transform.Rotate(new Vector3(0f, 180f, 0f));
        newguy.GetComponent<ISpawnable>().SetIsEnemy();
        newguy.layer = 3;
        spawns++;

        StartCoroutine("Spawn");
    }
}
