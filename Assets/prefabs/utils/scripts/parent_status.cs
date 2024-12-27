using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parent_status : MonoBehaviour
{
    [SerializeField] private Vector3 dir_movement;

    public void set_dir(Vector3 direction){
        dir_movement=direction;
    }
}
