using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_obj_trigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private string event_name;
    [SerializeField] private int level;
    [SerializeField] private List<string> dialogs;
    [SerializeField] private GameObject game_manager;
    [SerializeField] private bool is_dialog=true;

    private void Start() {
        game_manager = GameObject.FindWithTag("GameController");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag.Equals("Player")){
            if(is_dialog){
                if(game_manager.GetComponent<config>().add_new_event_progress(event_name)){
                    GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().change_igui_dialog();
                    GameObject.FindWithTag("mngr_ui").transform.Find("dialogs").GetComponent<dialog_manager>().show_dialog(dialogs);
                }
            }else{
                if(game_manager.GetComponent<config>().add_new_event_progress(event_name)){
                    if(event_name.Equals("show last video")){
                        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().play_video(2);
                    }
                    if(event_name.Equals("show fight video")){
                        GameObject.FindWithTag("mngr_ui").GetComponent<main_ui_navigation>().play_video(1);
                    }
                }
            }
        }
    }

    public void trigger(){
        game_manager.GetComponent<config>().add_new_event_progress(event_name);
    }
}
