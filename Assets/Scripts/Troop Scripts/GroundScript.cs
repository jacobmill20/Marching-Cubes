using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundScript : MonoBehaviour, ISpawnable
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
        if (myBody.velocity.magnitude < speed)
            myBody.AddForce(transform.forward * Time.deltaTime * 500f);
    }

    private void Move()
    {
        if (target != null)
        {
            if (myBody.velocity.magnitude < speed && Vector3.Distance(transform.position, target.position) > stopDistance)
                myBody.AddForce(transform.forward * Time.deltaTime * 500f);
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
            //Check for distance as if the target was at the same y value as the troop. This effectively makes the range a cylinder around the transform insead of a sphere
            if (Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) <= range)
            {
                if (shootTimer >= fireRate)
                {
                    GameObject newProjectile = Instantiate(projectile, transform);
                    ProjectileScript projScript = newProjectile.GetComponent<ProjectileScript>();

                    //Aim projectile vertically
                    float targetY = target.transform.position.y;
                    //If target is a building, aim slightly higher because their transforms are on the ground
                    if(target.gameObject.TryGetComponent<PlaceableObject>(out PlaceableObject idontneedthis))
                    {
                        targetY += 1f;
                    }
                    float opposite = targetY - transform.position.y;
                    float adjacent = Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z));
                    float degUp = Mathf.Atan(opposite / adjacent) * Mathf.Rad2Deg;
                    newProjectile.transform.Rotate(new Vector3(-degUp, 0f, 0f));

                    //Set projectile info
                    newProjectile.transform.parent = GameObject.FindGameObjectWithTag("ProjectileContainer").transform;
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
        if(other.tag == "Fresh Gate")
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
