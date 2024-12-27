using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class follow_players : MonoBehaviour
{
    private Transform target_object;
    private Quaternion target_rot;
    private Quaternion current_rot;
    private string target_name;
    private int dir;
    private bool rotate;
    private float vel;

    void Start()
    {
        target_name = "Player";
        target_object = GameObject.FindGameObjectWithTag(target_name).transform;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        update_position();
        self_rotate();
    }

    private void self_rotate(){
        if(rotate){
            // var tmp_step = vel*Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,target_rot,vel*.1f);
            var tmp_rot_dif = transform.rotation*Quaternion.Inverse(target_rot);
            if(tmp_rot_dif.eulerAngles.magnitude<.05){
                transform.rotation = target_rot;
                rotate = false;
            }
        }
    }

    void update_position(){
        transform.position = target_object.position;
    }
    public void setRotate(int _dir,float _vel){
        rotate = true;
        dir = _dir;
        vel = _vel;
        current_rot = transform.rotation;
        target_rot = transform.rotation*Quaternion.Euler(0,90*dir,0);
    }


}
