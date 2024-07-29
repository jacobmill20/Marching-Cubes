using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public bool isEnemy;
    public GameObject groundRally;
    public GameObject airRally;
    public int health;

    public int maxHealth { get; private set; }

    private Transform closestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AddPower");
        StartCoroutine("FindEnemy");

        //Set max health
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(closestEnemy && !isEnemy) {
            if (Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position) < 30f)
            {
                groundRally.transform.localPosition = new Vector3(0f, 0f, 2f);
                airRally.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else if (Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position) >= 30f)
            {
                groundRally.transform.localPosition = new Vector3(0f, 0f, 19.4f);
                airRally.transform.localPosition = new Vector3(0f, 6f, 10.88f);
            }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance.enabled)
        {
            if (isEnemy)
            {
                GameManager.instance.EnemyBaseDeath();
            }
            else
            {
                GameManager.instance.GameOver(false);
            }
        }
    }

    IEnumerator AddPower()
    {
        yield return new WaitForSeconds(1);
        if(!isEnemy)
            GameManager.instance.power += 1;
        StartCoroutine("AddPower");
    }

    IEnumerator FindEnemy()
    {
        if (isEnemy)
        {
            closestEnemy = GameManager.instance.findClosestFriendly(gameObject.transform, true, true, true);
        }
        else
        {
            closestEnemy = GameManager.instance.findClosestEnemy(gameObject.transform, true, true, true);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("FindEnemy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            ProjectileScript projectileData = collision.gameObject.GetComponent<ProjectileScript>();
            health -= projectileData.damage;
        }

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
