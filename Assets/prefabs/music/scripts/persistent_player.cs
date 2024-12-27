using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistent_player : MonoBehaviour
{
    public static persistent_player instance;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        if(GameObject.FindGameObjectsWithTag("sound_player").Length>1){Destroy(transform.gameObject);}
    }
}
