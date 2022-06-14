using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool open = false;
    [SerializeField] bool close = true;
    bool closeDoor = false;
    [SerializeField] float doorAutoCloseTime = 2f;
    float doorAutoCloseTimeref;
    [SerializeField] bool orderToCloseDoor = false;
    public bool OrderToCloseDoor
    {
        get
        {
            return orderToCloseDoor;
        }
        set
        {
            orderToCloseDoor = value;
        }
    }

    private void Start()
    {
        doorAutoCloseTimeref = doorAutoCloseTime;
    }
    private void Update()
    {
        if (closeDoor)
        {
            if (doorAutoCloseTimeref <= 0f)
            {
                doorAutoCloseTimeref = doorAutoCloseTime;
                animator.SetBool("OpenClose", false);
                open = false;
                close = true;
                closeDoor = false;
            }
            doorAutoCloseTimeref = doorAutoCloseTimeref - 1 * Time.deltaTime;
        }
        if (orderToCloseDoor)
        {
            animator.SetBool("OpenClose", false);
            open = false;
            close = true;
            closeDoor = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !orderToCloseDoor)
        {
            animator.SetBool("OpenClose", true);
            open = true;
            close = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.tag == "Player" && !orderToCloseDoor)
        {
            animator.SetBool("OpenClose", false);
            open = false;
            close = true;
            closeDoor = true;
        }
    }
}
