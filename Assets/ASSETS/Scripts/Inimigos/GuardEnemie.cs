using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class GuardEnemie : MonoBehaviour
{

    Vector3 GuardPosition; 



    [SerializeField] NavMeshAgent enemieAgent;
    [SerializeField] Transform target;
    [SerializeField] GameObject targets;
    [SerializeField] GameObject[] DoorsToCloseIfPlayerDetected;
    EnemieHealth enemieHp;


    public string spawnerName;

    
    float distanceBtwPlayerandEnemie;
    [SerializeField] float EnemieVisionDistanceMax = 15f;

    float timer;
    float maxTimeBeforeFollowPlayer = 0.2f;
    float maxDistanceFollowing = 100f;
    public Vector3 directionGiveByAlly;



    
    [Task]
    bool Idle = true;
    [Task]
    [SerializeField] bool CanSeePlayer;
    [Task]
    bool lostPlayer;
    [Task]
    bool isPatrolling = true; 
    [Task]
    bool isChasing; 
    [Task]
    bool returningPoint; 
    [Task]
    bool isShooting = false;







    private void Start()
    {
       
        GuardPosition = enemieAgent.transform.position;
        targets = GameObject.FindGameObjectWithTag("Player");
        enemieHp = GetComponent<EnemieHealth>();
    }

    private void Update()
    {
        if (enemieHp.ActualHp <=0)
        {
            for (int i = 0; i < DoorsToCloseIfPlayerDetected.Length; i++)
            {
                DoorsToCloseIfPlayerDetected[i].gameObject.GetComponentInChildren<DoorScript>().OrderToCloseDoor = false;
            }
            Destroy(gameObject);
        }
    }

    [Task]
    void Idling()
    {
     
        if (Idle)
        {
          
             
                lostPlayer = false;
                returningPoint = false;
                isPatrolling = true;
             
                enemieAgent.destination = GuardPosition;
        }
      
        enemieAgent.speed = 0f;
        enemieAgent.velocity = Vector3.zero;
          Idle = false;
        ThisTask.Succeed();

    }

    [Task]
    void CloseAllDoors()
    {
        if (CanSeePlayer)
        {
            for (int i = 0; i < DoorsToCloseIfPlayerDetected.Length; i++)
            {
                if (DoorsToCloseIfPlayerDetected[i].gameObject.GetComponentInChildren<DoorScript>().OrderToCloseDoor != true)
                {
                    DoorsToCloseIfPlayerDetected[i].gameObject.GetComponentInChildren<DoorScript>().OrderToCloseDoor = true;
                }
            }
        }
        else if (!CanSeePlayer)
        {
            for (int i = 0; i < DoorsToCloseIfPlayerDetected.Length; i++)
            {
                DoorsToCloseIfPlayerDetected[i].gameObject.GetComponentInChildren<DoorScript>().OrderToCloseDoor = false;
            }
        }
    }


    [Task]
    void CanSeePlayerTask()
    {
        distanceBtwPlayerandEnemie = Vector3.Distance(transform.position, targets.transform.position );

        if (distanceBtwPlayerandEnemie <= EnemieVisionDistanceMax)
        {
            enemieAgent.speed = 3.5f;
            CanSeePlayer = true;
                                
            ThisTask.Succeed();

        }
        else
        {
              CanSeePlayer = false;
            ThisTask.Succeed();
        }

    }

    [Task]
    void CantSeePlayerTask()
    {
        
        if (!CanSeePlayer && !isChasing)
        {
            
            ThisTask.Fail();
        }
        
        if (!CanSeePlayer && isChasing)
        {
            isChasing = false;
            lostPlayer = true; 
            ThisTask.Succeed();
        }
    }


    [Task] 
    void AlertNearEnemies()
    {
        if (CanSeePlayer)
        {
            
            GameObject[] allysNear = GameObject.FindGameObjectsWithTag("Enemie");

        
         
            for (int i = 0; i < allysNear.Length; i++)

            {
                float DistToOthers = Vector3.Distance(this.transform.position, allysNear[i].transform.position);

                if (DistToOthers < 30f)
                {

                   
                    if (allysNear[i].GetComponent<EnemyControllerPanda>() != null)
                    {
                        allysNear[i].GetComponent<EnemyControllerPanda>().directionGiveByAlly = this.enemieAgent.destination;
                    }
                    if (allysNear[i].GetComponent<GuardEnemie>() != null)
                    {
                        allysNear[i].GetComponent<GuardEnemie>().directionGiveByAlly = this.enemieAgent.destination;
                    }
                   

                }
            }



         
            isChasing = true;

            ThisTask.Succeed();
        }
        else
        {
         
            ThisTask.Fail();

        }



    }


    [Task]
    void ChaseThePlayerTask()
    {
        if (isChasing)
        {
           

            timer += Time.deltaTime;
            if (timer >= maxTimeBeforeFollowPlayer) 
            {
                
                transform.LookAt(targets.transform);

                
                Vector3 moveTo = Vector3.MoveTowards(transform.position, targets.transform.position, maxDistanceFollowing);
                
                enemieAgent.destination = moveTo;
                directionGiveByAlly = enemieAgent.destination;
                timer = 0f;
            }
            ThisTask.Succeed();

        }
        else
        {
            ThisTask.Fail();
        }

    }



    [Task]
    void LostPlayerPanic()
    {
        if (lostPlayer && !returningPoint)
        {
            enemieAgent.destination = this.transform.position;
            

            timer += Time.deltaTime;
            if (timer >= 30f) 
            {
               
                timer = 0f;
                lostPlayer = false;
                isChasing = false;
                returningPoint = true;
                ThisTask.Succeed();

            }
          
        }
        else
        {
            ThisTask.Fail();
        }
        


    }


    [Task]
    void GoBackToOriginPosition()
    {
        float REF = Vector3.Distance(enemieAgent.transform.position, setDestinationReturn(GuardPosition));

        if (returningPoint)
        {
            Debug.Log("1");
            setDestination(GuardPosition);
        }
        if (REF <= 2f)
        {
            returningPoint = false;
             Idle = true;
        }
        #region outra abordagem:

      
        #endregion
        ThisTask.Succeed();

    }



    
    [Task]
    void setDestination(Vector3 Destination)
    {        
            enemieAgent.destination = Destination;        
            ThisTask.Succeed();
    }

    Vector3 setDestinationReturn(Vector3 Destination)
    {
        enemieAgent.destination = Destination;
        return enemieAgent.destination;
    }
    [Task]
    void CheckingPlayer()
    {
        if (lostPlayer)
        {
            Vector3 x = this.transform.position;
            enemieAgent.destination = x;
            lostPlayer = false;
            isPatrolling = true;
            ThisTask.Succeed();

        }
    }





}
