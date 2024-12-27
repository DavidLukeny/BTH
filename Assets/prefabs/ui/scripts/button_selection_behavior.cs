using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.Jobs;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using Unity.VisualScripting;
using Unity.Mathematics;
using System;

public class button_selection_behavior : MonoBehaviour
{
    // Start is called before the first frame update
    private int normal_font_size=80;
    private Color selected_color= new Color(1,1,1,1);
    private int selected_font_size=100;
    private TMP_Text text_reference;
    private Color unselected_color= new Color(180f/255f,180f/255f,180f/255f,255);

    private GameObject gctl;
    [SerializeField] private string ctl_type;
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject delete_option;

    [SerializeField] private GameObject forced_update;

    void Start()
    {
        gctl = GameObject.FindWithTag("GameController");
        var tmp_txt = transform.Find("txt");
        if(tmp_txt){
            text_reference = tmp_txt.GetComponent<TMP_Text>();
        }
        load_auto_conf();
    }

    public void load_auto_conf(){
        try{
        var conf_vol = 0f;
        var conf_res = 0;
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                if(ctl_type!=""){
                    if(ctl_type.Equals("master")||ctl_type.Equals("music")||ctl_type.Equals("effects")){
                        
                        if(ctl_type.Equals("master")){conf_vol = mngr.get_master_volume();}
                        else if(ctl_type.Equals("music")){conf_vol = mngr.get_music_volume();}
                        else if(ctl_type.Equals("effects")){conf_vol = mngr.get_effects_volume();}
                        transform.GetComponent<Scrollbar>().value= conf_vol;
                    }else if(ctl_type.Equals("res")){
                        conf_res= mngr.get_resolution();
                        Debug.Log(conf_res);
                        transform.GetComponent<TMP_Dropdown>().value=conf_res;
                    }else if(ctl_type.Equals("partida_1")){
                        var money = transform.Find("dinero");
                        var aliados = transform.Find("aliados");
                        var progreso = transform.Find("progreso");
                        var tmp_message = transform.Find("gt_message");
                        if(mngr.check_game(0)){
                            var tmp_game_stat = mngr.get_game_metadata(0);
                            money.GetComponent<TMP_Text>().text = "Dinero: "+tmp_game_stat.money.ToSafeString();
                            aliados.GetComponent<TMP_Text>().text = "Aliados: "+ tmp_game_stat.allys.Count.ToSafeString();
                            progreso.GetComponent<TMP_Text>().text = "Progreso: "+ Math.Truncate(tmp_game_stat.events_progress.Count*100f/config_utils.standard_events.Count).ToSafeString()+"%";
                            tmp_message.GetComponent<TMP_Text>().text = "Continuar";
                        }else{
                            money.GetComponent<TMP_Text>().text = "Dinero: ";
                            aliados.GetComponent<TMP_Text>().text = "Aliados: ";
                            progreso.GetComponent<TMP_Text>().text = "Progreso: ";
                            tmp_message.GetComponent<TMP_Text>().text = "Nuevo Juego";
                            delete_option.SetActive(false);
                        }
                    }
                    else if(ctl_type.Equals("partida_2")){
                        var money = transform.Find("dinero");
                        var aliados = transform.Find("aliados");
                        var progreso = transform.Find("progreso");
                        var tmp_message = transform.Find("gt_message");
                        if(mngr.check_game(1)){
                            var tmp_game_stat = mngr.get_game_metadata(1);
                            money.GetComponent<TMP_Text>().text = "Dinero: "+tmp_game_stat.money.ToSafeString();
                            aliados.GetComponent<TMP_Text>().text = "Aliados: "+ tmp_game_stat.allys.Count.ToSafeString();
                            progreso.GetComponent<TMP_Text>().text = "Progreso: "+ Math.Truncate(tmp_game_stat.events_progress.Count*100f/config_utils.standard_events.Count).ToSafeString()+"%";
                            tmp_message.GetComponent<TMP_Text>().text = "Continuar";
                        }else{
                            money.GetComponent<TMP_Text>().text = "Dinero: ";
                            aliados.GetComponent<TMP_Text>().text = "Aliados: ";
                            progreso.GetComponent<TMP_Text>().text = "Progreso: ";
                            tmp_message.GetComponent<TMP_Text>().text = "Nuevo Juego";
                            delete_option.SetActive(false);
                        }
                    }
                    else if(ctl_type.Equals("partida_3")){
                        var money = transform.Find("dinero");
                        var aliados = transform.Find("aliados");
                        var progreso = transform.Find("progreso");
                        var tmp_message = transform.Find("gt_message");
                        if(mngr.check_game(2)){
                            var tmp_game_stat = mngr.get_game_metadata(2);
                            money.GetComponent<TMP_Text>().text = "Dinero: "+tmp_game_stat.money.ToSafeString();
                            aliados.GetComponent<TMP_Text>().text = "Aliados: "+ tmp_game_stat.allys.Count.ToSafeString();
                            progreso.GetComponent<TMP_Text>().text = "Progreso: "+ Math.Truncate(tmp_game_stat.events_progress.Count*100f/config_utils.standard_events.Count).ToSafeString()+"%";
                            tmp_message.GetComponent<TMP_Text>().text = "Continuar";
                        }else{
                            money.GetComponent<TMP_Text>().text = "Dinero: ";
                            aliados.GetComponent<TMP_Text>().text = "Aliados: ";
                            progreso.GetComponent<TMP_Text>().text = "Progreso: ";
                            tmp_message.GetComponent<TMP_Text>().text = "Nuevo Juego";
                            delete_option.SetActive(false);
                        }
                    }
                }
            }
        }
        }catch(Exception e){Debug.Log(e);}
    }

    private void OnEnable() {
        load_auto_conf();
    }
    public void selected(){
        try{
            text_reference.fontSize = selected_font_size;
            text_reference.color = selected_color;
            if(gctl){
                var mngr = gctl.GetComponent<config>();
                if(mngr){
                    mngr.play_ui_effect();
                }
            }
        }catch{}
    }

    public void unselected(){
        text_reference.fontSize = normal_font_size;
        text_reference.color = unselected_color;
    }

    public void change_ui_main(){
        message.SetActive(false);
        if(text_reference){
            text_reference.fontSize = normal_font_size;
            text_reference.color = unselected_color;
        }
        
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.loadConfig();
                mngr.play_ui_effect();
            }
        }
        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_ui_main();
    }

    public void change_ui_main_partidas(){
        if(text_reference){
            text_reference.fontSize = normal_font_size;
            text_reference.color = unselected_color;
        }
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
            }
        }
        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_ui_partidas();
    }

    
    public void change_ui_main_1(){
        if(text_reference){
            text_reference.fontSize = normal_font_size;
            text_reference.color = unselected_color;
        }
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
            }
        }
        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_ui_main_1();
    }

    public void save_changes(){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.saveConfig();
                mngr.play_ui_effect();
                message.SetActive(true);
            }
        }
    }

    
    public void change_ui_main_2(){
        if(text_reference){
            text_reference.fontSize = normal_font_size;
            text_reference.color = unselected_color;
        }
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
            }
        }
        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_ui_main_2();
    }

    
    public void change_ui_main_exit(){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
            }
        }
        Application.Quit();
    }

    public void change_pause_controlls(){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
            }
        }
        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_igui_pause_controlls();
    }

    public void change_resolution(int value){
        Debug.Log(value);
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.change_resolution((resolutions)value);
                mngr.play_ui_effect();
            }
        }
    }

    public void change_master_volume(float _volume){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.change_master_volume(_volume);
                mngr.play_ui_effect();
            }
        }
    }

    public void change_music_volume(float _volume){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.change_music_volume(_volume);
                mngr.play_ui_effect();
            }
        }
    }
    public void change_effects_volume(float _volume){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.change_effects_volume(_volume);
                mngr.play_ui_effect();
            }
        }
    }

    public void playgame(int value){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                if(mngr.check_game(value)){
                    mngr.load_game(value);
                }else{mngr.new_game(value);}
            }
        }
    }

    public void savegame(){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
                if(mngr.check_game(mngr.current_game)){
                    mngr.save_game(mngr.current_game);
                }else{mngr.new_game(mngr.current_game);}
                message.SetActive(true);
            }
        }
    }

    public void delete_game(int value){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
                if(mngr.delete_game(value)){
                    forced_update.GetComponent<button_selection_behavior>().load_auto_conf();
                    GameObject.FindGameObjectWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_ui_partidas();
                }
                
            }
        }
    }

    public void load_main_scene(){
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(mngr){
                mngr.play_ui_effect();
                mngr.load_main_screen();
            }
        }
    }

    public void change_text(){
        var txt_changer = transform.parent.transform.parent;
        if(!txt_changer.GetComponent<dialog_manager>().next()){
            GameObject.FindGameObjectWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_igui_hud();
        }
    }
}
