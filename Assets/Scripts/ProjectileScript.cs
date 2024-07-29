using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool isMissile;
    public float lifeTime = 30f;
    public Transform target;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
    }

    void Move()
    {
        if (isMissile)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > 2)
                    transform.LookAt(target);
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine("DestroyProjectile");
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}
