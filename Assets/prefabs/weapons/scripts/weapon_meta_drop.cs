using System.Collections;
using System.Collections.Generic;
using RSM;
using Unity.VisualScripting;
using UnityEngine;

public class weapon_meta_drop : MonoBehaviour
{
    [SerializeField] private weapon_info metadata;
    [SerializeField] private int ammo;

    private void Start() {
        metadata = transform.parent.GetComponent<enemy_behavior>().weapon_drop_info;
        ammo = transform.parent.GetComponent<enemy_behavior>().weapon_drop_ammo;
        transform.parent = GameObject.FindWithTag("mngr_ally").transform.parent.Find("weapon_drops").transform;
    }
    public void set_weapon_info(weapon_info _meta){metadata = _meta;}
    public void set_ammo(int _ammo){ammo = _ammo;}
    public weapon_info get_weapon_info(){return metadata;}
    public int get_ammo(){return ammo;}
    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag.Equals("Player")){
            Destroy(transform.gameObject,0.5f);
        }
    }
}
