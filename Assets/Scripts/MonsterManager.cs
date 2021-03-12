using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag=="Bullet")
        {
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}
