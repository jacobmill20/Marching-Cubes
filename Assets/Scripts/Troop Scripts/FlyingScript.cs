using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingScript : MonoBehaviour, ISpawnable
{
    public float speed;
    public float turnSpeed;
    public float stopDistance;
    public float range;
    public int health;
    public float fireRate;

    public int maxHealth { get; private set; }

    public bool canAttackGround;
    public bool canAttackAir;
    public bool canAttackBuilding;

    public int damage;
    public float projSpeed;
    public GameObject projectile;
    public Sprite icon;

    private Rigidbody myBody;
    private Transform target;
    private float checkTimer = 0.5f;
    private float shootTimer = 0;

    public bool isEnemy = false;
    public bool fresh = true;

    #region Getters Setters plus
    public bool GetIsFlying()
    {
        return true;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetRange()
    {
        return range;
    }
    public float GetFireRate()
    {
        return fireRate;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public bool GetCanAttackGround()
    {
        return canAttackGround;
    }
    public bool GetCanAttackAir()
    {
        return canAttackAir;
    }
    public bool GetCanAttackBuilding()
    {
        return canAttackBuilding;
    }
    public GameObject GetProjectile()
    {
        return projectile;
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
        if (isEnemy)
        {
            target = GameManager.instance.findClosestFriendly(gameObject.transform, canAttackGround, canAttackAir, canAttackBuilding);
        }
        else
        {
            target = GameManager.instance.findClosestEnemy(gameObject.transform, canAttackGround, canAttackAir, canAttackBuilding);
        }

        //Set max health
        maxHealth = health;
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
            Fire();
        }
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            if (isEnemy)
            {
                target = GameManager.instance.findClosestFriendly(gameObject.transform, canAttackGround, canAttackAir, canAttackBuilding);
            }
            else
            {
                target = GameManager.instance.findClosestEnemy(gameObject.transform, canAttackGround, canAttackAir, canAttackBuilding);
            }
            checkTimer = 0.5f;
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
            else if(myBody.velocity.magnitude > 0 && Vector3.Distance(transform.position, target.position) < stopDistance)
                myBody.AddForce(-myBody.velocity * Time.deltaTime * 50f);
        } else
        {
            myBody.AddForce(-myBody.velocity * Time.deltaTime * 50f);
        }

        if((transform.rotation * myBody.velocity).x > 20f)
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

    private void Fire()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) <= range)
            {
                if (shootTimer >= fireRate)
                {
                    GameObject newProjectile = Instantiate(projectile, transform);
                    newProjectile.transform.LookAt(target);
                    newProjectile.transform.parent = GameObject.FindGameObjectWithTag("ProjectileContainer").transform;
                    ProjectileScript projScript = newProjectile.GetComponent<ProjectileScript>();
                    projScript.target = this.target;
                    projScript.damage = this.damage;
                    projScript.speed = projSpeed;
                    if (isEnemy)
                    {
                        newProjectile.layer = 9;
                    }
                    shootTimer = 0f;
                }
                shootTimer += Time.deltaTime;
            }
            else
            {
                shootTimer = fireRate;
            }
        }
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
