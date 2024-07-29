using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceableObject : MonoBehaviour
{
    public int powerCost;
    public int cubitsCost;
    public int health;
    public Sprite icon;
    public string info;
    public bool Placed { get; private set;  }
    public Vector3Int Size { get; private set; }
    public int maxHealth { get; private set; }
    private Vector3[] Vertices;
    private Collider[] colliders;


    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {

        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x) + 1, Math.Abs((vertices[0] - vertices[3]).y) + 1, 1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();

        //Set max health
        maxHealth = health;

        //Disable hitboxes && only disable if not an enemy turret.
        colliders = GetComponents<Collider>();
        bool isEnemyTurret;
        try
        {
            isEnemyTurret = gameObject.GetComponent<TurretScript>().isEnemy;
        } catch (NullReferenceException)
        {
            isEnemyTurret = false;
        }
        if (!isEnemyTurret) {
            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = false;
        }
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        drag.RemoveDrag();
        try
        {
            gameObject.GetComponent<SpawnBuildingScript>().StartSpawn();
        }
        catch (NullReferenceException){ }
        try
        {
            gameObject.GetComponent<UtilityInterface>().StartUtility();
        }
        catch (NullReferenceException) { }
        transform.parent = BuildingSystem.current.parent;

        //Enable hitboxes
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;

        //Subtract cost
        GameManager.instance.power -= powerCost;
        GameManager.instance.cubits -= cubitsCost;

        Placed = true;
    }

    public void ManualPlace()
    {
        Placed = true;

        //Enable hitboxes
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
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
            BuildingSystem.current.DestroyBuilding(this);
        }
    }
}
