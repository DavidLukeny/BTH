using System.Collections;
using System.Collections.Generic;
using RSM;
using UnityEngine;

public class ui_sound_interface : MonoBehaviour
{
    [SerializeField] private AudioSource ui_player;

    private void Start() {
        ui_player = transform.Find("follow_player/ui_audio_player").GetComponent<AudioSource>();
    }

    public void play_ui_sound(){
        ui_player.Play();
    }
}
