using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timed_ammo_autokill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float time_counter =0f;
    [SerializeField] private int damage_ammount;
    [SerializeField] private Damage_Type type;
    public void set_source_tag(Damage_Origin source){transform.tag = default_weapons.damge_tag[source];}
    public Damage_Type get_dtype(){return type;}
    public void set_dtype(Damage_Type _dtype){type = _dtype;}
    public int get_damage_amount(){return damage_ammount;}
    public void set_damage_amount(int value){damage_ammount = value;}

    private void FixedUpdate() {
        time_counter+=Time.deltaTime;
        if(time_counter>3f){
            Destroy(transform.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals(default_weapons.damge_tag[Damage_Origin.Ally])||other.tag.Equals(default_weapons.damge_tag[Damage_Origin.Enemy])){
            Destroy(transform.gameObject,1f);
        }
    }
}
