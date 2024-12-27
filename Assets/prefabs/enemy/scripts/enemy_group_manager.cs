using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RSM;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class enemy_group_manager : MonoBehaviour
{
    [SerializeField] private GameObject boss_prefab;
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private GameObject coin;
    [SerializeField] public GameObject drop_weapon;
    [SerializeField] public weapon_info drop_info;
    [SerializeField] public int drop_ammo;
    [SerializeField] private Damage_Type group_damage;
    [SerializeField] private int min_coin_value;
    [SerializeField] private int max_coin_value;
    [SerializeField] private int base_life;
    [SerializeField] private float standard_dev;
    [SerializeField] private int enemy_number;
    [SerializeField] private float boss_size = 2f;
    [SerializeField] private List<weapon_info> avilable_weapons = new List<weapon_info>{default_weapons.weapons[Weapon_type.DAGE],default_weapons.weapons[Weapon_type.NONE]};
    private GameObject boss_holder;
    private GameObject enemy_holder;

    private float timer=0;

    private bool init=false;

    // Start is called before the first frame update
    void Start()
    {
        boss_holder=transform.Find("boss_holder").gameObject;
        enemy_holder=transform.Find("enemis_holder").gameObject;
    }

    void FixedUpdate(){
        timer+=Time.deltaTime;
        if(!init){
            for(int i = 0; i<enemy_number; i++){
                create_new_enemy();
            }
            create_new_boss();
            init=true;
        }
        if(timer>10f){
            timer=0;
            if(boss_holder.transform.childCount<1){create_new_boss();}
            if(enemy_holder.transform.childCount<enemy_number){create_new_enemy();}
        }
    }

    private void create_new_enemy(){
        try{
            var enemy_obj = Instantiate(enemy_prefab,new Vector3(transform.position.x+UnityEngine.Random.Range(-20f,20f),2f,transform.position.z+UnityEngine.Random.Range(-20f,20f)),Quaternion.identity,enemy_holder.transform);
            var enemy_info = Enemys.get_random_enemy(group_damage,avilable_weapons);
            enemy_info.life = (1+UnityEngine.Random.Range(-standard_dev,standard_dev))*base_life;
            enemy_info.max_life = enemy_info.life;
            enemy_obj.GetComponent<enemy_behavior>().set_char_metadata(enemy_info);
        }catch(Exception e){Debug.Log(e);}

    }
    private void create_new_boss(){
        // if(boss_instance){Destroy(boss_instance);}
        var enemy_info = Enemys.get_random_enemy(group_damage,avilable_weapons);
        var boss_instance = Instantiate(boss_prefab,new Vector3(transform.position.x+UnityEngine.Random.Range(-20f,20f),2f,transform.position.z+UnityEngine.Random.Range(-20f,20f)),Quaternion.identity,boss_holder.transform);
        boss_instance.transform.localScale=Vector3.one*boss_size;
        enemy_info.life = (1+UnityEngine.Random.Range(-standard_dev,standard_dev))*base_life*5;
        enemy_info.max_life = enemy_info.life;
        boss_instance.GetComponent<enemy_behavior>().set_detection_range(1f+boss_size*.1f);
        boss_instance.GetComponent<enemy_behavior>().set_char_metadata(enemy_info);
    }
    public List<weapon_info> get_winfo(){
        return avilable_weapons;
    }

    public void target_broadcast(GameObject broad_target){
        for(var i=0; i<enemy_holder.transform.childCount;i++){
            var tmp_enemy = enemy_holder.transform.GetChild(i);
            tmp_enemy.GetComponent<enemy_behavior>().set_broad_target(broad_target);
        }
        for(var i=0; i<boss_holder.transform.childCount;i++){
            var tmp_enemy_boss = boss_holder.transform.GetChild(i);
            tmp_enemy_boss.GetComponent<enemy_behavior>().set_broad_target(broad_target);
        }
    }

    public void kill_instance(string name,Vector3 pos){
        for(int i=0;i<boss_holder.transform.childCount;i++){
            var tmp_boss = boss_holder.transform.GetChild(i);
            if(tmp_boss.GetComponent<enemy_behavior>().get_metadata().name.Equals(name)){
                // var wdrop_instance = Instantiate(drop_weapon,pos,Quaternion.identity);
                // var tmp_wdrop_info = wdrop_instance.transform.Find("weapon").GetComponent<weapon_meta_drop>();
                // tmp_wdrop_info.set_ammo(drop_ammo);
                // tmp_wdrop_info.set_weapon_info(drop_info);
                // create_new_boss();
                return;
            }
        }
        var coin_instance = Instantiate(coin,pos,Quaternion.identity);
        coin_instance.GetComponent<money_metadata>().set_value(UnityEngine.Random.Range(min_coin_value,max_coin_value));
        // create_new_enemy();   
    }
}
