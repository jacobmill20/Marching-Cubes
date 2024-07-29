using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            ProjectileScript projectileData = collision.gameObject.GetComponent<ProjectileScript>();
            //Debug.Log("Damage Taken = " + projectileData.damage);
        }
    }
}
