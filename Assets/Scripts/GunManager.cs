using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    private float maxYRotation = 120;
    private float minYRotation = 0;

    private float maxXRotation = 60;
    private float minXRotation = 0;

    private float shootTime = 0.0f;
    private float shootTimer = 0;


    public GameObject bulletPrefab;
    public Transform fireTrans;

    public Text txtShootNum;
    public Text txtScore;

    private void Update()
    {

        if (GameManager.isPaused==false)
        {
            MoveMouse();
            Shoot();
            RefreshUI();
        }
        
    }


    private void RefreshUI()
    {
        txtShootNum.text = "射击数：" + LocalData.shootNum;
        txtScore.text = "得分：" + LocalData.score;
    }

    private void MoveMouse()
    {
        //鼠标跟屏幕比例
        float xPosPrecent = Input.mousePosition.x / Screen.width;
        float yPosPrecent = Input.mousePosition.y / Screen.height;

        float xAngle = -Mathf.Clamp(yPosPrecent * maxXRotation, minXRotation, maxXRotation) + 15;
        float yAngle = Mathf.Clamp(xPosPrecent * maxYRotation, minYRotation, maxYRotation) - 60;

        transform.eulerAngles = new Vector3(xAngle, yAngle - 180, 0);

        RefreshUI();
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            //射击
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bulletCurrent = Instantiate(bulletPrefab, fireTrans.position, Quaternion.identity);
                bulletCurrent.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
                gameObject.GetComponent<Animation>().Play();
                gameObject.GetComponent<AudioSource>().Play();
                LocalData.shootNum += 1;
                shootTimer = 0;
            }
        }
    }

}
