using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class config_data
{
    public resolutions resolution;
    public float master_volume;
    public float music_volume;
    public float effects_volume;

    public config_data(resolutions _res,float _mtr_volume, float _ms_volume, float _e_volume){
        this.resolution = _res;
        this.master_volume = _mtr_volume;
        this.music_volume = _ms_volume;
        this.effects_volume = _e_volume;
    }
}


[Serializable]
public class game_stat{
    public int money;
    public int level_index;
    public character_info player;
    public List<character_info> allys;
    public List<string> events_progress;
    public game_stat(){
        this.money = 0;
        this.level_index = 0;
        this.player = Ally.init_player();
        this.allys = new List<character_info>();
        this.events_progress = new List<string>();
    }
}

[Serializable]
public class config_utils{
    public static Dictionary<resolutions,Vector2> screen_size=new Dictionary<resolutions, Vector2>{
        {resolutions._1980x1080,new Vector2(1980,1080)},
        {resolutions._1366x768,new Vector2(1366,768)},
        {resolutions._1280x720,new Vector2(1280,720)}
    };
    public static Dictionary<string,event_info> standard_events= new Dictionary<string, event_info>{
        {"new begining",new event_info("new begining",1)},
        {"new begining tutorial1",new event_info("new begining tutorial1",1)},
        {"new begining tutorial2",new event_info("new begining tutorial2",1)},
        {"new begining tutorial3",new event_info("new begining tutorial3",1)},
        {"first kill",new event_info("first kill",1)},
        {"first buy",new event_info("first buy",1)},
        {"first town boss",new event_info("first town boss",1)},
        {"first level boss",new event_info("first level boss",1)},
        {"mele weapon upgrade 1",new event_info("mele weapon upgrade 1",1)},
        {"mele weapon upgrade 2",new event_info("mele weapon upgrade 2",1)},
        {"distance weapon upgrade 1",new event_info("distance weapon upgrade 1",1)},
        {"distance weapon upgrade 2",new event_info("distance weapon upgrade 2",1)},
        {"new adult",new event_info("new adult",1)},
    };
}

[Serializable]
public enum resolutions{
    _1980x1080,
    _1366x768,
    _1280x720
};

[Serializable]
public struct event_info{
    public string name;
    public int level;
    public event_info(string _name,int _level_index){
        this.name = _name;
        this.level = _level_index;
    }
};
