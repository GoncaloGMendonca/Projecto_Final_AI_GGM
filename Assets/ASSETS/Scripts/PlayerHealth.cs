using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHp = 1f;
    [SerializeField] float minHp = 0f;
    [SerializeField] float actualHp = 1f;
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
    [SerializeField] Text lifesAmount;
    [SerializeField] GameObject LooseMenu;

    private void Start()
    {
        LooseMenu.gameObject.SetActive(false);
        lifesAmount.text = actualHp.ToString("F0");
    }

    private void Update()
    {
        if (actualHp <=0)
        {
            actualHp = 0;
            Debug.Log("Dead");
            LooseMenu.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            actualHp = actualHp -1;
            lifesAmount.text = actualHp.ToString("F0");
        }
     
    }
}
