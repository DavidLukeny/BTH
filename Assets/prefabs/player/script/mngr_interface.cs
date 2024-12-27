using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mngr_interface : MonoBehaviour
{
    public Transform get_player_transform(){
        return this.gameObject.transform.Find("player_holder").transform;
    }
}
