using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class EnemieShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletObj;
    [SerializeField] GameObject GunShooter;

    [SerializeField] float reloadtimeRateOfFire = 2f;
    float reloadtimeRef;

    [SerializeField] float bulletSpeed = 100f;
    [Task]
    bool isShooting = false;
    [Task]
    bool canShoot = true;

    Color refcolor;

    private void Start()
    {
        reloadtimeRef = reloadtimeRateOfFire;
    }

    private void Update()
    {
        if (canShoot)
        {
            isShooting = true;
            ShootingBullet();
        }
        if (isShooting)
        {
            if (reloadtimeRef <= 0f)
            {
                reloadtimeRef = reloadtimeRateOfFire;
                isShooting = false;
                canShoot = true;
            }
            reloadtimeRef = reloadtimeRef - 1 * Time.deltaTime;
        }
    }

    [Task]
    void ShootingBullet()
    {
        if (!isShooting)
        {
            GameObject obj = Instantiate(bulletObj, GunShooter.transform.position, Quaternion.identity, transform.parent);
            obj.GetComponent<Transform>().rotation = GunShooter.transform.localRotation;
            obj.GetComponent<Bullet>().StartBullet(GunShooter.transform.position);
            obj.GetComponent<Bullet>().StartBullet(this.transform.forward);
            isShooting = true;
            canShoot = false;
        }
    }
}
