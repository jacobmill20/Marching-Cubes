using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerScript : MonoBehaviour, ISpawnable
{
    // Start is called before the first frame update
    public bool isEnemy = false;

    public float speed;
    public float turnSpeed;
    public float stopDistance;
    public int health;
    public int mineAmount;

    public int maxHealth { get; private set; }

    private Rigidbody myBody;
    private Animator myAnim;
    private Transform target;
    private float checkTimer = 0.5f;

    public bool fresh = true;

    public int GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetIsEnemy()
    {
        isEnemy = true;
    }
    public void AddHealth(int health)
    {
        this.health += health;
    }

    private void Awake()
    {
        myBody = gameObject.GetComponent<Rigidbody>();
        myAnim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {   
        target = GameManager.instance.FindClosestOre(gameObject.transform);
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
            target = GameManager.instance.FindClosestOre(gameObject.transform);
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

    IEnumerator Mine()
    {
        GameManager.instance.cubits += mineAmount;
        yield return new WaitForSeconds(3f);
        StartCoroutine("Mine");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fresh Gate")
        {
            fresh = false;
        }

        if (other.tag == "Ore")
        {
            myAnim.SetBool("Mine", true);
            StartCoroutine("Mine");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ore")
        {
            myAnim.SetBool("Mine", false);
            StopCoroutine("Mine");
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