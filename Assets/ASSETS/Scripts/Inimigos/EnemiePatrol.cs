
using UnityEngine;
using UnityEngine.AI;

public class EnemiePatrol : MonoBehaviour
{

    [SerializeField] Transform target;


    float AtackRadius = 20f;

    [SerializeField] NavMeshAgent enemieAgent;

    [SerializeField]  Transform[] destinations;
    int currentPoint;

    float timer;
    float maxTime = 1f;
    float maxDistance = 100f;
    bool inRange;



    private void Update()
    {
        if (AtackRadius <= 0.9f)
        {
            
        }
    }

    private void FixedUpdate()
    {
        float distTo = Vector3.Distance(transform.position, target.position);


        if (distTo <= AtackRadius)
        {
            

            timer += Time.deltaTime;
            if (timer > maxTime)
            {
                inRange = true;
                
                transform.LookAt(target);
                Vector3 moveTo = Vector3.MoveTowards(transform.position, target.position, maxDistance) ;
                enemieAgent.destination = moveTo;
            }
        }
        else if (distTo > AtackRadius)
        {
            inRange = false;
            BackToPath();
        }
    }

    void BackToPath()
    {
        if (!inRange && enemieAgent.remainingDistance < 0.5f)
        {
            enemieAgent.destination = destinations[currentPoint].position;
            UpdateCorrentPoint();
        }


    }

    void UpdateCorrentPoint()
    {
        if (currentPoint == destinations.Length -1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }

    }

}
