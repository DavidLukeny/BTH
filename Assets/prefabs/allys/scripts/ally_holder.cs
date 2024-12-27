using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using TMPro;

namespace RSM{
public class ally_holder : MonoBehaviour,IStateBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Vector3 dir_vel;
    [SerializeField] private Animator animation_controller;
    [SerializeField] private character_info char_metadata;
    [SerializeField] private GameObject weapon_changer;
    [SerializeField] private GameObject target_player;
    [SerializeField] private GameObject target;
    [SerializeField] private AudioSource walking_audio;
    [SerializeField] private AudioSource hit_sound;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Dictionary<UnityEngine.Vector3,float> avilable_movements;
    [SerializeField] private int number_of_rays=11;
    [SerializeField] private float detection_range=1;
    [SerializeField] private int direction_selected=0;
    [SerializeField] private float time_spacing=2;
    [SerializeField] private float time_waiter=2;
    [SerializeField] private float patroll_max_distance =5f;
    [SerializeField] private float player_max_distance =7f;
    [SerializeField] private bool attack_avilable=true;
    [SerializeField] private bool attacked=false;
    [SerializeField] private bool is_moving=false;
    [SerializeField] private bool no_life=false;
    [SerializeField] private bool player_fare=false;
    [SerializeField] private bool move=false;
    [SerializeField] private bool chase=false;
    void Start()
    {
        char_metadata.character_type = Damage_Origin.Ally;
        char_metadata.life = 100f;
        char_metadata.velocity = 13f;
        rb = transform.GetComponent<Rigidbody>();
        walking_audio = transform.Find("walk_player").GetComponent<AudioSource>();
        hit_sound = transform.Find("hit_player").GetComponent<AudioSource>();
        target_player = GameObject.FindWithTag("Player");
        animation_controller = transform.Find("ally/anim").gameObject.GetComponent<Animator>();
        weapon_changer = transform.Find("ally/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
        weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
        avilable_movements = new Dictionary<Vector3, float>();
        var delta_rad = 2*math.PI/number_of_rays;
        for(var rad_reference=0;rad_reference<number_of_rays;rad_reference++){
            avilable_movements[new Vector3(math.cos(rad_reference*delta_rad),0,math.sin(rad_reference*delta_rad))]=0;
        }
    }

    public character_info get_metadata(){return char_metadata;}
    public void set_char_metadata(character_info _chmeta){
        char_metadata.name=_chmeta.name;
        char_metadata.life=_chmeta.life;
        char_metadata.max_life=_chmeta.max_life;
        char_metadata.character_type=_chmeta.character_type;
        char_metadata.velocity=_chmeta.velocity;
        char_metadata.character_damage_type=_chmeta.character_damage_type;
        char_metadata.primary=_chmeta.primary;
        char_metadata.secondary=_chmeta.primary;
        char_metadata.ammo_primary=_chmeta.ammo_primary;
        char_metadata.ammo_secondary=_chmeta.ammo_secondary;
        transform.Find("Canvas/name").GetComponent<TMP_Text>().text = char_metadata.name;
        set_weapon(char_metadata.primary);
    }

    public void set_weapon(Weapon_type _weapon_type){
        if(weapon_changer==null)weapon_changer=transform.Find("ally/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
        char_metadata.primary = default_weapons.weapons[_weapon_type];
        if(char_metadata.primary.Wtype==Weapon_type.GUN||char_metadata.primary.Wtype==Weapon_type.FUSIL||char_metadata.primary.Wtype==Weapon_type.SUB_FUSIL){char_metadata.ammo_primary=UnityEngine.Random.Range(36,60);}
        weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
    }
    public void set_weapon(weapon_info _weapon){
        if(weapon_changer==null)weapon_changer=transform.Find("ally/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
        char_metadata.primary = _weapon;
        if(char_metadata.primary.Wtype==Weapon_type.GUN||char_metadata.primary.Wtype==Weapon_type.FUSIL||char_metadata.primary.Wtype==Weapon_type.SUB_FUSIL){char_metadata.ammo_primary=UnityEngine.Random.Range(36,60);}
        weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
    }
    
    private void FixedUpdate() {
        if(char_metadata.life>0){
            refresh_detection_status();
            char_metadata.position = transform.position;
        }
        player_fare = (target_player.transform.position-transform.position).magnitude>player_max_distance;
        if((target_player.transform.position-transform.position).magnitude>player_max_distance*6){
            transform.position=new Vector3(target_player.transform.position.x+UnityEngine.Random.Range(-5f,5f),1f,target_player.transform.position.z+UnityEngine.Random.Range(-5f,5f));
        }
        if(target==null&&chase){chase=false;}
    }

    void refresh_detection_status(){
        if(!no_life){
            RaycastHit temp_hit;
            List<UnityEngine.Vector3> temp_keys = new List<UnityEngine.Vector3>(avilable_movements.Keys);
            foreach(var key in temp_keys){
                if(Physics.Raycast(transform.position+Vector3.up, key, out temp_hit, detection_range)){
                    if(temp_hit.transform.tag == "static"){
                        avilable_movements[key]=-1;
                        Debug.DrawRay(transform.position+Vector3.up,key * detection_range, Color.red);
                    }else if(temp_hit.transform.tag == "enemy"){
                        avilable_movements[key]=number_of_rays/3;
                        if(temp_hit.distance< char_metadata.primary.attack_distance && !attacked){
                            attack_avilable = false;
                            attacked = true;
                            char_metadata.ammo_primary = weapon_changer.GetComponent<weapon_holder>().attack();
                            animation_controller.SetTrigger(default_weapons.weapon_anim[char_metadata.primary.Wtype]);
                            var tmp_vel = rb.velocity;
                            rb.velocity = tmp_vel*0.1f; 
                        }
                        Debug.DrawRay(transform.position+Vector3.up,key * detection_range, Color.green);
                    }else{
                        avilable_movements[key]=0;
                        Debug.DrawRay(transform.position+Vector3.up,key*detection_range, Color.white);
                    }
                }else{
                    avilable_movements[key]=0;
                    Debug.DrawRay(transform.position+Vector3.up,key*detection_range, Color.white);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.transform.tag==default_weapons.damge_tag[Damage_Origin.Enemy]){
            hit_sound.Play();
            var final_damage_ammount = 0f;
            // Debug.Log(other.transform.tag);
            if(other.transform.GetComponent<timed_ammo_autokill>()!=null){
                var tmp_cmp = other.transform.GetComponent<timed_ammo_autokill>();
                final_damage_ammount = tmp_cmp.get_damage_amount()*default_weapons.damage_multiplier(char_metadata.character_damage_type,tmp_cmp.get_dtype());
            }else if(other.transform.GetComponent<mele_foreward_damage>()!=null){
                var tmp_cmp = other.transform.GetComponent<mele_foreward_damage>();
                final_damage_ammount = tmp_cmp.get_damage_amount()*default_weapons.damage_multiplier(char_metadata.character_damage_type,tmp_cmp.get_dtype());
            }
            char_metadata.life-=final_damage_ammount;
            if(char_metadata.life<=0){
                no_life=true; 
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(!target && other.transform.tag=="enemy"){
            target = other.gameObject;
            chase = true;
            move = false;
            // transform.parent.transform.parent.GetComponent<enemy_group_manager>().target_broadcast(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform.tag=="enemy"){
            target = null;
            chase = false;
        }
    }

    #region behaviorActions

    private void timer_move(){
            time_waiter-=Time.deltaTime;
            if(time_waiter<0){
                time_waiter = time_spacing+UnityEngine.Random.Range(-0.5f,0.5f);
                move = true;
                direction_selected = UnityEngine.Random.Range(0,number_of_rays);
            }
        }

        private void timer_stop_moving(){
            time_waiter-=Time.deltaTime;
            if(time_waiter<0){
                time_waiter = time_spacing+UnityEngine.Random.Range(-0.5f,0.5f);
                move = false;
            }
        }

        private void timer_attack_reset(){
            time_waiter-=Time.deltaTime;
            if(time_waiter<0){
                time_waiter = char_metadata.primary.attack_tspacing+UnityEngine.Random.Range(-0.5f,0.5f);
                attack_avilable = true;
                attacked = false;
            }
        }

        private void stop_moving(){
            var tmp_vel = rb.velocity;
            rb.velocity = new Vector3(0,tmp_vel.y,0);
        }

        private void move_to(bool chase=false,bool player_too_fare=false){
            var dir = Vector3.zero;
            var ondistance = false;
            if(player_too_fare){
                if(player_fare){
                    dir = (target_player.transform.position - transform.position).normalized;
                }else{
                    ondistance = true;
                }
            }
            else if(chase){
                if(target!= null){
                    if((target.transform.position-transform.position).magnitude>char_metadata.primary.attack_distance){
                        dir = (target.transform.position - transform.position).normalized;
                    }else{
                        ondistance = true;
                    }
                }else{Debug.Log("chacing without target");chase=false;}
            }else{
                dir = avilable_movements.ElementAt(direction_selected).Key;
            }
            var tmp_vel = rb.velocity.normalized;
            var last_velocity = rb.velocity;
            foreach(var item in avilable_movements){
                tmp_vel+=item.Value*item.Key;
            }
            tmp_vel+=dir.normalized*number_of_rays/4;
            var velocity_setter = tmp_vel.normalized*char_metadata.velocity;
            last_velocity.x = velocity_setter.x*(ondistance?.2f:1f);
            last_velocity.z = velocity_setter.z*(ondistance?.2f:1f);
            if(velocity_setter.magnitude>0 && !is_moving){is_moving=true;animation_controller.SetInteger("state_indicator",1);}
            rb.velocity = last_velocity;
            dir_vel = tmp_vel;
            Debug.DrawRay(transform.position,tmp_vel,Color.blue);
        }

    #endregion

    #region states
    [State] private void EnterIdle(){
        is_moving = false;
        animation_controller.SetInteger("state_indicator",0);
        weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
        walking_audio.Stop();
    }
    [State] private void Idle(){
        timer_move();
    }
    [State] private void EnterPatrol(){walking_audio.Play();}
    [State] private void Patrol(){
        move_to();
        timer_stop_moving();
    }
    [State] private void EnterChace(){walking_audio.Play();}
    [State] private void Chace(){
        move_to(true,false);
    }
    [State] private void EnterFollowP(){walking_audio.Play();}
    [State] private void FollowP(){move_to(false,true);}
    [State] private void EnterAttack(){is_moving=false;animation_controller.SetInteger("state_indicator",0);walking_audio.Stop();}
    [State] private void Attack(){
        stop_moving();
        timer_attack_reset();
    }
    [State] private void EnterDie(){
        walking_audio.Stop();
        rb.isKinematic=true;
        animation_controller.SetTrigger("die");
        weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,default_weapons.weapons[Weapon_type.NONE],0);
        char_metadata.position = transform.position;
        Destroy(transform.gameObject,1f);
    }
    [State] private void Die(){}
    #endregion
    #region conditions
        [Condition] public bool chacing => chase;
        [Condition] public bool moving => move;
        [Condition] public bool attacking => attacked;
        [Condition] public bool death => no_life;
        [Condition] public bool goPlayer => player_fare;
    #endregion
}
}

