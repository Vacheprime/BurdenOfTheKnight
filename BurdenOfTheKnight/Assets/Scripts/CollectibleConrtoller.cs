using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleConrtoller : MonoBehaviour
{
    Vector3 speed = new Vector3(0, 50f, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * 5f * Time.deltaTime);
    }
}
