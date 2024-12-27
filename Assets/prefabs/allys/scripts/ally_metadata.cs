using System.Collections;
using System.Collections.Generic;
using RSM;
using UnityEngine;

public class ally_metadata : MonoBehaviour
{
    [SerializeField] public character_info ally_info;
    [SerializeField] private GameObject ally_prefab;
    [SerializeField] private GameObject ally_mngr;

    private void Start() {
        ally_info = Ally.transform_enemy(transform.parent.GetComponent<enemy_behavior>().get_metadata());
        ally_mngr = GameObject.FindWithTag("mngr_ally");
        transform.gameObject.transform.SetParent(ally_mngr.transform.parent.Find("ally_drops").transform);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag.Equals("Player")){
            if(ally_mngr.transform.childCount<6){
                var tmp_ally = Instantiate(ally_prefab,transform.position,Quaternion.identity,ally_mngr.transform);
                tmp_ally.GetComponent<ally_holder>().set_char_metadata(ally_info);
            }
            Destroy(transform.gameObject);
        }    
    }
}
