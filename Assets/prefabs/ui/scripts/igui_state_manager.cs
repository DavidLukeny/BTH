using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class igui_state_manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private VideoClip init;
    [SerializeField] private VideoClip fight;
    [SerializeField] private VideoClip end;
    [SerializeField] private int current_ui;
    [SerializeField] private List<GameObject> uis;

    public void play_video(){
        uis[current_ui].SetActive(false);
        current_ui = 1;
        uis[current_ui].SetActive(false);

    }
}
