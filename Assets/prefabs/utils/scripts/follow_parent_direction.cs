using System.Collections;
using System.Collections.Generic;
using RSM;
using UnityEngine;

public class follow_parent_direction : MonoBehaviour
{
    private Vector3 current_direction;
    private void Start() {
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        var p_dir = Vector3.forward;
        try{
           p_dir = transform.parent.GetComponent<enemy_behavior>().dir_vel;
        }catch{
           p_dir = transform.parent.GetComponent<ally_holder>().dir_vel;
        }
        current_direction = (current_direction+p_dir*0.2f).normalized; 
        current_direction = new Vector3(current_direction.x,0,current_direction.z).normalized;
        if(current_direction.magnitude>0){
            transform.rotation = Quaternion.LookRotation(current_direction, Vector3.up);
        }
    }
}
