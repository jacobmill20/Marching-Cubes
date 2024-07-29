using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTurretScript : MonoBehaviour, UtilityInterface
{
    public bool isEnemy = false;

    public LineRenderer line;
    public Transform healPoint;
    public int healing;
    public float healingRate;

    private Transform target;

    private float checkTimer = 0.5f;

    public void StartUtility()
    {
        line.SetPosition(0, healPoint.position);
        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            if (isEnemy)
            {
                target = GameManager.instance.FindClosestEnemyDamagedBuilding(gameObject.transform);
            }
            else
            {
                target = GameManager.instance.FindClosestFriendlyDamagedBuilding(gameObject.transform);
            }
            checkTimer = 0.5f;
        }

        if(target != null && gameObject.GetComponent<PlaceableObject>().Placed)
        {
            line.SetPosition(1, target.position);
            line.enabled = true;
        } else
        {
            line.enabled = false;
        }
    }

    IEnumerator Heal()
    {
        if (target != null)
        {
            //If target has a placeable object, add health. If not, Check for base script.
            if (target.gameObject.TryGetComponent(out PlaceableObject pO))
            {
                //If current health + healing is still less than the max health, then heal
                if (pO.health + healing <= pO.maxHealth)
                {
                    pO.health += healing;
                }
                else
                {
                    //If healing would bring object obove max health, set health to max
                    pO.health = pO.maxHealth;
                }
            }
            else
            {
                if (target.gameObject.TryGetComponent(out BaseScript bS))
                {
                    //If current health + healing is still less than the max health, then heal
                    if (bS.health + healing <= bS.maxHealth)
                    {
                        bS.health += healing;
                    }
                    else
                    {
                        //If healing would bring object obove max health, set health to max
                        bS.health = bS.maxHealth;
                    }
                }
            }
        }
        yield return new WaitForSeconds(healingRate);
        StartCoroutine(Heal());
    }

    public void DisableUtility()
    {
        this.enabled = false;
    }
}
