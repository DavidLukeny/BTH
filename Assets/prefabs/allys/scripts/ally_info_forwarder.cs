using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using RSM;
using Unity.Mathematics;
using UnityEngine;

public class ally_info_forwarder : MonoBehaviour
{
    [SerializeField] private GameObject manager;
    [SerializeField] private GameObject ally_prefab;

    private void Start() {
        manager = GameObject.FindWithTag("GameController");
        load_ally_status();
    }

    private void load_ally_status(){
        var allys = manager.GetComponent<config>().get_game_progress().allys;
        foreach(var ally in allys){
            var tmp_ally = Instantiate(ally_prefab,new UnityEngine.Vector3(0f,0f,0f),UnityEngine.Quaternion.identity,transform);
            tmp_ally.GetComponent<ally_holder>().set_char_metadata(ally);
        }
    }

    private void FixedUpdate() {
        var tmp_allys = new List<character_info>();
        for(int index = 0;index<transform.childCount;index++){
            tmp_allys.Add(transform.GetChild(index).GetComponent<ally_holder>().get_metadata());
        }
        manager.GetComponent<config>().update_allys_status(tmp_allys);
    }
}
