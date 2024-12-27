using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notify_damage_parent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag=="pdamage"){
            
        }
    }
}
