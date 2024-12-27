using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class shot_trigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ammo;
    [SerializeField] private GameObject shotting_position;
    [SerializeField] private int avilable_ammo=0;
    [SerializeField] private string damage_origin_tag;
    [SerializeField] private string damage_type;
    
    void Start()
    {
        shotting_position = transform.Find("shotter").gameObject;
    }

    public void add_ammo(int ammount){avilable_ammo=ammount;}
    public int shot(Damage_Origin _do,Damage_Type _dtype,int _damage){
        if(avilable_ammo>0){
            var ball = Instantiate(ammo,shotting_position.transform.position,Quaternion.identity,shotting_position.transform);
            ball.GetComponent<Rigidbody>().velocity=shotting_position.transform.forward*20;
            ball.GetComponent<timed_ammo_autokill>().set_source_tag(_do);
            ball.GetComponent<timed_ammo_autokill>().set_damage_amount(_damage);
            ball.GetComponent<timed_ammo_autokill>().set_dtype(_dtype);
            avilable_ammo-=1;
        }
        return avilable_ammo;
    }
    public int auto_shot(Damage_Origin _do,Damage_Type _dtype,int _damage){
        for (int counter= 0; counter<6; counter++){
            if(avilable_ammo>0){
                var ball = Instantiate(ammo,shotting_position.transform.position,Quaternion.identity,shotting_position.transform);
                ball.GetComponent<Rigidbody>().velocity=shotting_position.transform.forward*20;
                ball.GetComponent<timed_ammo_autokill>().set_source_tag(_do);
                ball.GetComponent<timed_ammo_autokill>().set_damage_amount(_damage);
                ball.GetComponent<timed_ammo_autokill>().set_dtype(_dtype);
                avilable_ammo-=1;
            }else{return 0;}
        }
        return avilable_ammo;
    }
}
