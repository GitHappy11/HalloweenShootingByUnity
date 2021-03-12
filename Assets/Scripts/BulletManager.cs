using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    private void Start()
    {
        Invoke("DestroyBullet", 3f);
    }


    void DestroyBullet()
    {
        //这里使用this是销毁自己（脚本），目标是销毁游戏对象
        Destroy(gameObject);
    }
}
