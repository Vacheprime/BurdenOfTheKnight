using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        if (health == 0)
        {
            health = 100;
        }
    }

    public void Damage()
    {
        health -= 20;
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
        
    }
}
