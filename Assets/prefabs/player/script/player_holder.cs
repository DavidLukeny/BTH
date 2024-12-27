using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace RSM{
    public class player_holder : MonoBehaviour,IStateBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Animator animation_controller;
        [SerializeField] private character_info char_metadata;
        [SerializeField] private GameObject weapon_changer;
        [SerializeField] private GameObject camera_holder;
        [SerializeField] private GameObject game_manager;
        [SerializeField] private GameObject ui_manager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private AudioSource walk_sound;
        [SerializeField] private AudioSource take_damage_sound;
        [SerializeField] private float h_move_action = 0;
        [SerializeField] private float v_move_action = 0;
        [SerializeField] private float attack_waiter = 0f;
        [SerializeField] private bool interaction_avilable=false;
        [SerializeField] private bool interaction_action=false;
        [SerializeField] private bool quit_action=false;
        [SerializeField] private bool attack_action_p=false;
        [SerializeField] private bool attack_action_s=false;
        [SerializeField] private bool attack_action=false;
        [SerializeField] private bool rotate_camara_left=false;
        [SerializeField] private bool rotate_camara_right=false;
        [SerializeField] private bool _interacting=false;
        [SerializeField] private bool no_life=false;
        void Start()
        {
            rb = transform.GetComponent<Rigidbody>();
            animation_controller = transform.Find("player/anim").gameObject.GetComponent<Animator>();
            weapon_changer = transform.Find("player/anim/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/attacher").gameObject;
            camera_holder= GameObject.FindGameObjectWithTag("mngr_camera").gameObject;
            game_manager = GameObject.FindWithTag("GameController");
            ui_manager = GameObject.FindWithTag("mngr_ui");
            take_damage_sound = transform.Find("hit_player").GetComponent<AudioSource>();
            walk_sound = transform.Find("walk_player").GetComponent<AudioSource>();
            set_last_status();
        }
        void FixedUpdate(){get_inputs();attack_behavior();rotate_camera();
            char_metadata.position = new UnityEngine.Vector3(transform.position.x,0f,transform.position.z);
            game_manager.GetComponent<config>().update_player_status(char_metadata);
            if((char_metadata.primary.Wtype==Weapon_type.GUN||char_metadata.primary.Wtype==Weapon_type.SUB_FUSIL||char_metadata.primary.Wtype==Weapon_type.FUSIL)&&char_metadata.ammo_primary<=0){
                char_metadata.primary=default_weapons.weapons[Weapon_type.SWORD];
                weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary);
            }
        }

        public UnityEngine.Vector3 get_move_dir(){
            return new UnityEngine.Vector3(h_move_action,0,v_move_action).normalized*char_metadata.velocity;
        }

        public void set_last_status(){
            var last_player_info = game_manager.GetComponent<config>().get_game_progress().player;
            char_metadata = last_player_info;
            transform.position = new UnityEngine.Vector3(char_metadata.position.x,1f,char_metadata.position.z);
            weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary);
        }

        void get_inputs(){
            v_move_action = Input.GetAxis("Vertical");
            h_move_action = Input.GetAxis("Horizontal");
            interaction_action = Input.GetKeyDown(KeyCode.KeypadEnter);
            quit_action = Input.GetKeyDown(KeyCode.Escape);
            attack_action_p = Input.GetKeyDown("h");
            attack_action_s = Input.GetKeyDown("u");
            rotate_camara_left = Input.GetKeyDown("g");
            rotate_camara_right = Input.GetKeyDown("y");
            if(quit_action){
                char_metadata.position =  new UnityEngine.Vector3(transform.position.x,0f,transform.position.z);
                game_manager.GetComponent<config>().update_player_status(char_metadata);
                ui_manager.GetComponent<main_ui_navigation>().change_igui_pause();
            }
        }

        private void change_dtype(Damage_Type _dtype){
            char_metadata.character_damage_type = _dtype;
        }

        void attack_behavior(){
            if((attack_action_p||attack_action_s)&&!attack_action){attack_action=true;}
        }

        void rotate_camera(){
            if(rotate_camara_left^rotate_camara_right){
                if(rotate_camara_left){
                    camera_holder.transform.GetComponent<follow_players>().setRotate(1,90);
                }else{
                    camera_holder.transform.GetComponent<follow_players>().setRotate(-1,90);
                }
            }
        }

        void update_velocity(){
            var movement = new UnityEngine.Vector3(h_move_action,0,v_move_action).normalized;
            var tmp_vel = rb.velocity.normalized;
            tmp_vel.x = Mathf.Clamp(tmp_vel.x*0.1f+movement.x,-char_metadata.velocity,char_metadata.velocity);
            tmp_vel.z = Mathf.Clamp(tmp_vel.z*0.1f+movement.z,-char_metadata.velocity,char_metadata.velocity);
            rb.velocity = tmp_vel.normalized*char_metadata.velocity;
        }

        void damp_velocity(){
            var tmp_vel = rb.velocity;
            tmp_vel.x *= 0.1f;
            tmp_vel.z *= 0.1f;
            rb.velocity = tmp_vel;
        }

        void check_interaction(){
            if(interaction_action^quit_action){
                if(interaction_action && interaction_avilable)_interacting=true;
                else if(_interacting && quit_action)_interacting=false;
            }
        }

        private void timer_attack(){
            attack_waiter-=Time.deltaTime;
            if(attack_waiter<=0){
                attack_action=false;
            }
        }
        private void OnTriggerEnter(Collider other) {
            if(other.transform.tag==default_weapons.damge_tag[Damage_Origin.Enemy]){
                take_damage_sound.Play();
                var final_damage_ammount = 0f;
                if(other.transform.GetComponent<timed_ammo_autokill>()!=null){
                    var tmp_cmp = other.transform.GetComponent<timed_ammo_autokill>();
                    final_damage_ammount = tmp_cmp.get_damage_amount()*default_weapons.damage_multiplier(char_metadata.character_damage_type,tmp_cmp.get_dtype());
                }else if(other.transform.GetComponent<mele_foreward_damage>()!=null){
                    var tmp_cmp = other.transform.GetComponent<mele_foreward_damage>();
                    final_damage_ammount = tmp_cmp.get_damage_amount()*default_weapons.damage_multiplier(char_metadata.character_damage_type,tmp_cmp.get_dtype());
                }
                char_metadata.life-=(int)final_damage_ammount;
                if(char_metadata.life<=0){
                    no_life = true;
                    // gamemanager.end_game()
                    // canvas_ui.end_game()
                }
            }else if(other.transform.tag=="money"){
                var value = other.GetComponent<money_instance>().value;
                game_manager.GetComponent<config>().add_money(value);
            }else if(other.transform.tag=="drop"){
                Debug.Log("weapon_drop!");
                var wdrop = other.GetComponent<weapon_meta_drop>().get_weapon_info();
                var drop_ammo = 0;
                if(wdrop.Wtype==Weapon_type.GUN||wdrop.Wtype==Weapon_type.SUB_FUSIL||wdrop.Wtype==Weapon_type.FUSIL){
                    drop_ammo = other.GetComponent<weapon_meta_drop>().get_ammo();
                }
                char_metadata.secondary = char_metadata.primary;
                char_metadata.ammo_secondary = char_metadata.ammo_primary;
                char_metadata.primary = wdrop;
                char_metadata.ammo_primary = drop_ammo;
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.transform.tag=="interactable"){interaction_avilable=true;}
        }
        private void OnTriggerExit(Collider other) {
            if(other.transform.tag=="interactable"){interaction_avilable=false;_interacting=false;}
        }

        #region  states
        [State] private void EnterIdle(){animation_controller.SetInteger("state_indicator",0);}
        [State] private void Idle(){
            damp_velocity();
            check_interaction();
            walk_sound.Stop();
        }

        [State] private void EnterMoving(){
            animation_controller.SetInteger("state_indicator",1);
            walk_sound.Play();
        }
        [State] private void Moving(){
            update_velocity();
            check_interaction();
        }
        [State] private void ExitMoving(){
        }
        [State] private void EnterInteracting(){animation_controller.SetTrigger("interacting");}
        [State] private void Interacting(){
            check_interaction();
        }
        [State] private void ExitInteracting(){}

        [State] private void EnterAttack(){
            if(attack_action_p){
                weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.primary,char_metadata.ammo_primary);
                animation_controller.SetTrigger(default_weapons.weapon_anim[char_metadata.primary.Wtype]);
                attack_waiter = char_metadata.primary.attack_tspacing;
                char_metadata.ammo_primary= weapon_changer.GetComponent<weapon_holder>().attack();
            }else if(attack_action_s){
                weapon_changer.GetComponent<weapon_holder>().change_weapon(char_metadata.character_type,char_metadata.character_damage_type,char_metadata.secondary,char_metadata.ammo_secondary);
                animation_controller.SetTrigger(default_weapons.weapon_anim[char_metadata.secondary.Wtype]);
                attack_waiter = char_metadata.secondary.attack_tspacing;
                char_metadata.ammo_secondary= weapon_changer.GetComponent<weapon_holder>().attack();
            }       
        }
        [State] private void Attack(){
            timer_attack();
        }

        [State] private void ExitAttack(){}

        [State] private void EnterDeath(){
            walk_sound.Stop();
            rb.velocity=new UnityEngine.Vector3(0f,0f,0f);
            animation_controller.SetTrigger("die");
            ui_manager.GetComponent<main_ui_navigation>().change_igui_gameover();
            var tmp_game_master = game_manager.GetComponent<config>();
            tmp_game_master.delete_game(tmp_game_master.current_game);
        }
        [State] private void Death(){}
        #endregion

        #region  Triggers
        
        [Trigger] public bool _Interaction_;
        
        #endregion

        #region confitions
        [Condition] public bool hmoving => h_move_action != 0 || v_move_action!=0;
        [Condition] public bool interact => _interacting;
        [Condition] public bool attack => attack_action;
        [Condition] public bool death=> no_life;
        
        
        #endregion
    }
}

