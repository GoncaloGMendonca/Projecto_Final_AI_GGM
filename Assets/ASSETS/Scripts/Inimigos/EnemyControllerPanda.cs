using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class EnemyControllerPanda : MonoBehaviour
{

    [SerializeField] GameObject target;

    [SerializeField] NavMeshAgent enemieAgent;

    [SerializeField] Transform[] destinations;
    int currentPoint;


    float timer;
    float maxTimeBeforeFollowPlayer = 2f;
    float maxDistanceFollowing = 100f;
    public Vector3 directionGiveByAlly;


    
    float distanceBtwPlayerandEnemie;

    
    [SerializeField] float EnemieVisionDistanceMax = 25f;
    float AtackRadius;


    bool isMelleAtack;
    bool isRangedAtack;


    [Task]
    bool CanSeePlayer;

    [Task]
    bool isPatrolling = true; 

    [Task]
    bool isChasing; 


    [Task]
    bool lostPlayer;

    [Task]
    bool alertEnemieHelp;

   
    [Task]
    bool CanAtackPlayer;
    [Task]
    bool CanAtackPlayerClose;


    EnemieHealth enemieHp;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        AtackRadius = EnemieVisionDistanceMax - 2;
        enemieHp = GetComponent<EnemieHealth>();
    }

    private void Update()
    {
        if (enemieHp.ActualHp <= 0)
        {
           Destroy(gameObject);
        }
    }

    
    [Task]
    void CanSeePlayerTask()
    {
        
        distanceBtwPlayerandEnemie = Vector3.Distance(transform.position, target.transform.position);

        if (distanceBtwPlayerandEnemie <= EnemieVisionDistanceMax)
        {
            CanSeePlayer = true;
            isPatrolling = false;
            alertEnemieHelp = true;

        }
        else 
        {
            CanSeePlayer = false;
            isPatrolling = true;
        }
        ThisTask.Succeed();

    }

    [Task]
    void ChaseThePlayerTask()
    {
        

        isPatrolling = false;
        isChasing = true;

        if (CanSeePlayer)
        {
         
           
            timer += Time.deltaTime;
            if (timer >= maxTimeBeforeFollowPlayer)
            {

               
                CanSeePlayerTask();
                if (CanSeePlayer)
                {
                    
                    transform.LookAt(target.transform);

                    Vector3 moveTo = Vector3.MoveTowards(transform.position, target.transform.position, maxDistanceFollowing);
            
                    enemieAgent.destination = moveTo;
                    directionGiveByAlly = enemieAgent.destination;
                }
            

            }

        }
        ThisTask.Succeed();
    }


    [Task]
    void PatrollingAroundPoints()
    {
        if (!isMelleAtack && enemieAgent.remainingDistance < 0.5f)
        {
            isPatrolling = true;
            isChasing = false;
            enemieAgent.destination = destinations[currentPoint].position;
            UpdateCorrentPoint();
        }
        Task.current.Succeed();
    }

    void UpdateCorrentPoint()
    {
        
        if (currentPoint == destinations.Length - 1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }

    }


    [Task]
    void LostPlayerPanic()
    {
        lostPlayer = true;
        ThisTask.Succeed();
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
    void CheckingPlayer()
    {
        
        if (lostPlayer)
        {
            Vector3 x = this.transform.position;
            enemieAgent.destination = x;
        }
        ThisTask.Succeed();

    }

    IEnumerator ProcurarAteDesistir()
    {
        Debug.Log("A procurar jogador!");
        
        yield return new WaitForSeconds(10f);
    }



    [Task]
    void AlertNearEnemies()
    {
     
        for (int i = 0; i < EnemiesListNoMono.enemiesPatrolList.Count ; i++)
        {
            float DistToOthers = Vector3.Distance(this.transform.position, EnemiesListNoMono.enemiesPatrolList[i].transform.position);
            if (DistToOthers < 30f)
            {               

              
                EnemiesListNoMono.enemiesPatrolList[i].GetComponent<EnemyControllerPanda>().directionGiveByAlly = this.enemieAgent.destination; 

            }
        }

      
        for (int i = 0; i < EnemiesListNoMono.enemiesGuardList.Count; i++)
        {
            float DistToOthers = Vector3.Distance(this.transform.position, EnemiesListNoMono.enemiesGuardList[i].transform.position);
            if (DistToOthers < 30f)
            {

                
                EnemiesListNoMono.enemiesGuardList[i].GetComponent<GuardEnemie>().directionGiveByAlly = this.enemieAgent.destination; 
            }
        }

        alertEnemieHelp = false;

        ThisTask.Succeed();

    }


    #region para depois
    [Task]
    void IsCloseDamage()
    {
        if (distanceBtwPlayerandEnemie > AtackRadius)
        {
            Debug.Log("CloseDamage!");
            isMelleAtack = true;
            isRangedAtack = false;
        }
    }



    [Task]
    void IsRangedDamage()
    {
        if (distanceBtwPlayerandEnemie > AtackRadius / 2)
        {
            Debug.Log("RangedDamage!");

            isMelleAtack = false; 
            isRangedAtack = true;
        }
    }
    #endregion


}
