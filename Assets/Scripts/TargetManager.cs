using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] monsters;
    private GameObject activeMonster;


    private void Start()
    {
        InvokeRepeating("ActiveMonster", 1f,1f);
    }

    private void ActiveMonster()
    {
        //随机生成一种怪物
        int index = Random.Range(0, monsters.Length);
        //利用随机数，随机生成了怪物列表中的某一个怪物
        activeMonster = monsters[index];
        if (activeMonster.activeSelf==false)
        {
            activeMonster.SetActive(true);
        }
      
    }

    private void Update()
    {
        
    }
}
