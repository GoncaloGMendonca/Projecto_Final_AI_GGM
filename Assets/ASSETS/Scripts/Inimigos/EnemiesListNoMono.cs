using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesListNoMono 
{
   public static List<GameObject> enemiesPatrolList = new List<GameObject>(); //Lista que ira guardar os inimigos ativos!
    public static List<GameObject> enemiesGuardList = new List<GameObject>(); //Lista que ira guardar os inimigos ativos!

    public EnemiesListNoMono()
    {
        enemiesPatrolList = new List<GameObject>();
        enemiesGuardList = new List<GameObject>();
    }

}
