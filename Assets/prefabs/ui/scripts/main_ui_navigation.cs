using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class main_ui_navigation : MonoBehaviour
{
    [SerializeField] private int current;
    [SerializeField] private int current_submenu;
    [SerializeField] private List<GameObject> main;
    [SerializeField] private List<GameObject> pause_submenus;
    [SerializeField] private List<GameObject> first_selected;
    [SerializeField] private List<GameObject> ig_first_selected;
    [SerializeField] private GameObject e_sys;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        current_submenu=0;
        main[current].SetActive(true);
        var new_game_event = false;
        for(int i=1;i<main.Count;i++){
            main[i].SetActive(false);
        }
        var  gctl = GameObject.FindWithTag("GameController");
        if(gctl){
            gctl.GetComponent<config>().play_random_music();
            var game_progress = gctl.GetComponent<config>().get_game_progress();
            if(game_progress!=null && game_progress.events_progress.Count==0){
                // Debug.Log("NEW GAME!!");
                new_game_event = true;
            }
        }
        try{
            if(new_game_event){
                gctl.GetComponent<config>().add_new_event_progress("new begining");
                Time.timeScale=1;
                play_video();
            }else{
                change_igui_hud();
            }
        }catch{}
    }

    void FixedUpdate(){
        var  gctl = GameObject.FindWithTag("GameController");
        if(gctl){
            var mngr = gctl.GetComponent<config>();
            if(!mngr.check_playing())mngr.play_random_music();
        }
    }

    public void change_ui_main(){
        main[current].SetActive(false);
        current = 0;
        main[current].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(first_selected[current]);
    }

    public void change_ui_partidas(){
        main[current].SetActive(false);
        current = 1;
        main[current].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(first_selected[current]);

    }

    public void change_ui_main_1(){
        main[current].SetActive(false);
        current = 2;
        main[current].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(first_selected[current]);

    }

    public void change_ui_main_2(){
        main[current].SetActive(false);
        current = 3;
        main[current].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(first_selected[current]);

    }

    public void change_igui_pause(){
        main[current].SetActive(false);
        current = 1;
        main[current].SetActive(true);
        main[current].transform.Find("Panel/game_saved").gameObject.SetActive(false);
        reset_pause_menu();
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(ig_first_selected[0]);
        Time.timeScale = 0;
    }
    public void reset_pause_menu(){
        pause_submenus[current_submenu].SetActive(false);
        current_submenu = 0;
        pause_submenus[current_submenu].SetActive(true);
    }
    public void change_igui_hud(){
        reset_pause_menu();
        main[current].SetActive(false);
        current = 0;
        main[current].SetActive(true);
        Time.timeScale = 1;
    }
    public void change_igui_pause_configs(){
        main[current].SetActive(false);
        current = 1;
        main[current].SetActive(true);
        pause_submenus[current_submenu].SetActive(false);
        current_submenu = 1;
        pause_submenus[current_submenu].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(ig_first_selected[1]);

    }
    public void change_igui_pause_controlls(){
        main[current].SetActive(false);
        current = 1;
        main[current].SetActive(true);
        pause_submenus[current_submenu].SetActive(false);
        current_submenu = 2;
        pause_submenus[current_submenu].SetActive(true);
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(ig_first_selected[2]);

    }

    public void play_video(int video_index = 0){
        main[current].SetActive(false);
        current = 2;
        main[current].SetActive(true);
        reset_pause_menu();
        // Time.timeScale = 0;
        main[current].GetComponent<video_player>().play(video_index);
    }

    public void change_igui_gameover(){
        main[current].SetActive(false);
        current = 3;
        main[current].SetActive(true);
        Time.timeScale = 0;
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(ig_first_selected[3]);

    }

    public void change_igui_dialog(){
        main[current].SetActive(false);
        current = 4;
        main[current].SetActive(true);
        Time.timeScale = 0;
        e_sys.GetComponent<EventSystem>().SetSelectedGameObject(ig_first_selected[4]);
    }
}
