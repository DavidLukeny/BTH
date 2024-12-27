using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct weapon_info{
    [SerializeField] public string name;
    [SerializeField] public float damage;
    [SerializeField] public float sell_price;
    [SerializeField] public float buy_price;
    [SerializeField] public Weapon_type Wtype;
    [SerializeField] public float attack_distance;
    [SerializeField] public float attack_tspacing;
    public weapon_info(string _name, float _damage_amount, float _sell_price, float _buy_price,Weapon_type _type,float _AD, float _AT){
        this.name = _name;
        this.damage = _damage_amount;
        this.sell_price = _sell_price;
        this.buy_price = _buy_price;
        this.Wtype = _type;
        this.attack_distance = _AD;
        this.attack_tspacing = _AT;
    }
}

[Serializable]
public enum Weapon_type{
    NONE,
    DAGE,
    SWORD,
    POWER_SWORD,
    GUN,
    SUB_FUSIL,
    FUSIL
};

[Serializable]
public enum Damage_Type{
    NONE,
    LIMBO,
    LUST,
    GLUTTONY,
    GREED,
    WRATH,
    HERESY,
    VIOLENSE,
    FRAUD,
    TREACHERY
}

[Serializable]
public enum Damage_Origin{
    Enemy,
    Ally
    
}

public class default_weapons{

    public static float damage_multiplier(Damage_Type receiver,Damage_Type attacker){
        var receiver_mask = ((int)receiver)%3;
        var attacker_mask = ((int)attacker)%3;
        if(receiver_mask==attacker_mask){return 1f;}
        if(receiver_mask>attacker_mask){return 0.5f;}
        if(receiver_mask==0&&attacker_mask==2||receiver_mask<attacker_mask){return 2f;}
        return 1f;
    }
    public static Dictionary<Damage_Origin,string> damge_tag = new Dictionary<Damage_Origin, string>{
        {Damage_Origin.Ally,"wepon_damage"},
        {Damage_Origin.Enemy,"enemy_damage"}
    };
    public static List<Weapon_type> mele = new List<Weapon_type>{
     Weapon_type.NONE,   
     Weapon_type.DAGE,   
     Weapon_type.SWORD,   
     Weapon_type.POWER_SWORD
    };
    public static List<Weapon_type> distance = new List<Weapon_type>{
     Weapon_type.GUN,   
     Weapon_type.FUSIL,   
     Weapon_type.SUB_FUSIL   
    };
    public static Dictionary<Weapon_type,weapon_info> weapons = new Dictionary<Weapon_type, weapon_info>{
        {Weapon_type.NONE,new weapon_info("Hand",2f,0f,0f,Weapon_type.NONE,3f,.6f)},
        {Weapon_type.DAGE,new weapon_info("Dage",20f,4f,8f,Weapon_type.DAGE,3f,.6f)},
        {Weapon_type.SWORD,new weapon_info("Small Sword",10f,100f,200f,Weapon_type.SWORD,5f,2f)},
        {Weapon_type.POWER_SWORD,new weapon_info("Big Sword",20f,150f,300f,Weapon_type.POWER_SWORD,1.5f,3f)},
        {Weapon_type.GUN,new weapon_info("Old Gun",20f,300f,600f,Weapon_type.GUN,4f,1f)},
        {Weapon_type.SUB_FUSIL,new weapon_info("Sub Fusil",10f,600f,1200f,Weapon_type.SUB_FUSIL,8f,1f)},
        {Weapon_type.FUSIL,new weapon_info("Hand",20f,1200f,2400f,Weapon_type.FUSIL,12f,2f)}
    };

    public static Dictionary<Weapon_type, string> weapon_anim= new Dictionary<Weapon_type, string>(){
        {Weapon_type.NONE,"swording"},
        {Weapon_type.DAGE,"stabbing"},
        {Weapon_type.SWORD,"swording"},
        {Weapon_type.POWER_SWORD,"swording"},
        {Weapon_type.GUN,"gun_shotting"},
        {Weapon_type.SUB_FUSIL,"gun_playing"},
        {Weapon_type.FUSIL,"gun_powered_playing"} 
    };

    public static Dictionary<Weapon_type,string> weapon_snames=new Dictionary<Weapon_type, string>{
        {Weapon_type.NONE,"Mano"},
        {Weapon_type.DAGE,"Daga"},
        {Weapon_type.SWORD,"Espada corta"},
        {Weapon_type.POWER_SWORD,"Espada larga"},
        {Weapon_type.GUN,"Pistola antigua"},
        {Weapon_type.SUB_FUSIL,"Metralleta"},
        {Weapon_type.FUSIL,"Rifle"}
    };
}