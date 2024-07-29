using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float range;
    public float fireRate;
    public float turnSpeed;

    public bool isEnemy = false;

    public bool canAttackGround;
    public bool canAttackAir;

    public Transform turretHead;
    public Transform[] attackPoints;
    private int attackIdx = 0;
    public int damage;
    public float projSpeed;
    public GameObject projectile;

    private Transform target;

    private float checkTimer = 0.5f;
    private float shootTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (isEnemy)
        {
            target = GameManager.instance.findClosestFriendly(gameObject.transform, canAttackGround, canAttackAir, false);
        }
        else
        {
            target = GameManager.instance.findClosestEnemy(gameObject.transform, canAttackGround, canAttackAir, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && gameObject.GetComponent<PlaceableObject>().Placed)
        {
            if (Vector3.Distance(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z)) <= range)
            {
                Rotate();
                Fire();
            }
            else
            {
                RotateForward();
                shootTimer = fireRate;
            }
        }
        else
        {
            RotateForward();
        }
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            if (isEnemy)
            {
                target = GameManager.instance.findClosestFriendly(gameObject.transform, canAttackGround, canAttackAir, false);
            }
            else
            {
                target = GameManager.instance.findClosestEnemy(gameObject.transform, canAttackGround, canAttackAir, false);
            }
            checkTimer = 0.5f;
        }
    }

    private void Rotate()
    {
        Vector3 targetDirection = target.position - turretHead.position;
        Vector3 newDirection = Vector3.RotateTowards(turretHead.forward, targetDirection, turnSpeed * Time.deltaTime, 0f);
        turretHead.rotation = Quaternion.LookRotation(newDirection);
    }
    private void RotateForward()
    {
        Vector3 direction;
        if (isEnemy)
            direction = Vector3.back;
        else
            direction = Vector3.forward;

        Vector3 newDirection = Vector3.RotateTowards(turretHead.forward, direction, turnSpeed * Time.deltaTime, 0f);
        turretHead.rotation = Quaternion.LookRotation(newDirection);
    }

    private void Fire()
    {
        if (shootTimer >= fireRate)
        {
            if (attackIdx == attackPoints.Length)
                attackIdx = 0;
            GameObject newProjectile = Instantiate(projectile, attackPoints[attackIdx++]);
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
}
