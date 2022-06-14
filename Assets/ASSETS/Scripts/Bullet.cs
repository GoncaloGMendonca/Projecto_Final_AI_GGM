using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 25f;

    Vector3 shootdirection;
    public Vector3 Shootdirection
    {
        get
        {
            return shootdirection;
        }
        set
        {
            shootdirection = value;
        }
    }

    bool updatebullet = false;
    

    
    public void StartBullet(Vector3 _shootdirection)
    {
        shootdirection = _shootdirection;
        updatebullet = true;

    }




    private void Update()
    {
        if (updatebullet)
        {
            transform.position += shootdirection * bulletSpeed * Time.deltaTime;
        }
    }

  

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }




}
