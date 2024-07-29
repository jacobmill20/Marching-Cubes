using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : MonoBehaviour, ISpawnable
{
    public float speed;
    public float turnSpeed;
    public float stopDistance;
    public float range;
    public int health;
    public int healing;
    public float healingRate;

    public int maxHealth { get; private set; }

    public Sprite icon;
    public GameObject healPoint;
    public LineRenderer line;

    private Rigidbody myBody;
    private Transform target;
    private float checkTimer = 0.5f;

    public bool isEnemy = false;
    public bool fresh = true;

    #region Getters Setters plus
    public bool GetIsFlying()
    {
        return true;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetRange()
    {
        return range;
    }
    public int GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public Sprite GetIcon()
    {
        return icon;
    }
    public void SetIsEnemy()
    {
        isEnemy = true;
    }
    public void AddHealth(int health)
    {
        this.health += health;
    }
    #endregion

    private void Awake()
    {
        myBody = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Set max health
        maxHealth = health;

        StartCoroutine(Heal());
    }

    // Update is called once per frame
    void Update()
    {
        if (fresh)
        {
            FreshMove();
        }
        else
        {
            Move();
            Rotate();
        }
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            if (isEnemy)
            {
                target = GameManager.instance.FindClosestEnemyDamagedTroop(gameObject.transform);
            }
            else
            {
                target = GameManager.instance.FindClosestFriendlyDamagedTroop(gameObject.transform);
            }
            checkTimer = 0.5f;
        }

        if (target != null && Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) <= range)
        {
            line.SetPosition(0, healPoint.transform.position);
            line.SetPosition(1, target.position);
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }

    private void FreshMove()
    {
        if (transform.position.y < 12f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }

    private void Move()
    {
        if (transform.position.y < 12f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5);
        }

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > stopDistance)
                myBody.AddForce(transform.forward * Time.deltaTime * speed);
            else if (myBody.velocity.magnitude > 0 && Vector3.Distance(transform.position, target.position) < stopDistance)
                myBody.AddForce(-myBody.velocity * Time.deltaTime * 50f);
        }
        else
        {
            myBody.AddForce(-myBody.velocity * Time.deltaTime * 50f);
        }

        if ((transform.rotation * myBody.velocity).x > 20f)
        {
            myBody.AddForce(-transform.right * Time.deltaTime * 50f);
        }
        if ((transform.rotation * myBody.velocity).x < -20f)
        {
            myBody.AddForce(transform.right * Time.deltaTime * 50f);
        }
    }
    private void Rotate()
    {
        if (target != null)
        {
            Vector3 targetOnGround = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 targetDirection = targetOnGround - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    IEnumerator Heal()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) <= range)
            {
                ISpawnable iS = target.gameObject.GetComponent<ISpawnable>();

                //If current health + healing is still less than the max health, then heal
                if (iS.GetHealth() + healing <= iS.GetMaxHealth())
                {
                    iS.AddHealth(healing);
                }
                else
                {
                    //If healing would bring object obove max health, set health to max
                    iS.AddHealth(iS.GetMaxHealth() - iS.GetHealth());
                }
            }
        }
        yield return new WaitForSeconds(healingRate);
        StartCoroutine(Heal());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fresh Gate")
        {
            fresh = false;
        }
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
