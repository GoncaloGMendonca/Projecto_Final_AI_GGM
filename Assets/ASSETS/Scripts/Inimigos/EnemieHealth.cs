using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemieHealth : MonoBehaviour
{
    [SerializeField] float maxHp = 3f;
    [SerializeField] float minHp = 0f;
    [SerializeField] float actualHp = 3f;
    public float ActualHp
    {
        get
        {
            return actualHp;
        }
        set
        {
            actualHp = value;
        }
    }


    [SerializeField] GameObject[] hpSprites;

    private void Start()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            actualHp--;
            Destroy(hpSprites[hpSprites.Length-1].gameObject);
        }
    }
}
