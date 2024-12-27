using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;

[Serializable]
public struct character_info
{
    [SerializeField] public string name;
    [SerializeField] public Vector3 position;
    [SerializeField] public float life;
    [SerializeField] public float max_life;
    [SerializeField] public Damage_Origin character_type;
    [SerializeField] public Damage_Type character_damage_type;
    [SerializeField] public float velocity;
    [SerializeField] public weapon_info primary;
    [SerializeField] public weapon_info secondary;
    [SerializeField] public int ammo_primary;
    [SerializeField] public int ammo_secondary;
    
    public character_info(
        string _name,
        Vector3 _position,
        float _life,
        float _maxl,
        Damage_Origin _ch_type,
        Damage_Type _damage_t,
        float _velocity,
        weapon_info _p,
        weapon_info _s,
        int ammo_p,
        int ammo_s
    ){
        this.name = _name;
        this.position = _position;
        this.life = _life;
        this.max_life = _maxl;
        this.character_type = _ch_type;
        this.character_damage_type = _damage_t;
        this.velocity = _velocity;
        this.primary = _p;
        this.secondary = _s;
        this.ammo_primary = ammo_p;
        this.ammo_secondary = ammo_s;
    }
}

public static class Enemys{

    private static List<string> vocal = new List<string>{"a","e","i","o","u"};
    private static List<string> cons = new List<string>{"b","c","ch","d","f","g","h","j","k","l","ll","m","mb","mp","n","ñ","p","ph","q","r","s","ss","sc","t","v","w","x","y","z"};
    public static character_info get_random_enemy(Damage_Type d_type,List<weapon_info> avilable_w){
        var _weapon = avilable_w[UnityEngine.Random.Range(0,avilable_w.Count)];
        var tmp_life_val = 150+UnityEngine.Random.Range(0f,100f);
        var name = get_random_c()+
            get_random_v()+
            get_random_c()+
            get_random_v()+
            get_random_c()+
            get_random_v()+
            get_random_c()+
            get_random_v();
        return new character_info(
            name,
            new Vector3(0,0,0),
            tmp_life_val,
            tmp_life_val,
            Damage_Origin.Enemy,
            d_type,
            6+UnityEngine.Random.Range(0f,6f),
            _weapon,
            default_weapons.weapons[Weapon_type.NONE],
            default_weapons.distance.Contains(_weapon.Wtype)?UnityEngine.Random.Range(5,20):0,
            0
        );
    }

    public static character_info get_base_enemy(){
        var tmp_life_val = 150+UnityEngine.Random.Range(0f,100f);
        return new character_info(
            "",
            new Vector3(0,0,0),
            tmp_life_val,
            tmp_life_val,
            Damage_Origin.Enemy,
            Damage_Type.NONE,
            6+UnityEngine.Random.Range(0f,6f),
            default_weapons.weapons[Weapon_type.NONE],
            default_weapons.weapons[Weapon_type.NONE],
            0,
            0
        );
    }

    public static string get_random_v(){return vocal[UnityEngine.Random.Range(0,vocal.Count)];}
    public static string get_random_c(){return cons[UnityEngine.Random.Range(0,cons.Count)];}
}

public static class Ally{
    public static character_info init_player(){
        return new character_info(
            "VEX",
            new Vector3(0,0,0),
            3000f,
            3000f,
            Damage_Origin.Ally,
            Damage_Type.NONE,
            9.5f,
            default_weapons.weapons[Weapon_type.DAGE],
            default_weapons.weapons[Weapon_type.NONE],
            0,
            0
        );
    }

    public static character_info transform_enemy(character_info _enemy){
        _enemy.character_type = Damage_Origin.Ally;
        return _enemy;
    }
}



