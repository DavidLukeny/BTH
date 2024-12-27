using System.Collections;
using System.Collections.Generic;
using RSM;
using UnityEngine;
using UnityEngine.Video;

public class video_player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<VideoClip> videos;
    [SerializeField] private GameObject ui;
    [SerializeField] private bool playing=false;
    [SerializeField] private float time_counter = 0f;
    [SerializeField] private float max_time = 0f;

    private void Start() {
        ui = GameObject.FindWithTag("mngr_ui").gameObject;
    }

    public void play(int video_index=0){
        playing = true;
        max_time = (float)videos[video_index].length;
        var tmp_panel = transform.Find("Panel");
        tmp_panel.GetComponent<VideoPlayer>().clip=videos[video_index];
        tmp_panel.GetComponent<VideoPlayer>().Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playing){
            time_counter+=Time.deltaTime;
            var tmp_panel = transform.Find("Panel");
            if(time_counter>=max_time){
                playing=false;
                max_time = 0f;
                time_counter=0f;
                tmp_panel.GetComponent<VideoPlayer>().Stop();
                ui.GetComponent<main_ui_navigation>().change_igui_hud();
            }
        }
    }
}
