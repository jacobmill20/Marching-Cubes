using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int numOfBases;

    public Text powerText;
    public int power;
    public Text cubitsText;
    public int cubits;

    public GameObject VictoryScreen;
    public GameObject DefeatScreen;

    public GameObject menuButton;

    public List<UtilityInterface> placeUtilities = new List<UtilityInterface>();

    public Transform groundEnemies { get; private set; }
    public Transform flyingEnemies { get; private set; }
    public Transform buildingEnemies { get; private set; }
    public Transform groundFriendlies { get; private set; }
    public Transform flyingFriendlies { get; private set; }
    public Transform buildingFriendlies { get; private set; }
    public Transform Ores { get; private set; }

    private bool paused;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            string name = child.name;
            if (name == "Enemy Ground Units")
                groundEnemies = child;
            if (name == "Enemy Flying Units")
                flyingEnemies = child;
            if (name == "Enemy Buildings")
                buildingEnemies = child;
            if (name == "Friendly Ground Units")
                groundFriendlies = child;
            if (name == "Friendly Flying Units")
                flyingFriendlies = child;
            if (name == "Friendly Buildings")
                buildingFriendlies = child;
            if (name == "Cubit Ores")
                Ores = child;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePower();
        UpdateCubits();
    }

    private void UpdatePower()
    {
        powerText.text = power.ToString();
    }

    private void UpdateCubits()
    {
        cubitsText.text = cubits.ToString();
    }

    //Called whenever a buidling is placed
    public void OnPlace()
    {
        foreach(UtilityInterface i in placeUtilities)
        {
            i.StartUtility();
        }
    }

    public void EnemyBaseDeath()
    {
        numOfBases--;
        if(numOfBases == 0)
        {
            GameOver(true);
        }
    }

    public void Pause()
    {
        if (paused)
        {
            menuButton.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        } else
        {
            menuButton.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
    }

    //Bellow is code to help troops find their targets
    public Transform[] getGroundEnemies()
    {
        Transform[] enemies = new Transform[groundEnemies.transform.childCount];

        for (int i = 0; i < groundEnemies.transform.childCount; i++)
        {
            enemies[i] = groundEnemies.transform.GetChild(i);
        }

        return enemies;
    }

    public Transform[] getFlyingEnemies()
    {
        Transform[] enemies = new Transform[flyingEnemies.transform.childCount];

        for (int i = 0; i < flyingEnemies.transform.childCount; i++)
        {
            enemies[i] = flyingEnemies.transform.GetChild(i);
        }

        return enemies;
    }

    public Transform[] getBuildingEnemies()
    {
        Transform[] enemies = new Transform[buildingEnemies.transform.childCount];

        for (int i = 0; i < buildingEnemies.transform.childCount; i++)
        {
            enemies[i] = buildingEnemies.transform.GetChild(i);
        }

        return enemies;
    }

    public Transform[] getGroundFriendlies()
    {
        Transform[] enemies = new Transform[groundFriendlies.transform.childCount];

        for (int i = 0; i < groundFriendlies.transform.childCount; i++)
        {
            enemies[i] = groundFriendlies.transform.GetChild(i);
        }

        return enemies;
    }

    public Transform[] getFlyingFriendlies()
    {
        Transform[] enemies = new Transform[flyingFriendlies.transform.childCount];

        for (int i = 0; i < flyingFriendlies.transform.childCount; i++)
        {
            enemies[i] = flyingFriendlies.transform.GetChild(i);
        }

        return enemies;
    }

    public Transform[] getBuildingFriendlies()
    {
        Transform[] enemies = new Transform[buildingFriendlies.transform.childCount];

        for (int i = 0; i < buildingFriendlies.transform.childCount; i++)
        {
            enemies[i] = buildingFriendlies.transform.GetChild(i);
        }
        return enemies;
    }

    public Transform[] getOres()
    {
        Transform[] cubitOres = new Transform[Ores.transform.childCount];

        for (int i = 0; i < Ores.transform.childCount; i++)
        {
            cubitOres[i] = Ores.transform.GetChild(i);
        }

        return cubitOres;
    }

    public Transform findClosestEnemy(Transform troop, bool canTargetGround, bool canTargetFlying, bool canTargetBuilding)
    {

        Transform target = null;


        if (canTargetGround && groundEnemies.transform.childCount > 0)
        {
            target = groundEnemies.transform.GetChild(0);
        }
        if (canTargetFlying && flyingEnemies.transform.childCount > 0)
        {
            target = flyingEnemies.transform.GetChild(0);
        }
        if (canTargetBuilding && buildingEnemies.transform.childCount > 0)
        {
            target = buildingEnemies.transform.GetChild(0);
        }

        if (target != null)
        {
            if (canTargetGround)
            {

                foreach (var a in getGroundEnemies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }
                }
            }

            if (canTargetFlying)
            {

                foreach (var a in getFlyingEnemies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }
                }
            }

            if (canTargetBuilding)
            {

                foreach (var a in getBuildingEnemies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }

                }
            }
        }

        return target;
    }

    public Transform findClosestFriendly(Transform troop, bool canTargetGround, bool canTargetFlying, bool canTargetBuilding)
    {

        Transform target = null;


        if (canTargetGround && groundFriendlies.transform.childCount > 0)
        {
            target = groundFriendlies.transform.GetChild(0);
        }
        if (canTargetFlying && flyingFriendlies.transform.childCount > 0)
        {
            target = flyingFriendlies.transform.GetChild(0);
        }
        if (canTargetBuilding && buildingFriendlies.transform.childCount > 0)
        {
            target = buildingFriendlies.transform.GetChild(0);
        }


        if (target != null)
        {
            if (canTargetGround)
            {

                foreach (var a in getGroundFriendlies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }
                }
            }

            if (canTargetFlying)
            {

                foreach (var a in getFlyingFriendlies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }
                }
            }

            if (canTargetBuilding)
            {

                foreach (var a in getBuildingFriendlies())
                {
                    if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                    {
                        target = a;
                    }
                }
            }
        }

        return target;
    }

    public Transform FindClosestOre(Transform troop)
    {
        Transform target = null;

        try
        {
            target = Ores.transform.GetChild(0);
        }
        catch { }

        if (target != null)
        {
            foreach (var a in getOres())
            {
                if (Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position))
                {
                    target = a;
                }
            }
        }

        return target;
    }

    public Transform FindClosestFriendlyDamagedBuilding(Transform troop)
    {

        Transform target = null;

        foreach (var a in getBuildingFriendlies())
        {
            //Checks for placeable object
            if (a.gameObject.TryGetComponent<PlaceableObject>(out PlaceableObject placeableObject))
            {
                //If target is null and a damaged building is found, set target to the building
                if (target == null && placeableObject.health < placeableObject.maxHealth)
                {
                    target = a;
                }

                //If target is not null and a closer damaged building is found, set target to the building
                if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && placeableObject.health < placeableObject.maxHealth)
                {
                    target = a;
                }
            }
            else
            {
                BaseScript bS = a.gameObject.GetComponent<BaseScript>();
                //If target is null and a damaged building is found, set target to the building
                if (target == null && bS.health < bS.maxHealth)
                {
                    target = a;
                }

                //If target is not null and a closer damaged building is found, set target to the building
                if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && bS.health < bS.maxHealth)
                {
                    target = a;
                }
            }
        }

        return target;
    }

    public Transform FindClosestEnemyDamagedBuilding(Transform troop)
    {
        Transform target = null;

        foreach (var a in getBuildingEnemies())
        {
            //Checks for placeable object
            if (a.gameObject.TryGetComponent<PlaceableObject>(out PlaceableObject placeableObject))
            {
                //If target is null and a damaged building is found, set target to the building
                if (target == null && placeableObject.health < placeableObject.maxHealth)
                {
                    target = a;
                }

                //If target is not null and a closer damaged building is found, set target to the building
                if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && placeableObject.health < placeableObject.maxHealth)
                {
                    target = a;
                }
            }
            else
            {
                BaseScript bS = a.gameObject.GetComponent<BaseScript>();
                //If target is null and a damaged building is found, set target to the building
                if (target == null && bS.health < bS.maxHealth)
                {
                    target = a;
                }

                //If target is not null and a closer damaged building is found, set target to the building
                if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && bS.health < bS.maxHealth)
                {
                    target = a;
                }
            }
        }

        return target;
    }

    public Transform FindClosestFriendlyDamagedTroop(Transform troop)
    {
        Transform target = null;

        foreach (var a in getGroundFriendlies())
        {
            
            ISpawnable info = a.gameObject.GetComponent<ISpawnable>();

            //If target is null and a damaged friendly is found, set target to the building
            if (target == null && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }

            //If target is not null and a closer damaged friendly is found, set target to the building
            if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }
        }
        foreach (var a in getFlyingFriendlies())
        {
            if (a == troop)
                continue;

            ISpawnable info = a.gameObject.GetComponent<ISpawnable>();

            //If target is null and a damaged friendly is found, set target to the building
            if (target == null && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }

            //If target is not null and a closer damaged friendly is found, set target to the building
            if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }
        }

        return target;
    }

    public Transform FindClosestEnemyDamagedTroop(Transform troop)
    {
        Transform target = null;

        //Put all of the ground and flying enemies into a single array because I really cant be bothered
        Transform[] ground = getGroundEnemies();
        Transform[] flying = getFlyingEnemies();
        Transform[] groundAndFlying = new Transform[ground.Length + flying.Length];
        ground.CopyTo(groundAndFlying, 0);
        flying.CopyTo(groundAndFlying, ground.Length);

        foreach (var a in groundAndFlying)
        {
            ISpawnable info = a.gameObject.GetComponent<ISpawnable>();

            //If target is null and a damaged Enemy is found, set target to the building
            if (target == null && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }

            //If target is not null and a closer damaged Enemy is found, set target to the building
            if (target != null && Vector3.Distance(troop.position, target.position) > Vector3.Distance(troop.position, a.position) && info.GetHealth() < info.GetMaxHealth())
            {
                target = a;
            }
        }

        return target;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void GameOver(bool win)
    {
        StartCoroutine(GameOverCR(win));
    }

    IEnumerator GameOverCR(bool win)
    {       
        while(Time.timeScale > 0f)
        {
            yield return new WaitForSeconds(0.1f);
            if (Time.timeScale - 0.1f > 0f)
            {
                Time.timeScale -= 0.1f;
            } else
            {
                Time.timeScale = 0f;
            }
        }

        if (win)
        {
            VictoryScreen.SetActive(true);
        } else
        {
            DefeatScreen.SetActive(true);
        }
    }
}
