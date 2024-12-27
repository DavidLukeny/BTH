using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace RSM{
    public class enemy_behavior : MonoBehaviour,IStateBehaviour
    {
        [SerializeField] public Vector3 dir_vel;
        [SerializeField] private Animator animation_controller;
        [SerializeField] private character_info char_metadata;
        [SerializeField] private GameObject weapon_changer;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject broad_target;
        [SerializeField] private GameObject ally_drop;
        [SerializeField] private GameObject weapon_drop;
        [SerializeField] private AudioSource walk_sound;
        [SerializeField] private AudioSource hit_sound;
        [SerializeField] public weapon_info weapon_drop_info;
        [SerializeField] public int weapon_drop_ammo;
        [SerializeField] private GameObject manager;
        [SerializeField] private Rigidbody rb;  
        [SerializeField] private Dictionary<UnityEngine.Vector3,float> avilable_movements;
        [SerializeField] private int number_of_rays=8;
        [SerializeField] private float detection_range=1;
        [SerializeField] private int direction_selected = 0;
        [SerializeField] private float time_spacing=2;
        [SerializeField] private float time_waiter=2;
        [SerializeField] private float patroll_max_distance = 5;
        [SerializeField] private bool attack_avilable=true;
        [SerializeField] private bool attacked=false;
        [SerializeField] private bool is_moving = false;
        [SerializeField] private bool no_life = false;
        [SerializeField] private bool move = false;
        [SerializeField] private bool chase = false;
        void Start()
        {
            char_metadata.character_type = Damage_Origin.Enemy;
            char_metadata.life = 100f;
            char_metadata.velocity = 13f;
            rb = transform.GetComponent<Rigidbody>();
            walk_sound = transform.Find("walk_player").GetComponent<AudioSource>();
            hit_sound = transform.Find("hit_player").GetComponent<AudioSource>();
            manager = transform.parent.transform.parent.gameObject;
            weapon_drop = transform.parent.transform.parent.GetComponent<enemy_group_manager>().drop_weapon;
            weapon_drop_ammo = transform.parent.transform.parent.GetComponent<enemy_group_manager>().drop_ammo;
            weapon_drop_info = transform.parent.transform.parent.GetComponent<enemy_group_manager>().drop_info;
            animation_controller = transform.Find("enemy/anim").gameObject.GetComponent<Animator>();
            weapon_changer = transform.Find("enemy/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
            avilable_movements = new Dictionary<Vector3, float>();
            var delta_rad = 2*math.PI/number_of_rays;
            for(var rad_reference=0; rad_reference<number_of_rays; rad_reference++){
                avilable_movements[new Vector3(math.cos(rad_reference*delta_rad),0,math.sin(rad_reference*delta_rad))]=0;
            }
            transform.Find("ui/Canvas/life").GetComponent<Slider>().value = char_metadata.life/char_metadata.max_life;
        }

        public character_info get_metadata(){return char_metadata;}

        public void set_life(int life){
            char_metadata.life = life;
            char_metadata.max_life = life;
        }

        public void set_weapon(Weapon_type _weapon_type){
            if(weapon_changer==null)weapon_changer = transform.Find("enemy/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
            char_metadata.primary = default_weapons.weapons[_weapon_type];
            if(char_metadata.primary.Wtype==Weapon_type.GUN||char_metadata.primary.Wtype==Weapon_type.FUSIL||char_metadata.primary.Wtype==Weapon_type.SUB_FUSIL){char_metadata.ammo_primary=UnityEngine.Random.Range(36,60);}
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
        }
        public void set_weapon(weapon_info _weapon){
            if(weapon_changer==null)weapon_changer = transform.Find("enemy/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
            char_metadata.primary = _weapon;
            if(char_metadata.primary.Wtype==Weapon_type.GUN||char_metadata.primary.Wtype==Weapon_type.FUSIL||char_metadata.primary.Wtype==Weapon_type.SUB_FUSIL){char_metadata.ammo_primary=UnityEngine.Random.Range(36,60);}
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_secondary);
        }

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
            set_weapon(char_metadata.primary);
        }

        public void set_wdrop(GameObject _wdrop,weapon_info _wdrop_info){weapon_drop = _wdrop; weapon_drop_info = _wdrop_info;}

        void FixedUpdate(){
            if(char_metadata.life>0){
                refresh_detection_status();
                char_metadata.position = transform.position;
            }
            if(transform.position.y<0){transform.position=new Vector3(transform.position.x,2f,transform.position.z);}
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
                        }else if(temp_hit.transform.tag == "player_attack" || temp_hit.transform.tag == "Ally"){
                            avilable_movements[key]=number_of_rays/3;
                            if(temp_hit.distance< char_metadata.primary.attack_distance && !attacked){
                                attack_avilable = false;
                                attacked = true;
                                char_metadata.ammo_primary = weapon_changer.GetComponent<weapon_holder>().attack();
                                animation_controller.SetTrigger(default_weapons.weapon_anim[char_metadata.primary.Wtype]);
                                var tmp_vel = rb.velocity;
                                rb.velocity = tmp_vel*0.1f; 
                            }
                            Debug.DrawRay(transform.position+Vector3.up,key * detection_range, Color.yellow);
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

        public void set_detection_range(float range){
            detection_range *= range;
        }

        private void OnTriggerEnter(Collider other){
            if(other.transform.tag==default_weapons.damge_tag[Damage_Origin.Ally]){
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
                transform.Find("ui/Canvas/life").GetComponent<Slider>().value = char_metadata.life/char_metadata.max_life;
                if(char_metadata.life<=0){
                    no_life=true; 
                }
            }
        }
        private void OnTriggerStay(Collider other) {
            if(!target && other.transform.tag=="Player" ||  other.transform.tag=="Ally" && broad_target==null){
                target = other.gameObject;
                chase = true;
                move = false;
                transform.parent.transform.parent.GetComponent<enemy_group_manager>().target_broadcast(other.gameObject);
            }
        }

        public void set_broad_target(GameObject _broad_target){
            broad_target = _broad_target;
            chase = true;
            move = false;
        }

        private void OnTriggerExit(Collider other) {
            if(other.transform.tag=="Player"){
                target = null;
                chase = false;
                broad_target=null;
            }
        }
        #region behavior_actions
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

        private void move_to(bool chace=false){
            var dir = Vector3.zero;
            var manager_position_dif = manager.transform.position - transform.position;
            var ondistance = false;
            if(chace){
                if(target!= null){
                    if((target.transform.position-transform.position).magnitude>char_metadata.primary.attack_distance){
                        dir = (target.transform.position - transform.position).normalized;
                    }else{
                        ondistance = true;
                    }
                }
                else if(broad_target!= null){
                    if((broad_target.transform.position-transform.position).magnitude>char_metadata.primary.attack_distance){
                        dir = (broad_target.transform.position - transform.position).normalized;
                    }
                }
                else{Debug.Log("chacing without target");chace=false;}
            }else{
                dir = avilable_movements.ElementAt(direction_selected).Key;
            }
            var tmp_vel = rb.velocity.normalized;
            var last_velocity = rb.velocity;
            foreach(var item in avilable_movements){
                tmp_vel+=item.Value*item.Key;
            }
            tmp_vel+=dir.normalized*number_of_rays/4;
            if(!chace)tmp_vel+=manager_position_dif*(.9f/patroll_max_distance);
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
            is_moving=false;
            animation_controller.SetInteger("state_indicator",0);
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
            walk_sound.Stop();
        }
        [State] private void Idle(){
            timer_move();
        }

        [State] private void EnterPatroll(){walk_sound.Play();}
        [State] private void Patroll(){
            move_to();
            timer_stop_moving();
        }

        [State] private void EnterChace(){walk_sound.Play();}
        [State] private void Chace(){
            move_to(true);
        }

        [State] private void EnterAttack(){is_moving=false;animation_controller.SetInteger("state_indicator",0);walk_sound.Stop();}
        [State] private void Atack(){
            stop_moving();
            timer_attack_reset();
        }
        [State] private void ExitAtack(){
        }

        [State] private void EnterDie(){
            walk_sound.Stop();
            rb.isKinematic=true;
            animation_controller.SetTrigger("die");
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,default_weapons.weapons[Weapon_type.NONE],0);
            char_metadata.position = transform.position;
            transform.parent.transform.parent.GetComponent<enemy_group_manager>().kill_instance(char_metadata.name,transform.position);
            if(UnityEngine.Random.Range(0f,1f)<0.3f){
                Instantiate(ally_drop,transform.position,Quaternion.identity,transform);
            }

            if(UnityEngine.Random.Range(0f,1f)<0.1f){
                Instantiate(weapon_drop,new Vector3(transform.position.x+UnityEngine.Random.Range(-0.5f,0.5f),1f,transform.position.z+UnityEngine.Random.Range(-0.5f,0.5f)),Quaternion.identity,transform);
            }
            Destroy(transform.gameObject,6f);
        }
        [State] private void Die(){

        }
        #endregion
        #region conditions
        [Condition] public bool chacing => chase;
        [Condition] public bool moving => move;
        [Condition] public bool attacking => attacked;
        [Condition] public bool death => no_life;
        #endregion
    }
}