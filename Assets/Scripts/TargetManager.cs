using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public GameObject[] monsters;
    private GameObject activeMonster;
    public Image imgPanel;
    public SaveType saveType;


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

    public void ClickNewGameBtn()
    {
        foreach (GameObject monster in monsters)
        {
            monster.SetActive(false);
            LocalData.shootNum = 0;
            LocalData.score = 0;
            imgPanel.gameObject.SetActive(false);
        }
    }

    public void ClickContinueGameBtn()
    {
        imgPanel.gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickSaveBtn()
    {
        Save.Instance.SavaGame(saveType);

    }
    public void ClickLoadBtn()
    {
        Save.Instance.LoadGame(saveType);
        imgPanel.gameObject.SetActive(false);
    }
   




}
