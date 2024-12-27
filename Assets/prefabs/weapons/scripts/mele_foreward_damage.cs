using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mele_foreward_damage : MonoBehaviour
{    // Start is called before the first frame update
    [SerializeField] private int damage_ammount;
    [SerializeField] Damage_Type d_type;
    public int get_damage_amount(){return damage_ammount;}
    public void set_damage_amount(int value){damage_ammount = value;}
    public Damage_Type get_dtype(){return d_type;}
    public void set_dtype(Damage_Type _dtype){d_type = _dtype;}
}
