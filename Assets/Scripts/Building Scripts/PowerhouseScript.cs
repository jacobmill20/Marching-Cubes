using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerhouseScript : MonoBehaviour, UtilityInterface
{
    public List<GameObject> buildings = new List<GameObject>();

    void Start()
    {
        GameManager.instance.placeUtilities.Add(this); 
    }

    void OnDestroy()
    {
        if (enabled == true)
        {
            foreach (GameObject b in buildings)
            {
                if (b != null)
                    b.GetComponent<TurretScript>().damage /= 2;
            }
            GameManager.instance.placeUtilities.Remove(this);
        }
    }

    public void StartUtility()
    {
        //Debug.DrawRay(transform.position, Vector3.forward * 2.5f, Color.white, 100f);
        Vector3 start = transform.position + Vector3.up;
        
        //Chech surrounding buidlings
        if(Physics.Raycast(start, Vector3.forward, out RaycastHit hit, 5f))
        {
            AddBuilding(hit.collider.gameObject);
        }
        if (Physics.Raycast(start, Vector3.right, out hit, 5f))
        {
            AddBuilding(hit.collider.gameObject);
        }
        if (Physics.Raycast(start, Vector3.back, out hit, 5f))
        {
            AddBuilding(hit.collider.gameObject);
        }
        if (Physics.Raycast(start, Vector3.left, out hit, 5f))
        {
            AddBuilding(hit.collider.gameObject);
        }
    }

    private void AddBuilding(GameObject building)
    {
        if (!buildings.Contains(building) && building.TryGetComponent(out TurretScript turret))
        {
            buildings.Add(building);
            turret.damage *= 2;
        }
    }

    public void DisableUtility()
    {
        this.enabled = false;
    }
}
