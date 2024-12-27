using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class weapon_holder : MonoBehaviour
{
    [SerializeField] private weapon_info holding;
    [SerializeField] private Damage_Type damage_type;
    [SerializeField] private Damage_Origin owner;
    [SerializeField] private float damage_ammount;
    [SerializeField] private List<GameObject> avilable_weapons;
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private int weapon_hold_index=0;
    [SerializeField] private float attack_tspacing=0.5f;
    [SerializeField] private bool attacking;
    [SerializeField] private float attacking_timer;
    [SerializeField] private string owner_tag;
    // Start is called before the first frame update
    void Start()
    {   attacking = false;
        attacking_timer = 0f;
        owner_tag = "weapon_unactive";
        if(weapons.Count<1)instanciate_weapons();
    }

    void instanciate_weapons(){
        for (int tmp_index=0;tmp_index < avilable_weapons.Count;tmp_index++)
        {
            weapons.Add(Instantiate(avilable_weapons[tmp_index],transform.position,quaternion.identity,transform));
            weapons[tmp_index].transform.localScale = new UnityEngine.Vector3(1,1,1);
            weapons[tmp_index].transform.rotation = new UnityEngine.Quaternion(0,0,0,0);
            weapons[tmp_index].SetActive(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        weapons[weapon_hold_index].SetActive(true);
        if(attacking){
            attacking_timer+=Time.deltaTime;
            if(attacking_timer>=attack_tspacing){
                attacking_timer=0f;
                attacking=false;
                weapons[weapon_hold_index].transform.Find("weapon").transform.tag = "weapon_unactive";
            }
        }
    }

    public bool change_weapon(Damage_Origin _owner,Damage_Type _dtype,weapon_info w_meta,int avilable_ammo = 0){
        // try{
        if(weapons.Count<1)instanciate_weapons();
        holding = w_meta;
        damage_type = _dtype;
        owner = _owner;
        attack_tspacing =w_meta.attack_tspacing;
        weapons[weapon_hold_index].SetActive(false); 
        weapon_hold_index = (int)w_meta.Wtype;
        weapons[weapon_hold_index].SetActive(true);
        weapons[weapon_hold_index].transform.Find("weapon").transform.tag = "weapon_unactive";
        owner_tag = default_weapons.damge_tag[owner];
        if(holding.Wtype==Weapon_type.GUN || holding.Wtype==Weapon_type.GUN || holding.Wtype==Weapon_type.GUN){
            weapons[weapon_hold_index].GetComponent<shot_trigger>().add_ammo(avilable_ammo);
        }
        if(holding.Wtype==Weapon_type.NONE||holding.Wtype==Weapon_type.DAGE||holding.Wtype==Weapon_type.SWORD||holding.Wtype==Weapon_type.POWER_SWORD){
            var tmp_info = weapons[weapon_hold_index].transform.Find("weapon").GetComponent<mele_foreward_damage>();
            tmp_info.set_damage_amount((int)holding.damage);
            tmp_info.set_dtype(damage_type);
        }
        return true;
        // }catch{}
        // return false;
    }

    public int attack(){
        try{
            weapons[weapon_hold_index].transform.Find("weapon").transform.tag = owner_tag;
            attacking = true;
            if(holding.Wtype==Weapon_type.GUN){ return weapons[weapon_hold_index].GetComponent<shot_trigger>().shot(owner,damage_type,(int)holding.damage); }
            if(holding.Wtype==Weapon_type.SUB_FUSIL||holding.Wtype==Weapon_type.FUSIL){ return weapons[weapon_hold_index].GetComponent<shot_trigger>().auto_shot(owner,damage_type,(int)holding.damage); }
            
        }catch{ weapons[weapon_hold_index].transform.Find("weapon").transform.tag = "weapon_unactive"; return 0;}
        return 0;
    }

    public weapon_info get_damage_metadata(){return holding;}
    public Damage_Type get_damage_type(){return damage_type;}
}
