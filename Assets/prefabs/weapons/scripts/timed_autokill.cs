using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timed_autokill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float timer=0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        timer+=Time.deltaTime;
        if(timer>=10){
            Destroy(transform.gameObject);
        }
    }
}
