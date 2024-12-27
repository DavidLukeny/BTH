using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ingame_nav_manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject g_manager;
    [SerializeField] private EventSystem e_sys;
    [SerializeField] private List<GameObject> focus_objs;
    void Start()
    {
        
    }
    public void set_state(){

    }
}

public enum igui_state{
    hud,
    pause_main,
    pause_config,
    pause_controlls,
    game_over
};
