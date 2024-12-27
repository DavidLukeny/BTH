using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class config : MonoBehaviour
{
    // Start is called before the first frame update
    public static config instance;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        if(GameObject.FindGameObjectsWithTag("GameController").Length>1){Destroy(transform.gameObject);}
    }

    private config_data settings;
    private game_stat game_progress;
    [SerializeField] private AudioMixer audio_manager;
    [SerializeField] private string base_path_configs = "./savings";
    [SerializeField] private string base_path_games = "./games";
    [SerializeField] private string settings_file_name = "settings.json";
    [SerializeField] private List<string> game_files = new List<string>(){"game1.json","game2.json","game3.json"};
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource effects;
    [SerializeField] public int current_game=0;
    [SerializeField] private List<AudioClip> ui_button_sounds;
    [SerializeField] private List<AudioClip> background_sounds;
    void Start()
    {
        if(!loadConfig()){
            settings = new config_data(
                resolutions._1980x1080,
                1f,
                .1f,
                .5f
            );
            saveConfig();
        }
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 62;
    }

    public void set_master_volume(float volume){audio_manager.SetFloat("master_volume",volume);}
    public void set_music_volume(float volume){audio_manager.SetFloat("music_volume",volume);}
    public void set_effects_volume(float volume){audio_manager.SetFloat("effects_volume",volume);}
    public game_stat get_game_progress(){return game_progress;}
    public void add_money(int value){game_progress.money+=value;}
    public bool minus_money(int value){game_progress.money-=value;if(game_progress.money<0){game_progress.money+=value; return false;}return true;}
    public void update_player_status(character_info _player){game_progress.player = _player;}
    public void update_allys_status(List<character_info> allys){game_progress.allys = allys;}
    public void update_level_index(int index){game_progress.level_index = index;}
    public bool add_new_event_progress(string event_name){
        foreach(var itm in game_progress.events_progress){
            if(itm.Equals(event_name)){
                return false;
            }
        }
        game_progress.events_progress.Add(event_name);
        return true;
    }

    public void saveConfig(){
        string json = JsonUtility.ToJson(settings);
        if(Directory.Exists(base_path_configs)){
            File.WriteAllText(base_path_configs+"/"+settings_file_name,json);
        }else{
            Directory.CreateDirectory(base_path_configs);
            File.WriteAllText(base_path_configs+"/"+settings_file_name,json);
        }
    }

    
    public bool loadConfig(){
        if(Directory.Exists(base_path_configs)&&File.Exists(base_path_configs+"/"+settings_file_name)){
            var conf_str = File.ReadAllText(base_path_configs+"/"+settings_file_name);
            settings = JsonUtility.FromJson<config_data>(conf_str);

            Screen.SetResolution(
                (int)config_utils.screen_size[settings.resolution].x,
                (int)config_utils.screen_size[settings.resolution].y,
                false
            );
            audio_manager.SetFloat("master_volume",math.log10(settings.master_volume+0.01f)*20);
            audio_manager.SetFloat("music_volume",math.log10(settings.music_volume+0.01f)*20);
            audio_manager.SetFloat("effects_volume",math.log10(settings.effects_volume+0.01f)*20);
            return true;
        }else{
            Debug.Log("WARNING: no settings file to load");
            return false;
        }
    }

    public float get_master_volume(){
        return settings.master_volume;
    }
     
    public float get_music_volume(){
        return settings.music_volume;
    }

    public float get_effects_volume(){
        return settings.effects_volume;
    }

    public int get_resolution(){
        return (int)settings.resolution;
    }

    public bool change_resolution(resolutions _res){
        Debug.Log(_res);
        this.settings.resolution = _res;
        try{
            Screen.SetResolution(
                (int)config_utils.screen_size[_res].x,
                (int)config_utils.screen_size[_res].y,
                false
            );
            return true;
        }catch{
            Debug.Log("there is a problem thanging the resolution");
            return false;
        }
    }

    public bool change_master_volume(float value){
        try{
            settings.master_volume = value;
            audio_manager.SetFloat("master_volume",math.log10(value+0.01f)*20);
            return true;
        }catch{
            Debug.Log("canot able to set master volume");
            return false;
        }
    }

    public bool change_music_volume(float value){
        try{
            settings.music_volume = value;
            audio_manager.SetFloat("music_volume",math.log10(value+0.01f)*20);
            return true;
        }catch{
            Debug.Log("canot able to set music volume");
            return false;
        }
    }

    public bool change_effects_volume(float value){
        try{
            settings.effects_volume = value;
            audio_manager.SetFloat("effects_volume",math.log10(value+0.01f)*20);
            return true;
        }catch{
            Debug.Log("canot able to set effects volume");
            return false;
        }
    }

    public void play_music(AudioClip _clip){
        music.clip = _clip;
        music.Play();
    }

    public void play_random_music(){
        music.clip = background_sounds[UnityEngine.Random.Range(0,background_sounds.Count)];
        music.Play();
    }

    public bool check_playing(){
        return music.isPlaying;
    }

    public void play_ui_effect(){
        effects.clip = ui_button_sounds[UnityEngine.Random.Range(0,ui_button_sounds.Count)];
        effects.Play();
    }

    public void play_effect(AudioClip _clip){
        effects.clip = _clip;
        effects.Play();
    }

    public void new_game(int value){
        var g_file = game_files[value];
        game_progress = new game_stat();
        current_game = value;
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games);}
        if(!File.Exists(base_path_games+"/"+g_file)){
            var json = JsonUtility.ToJson(game_progress);
            File.WriteAllText(base_path_games+"/"+g_file,json);
        }
        SceneManager.LoadScene(scenes.stage_1.ToString());
    }

    public void save_game(int value){
        var g_file = game_files[value];
        current_game = value;
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games);}
        if(!File.Exists(base_path_games+"/"+g_file)){
            new_game(value);
            return;
        }
        var json = JsonUtility.ToJson(game_progress);
        Debug.Log(json);
        File.Delete(base_path_games+"/"+g_file);
        File.WriteAllText(base_path_games+"/"+g_file,json);
    }

    public bool check_game(int value){
        var g_file = game_files[value];
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games); return false;}
        return File.Exists(base_path_games+"/"+g_file);
    }

    public bool delete_game(int value){
        var g_file = game_files[value];
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games); return true;}
        try{
            File.Delete(base_path_games+"/"+g_file);
            return true;
        }catch{return false;}
    }

    public void load_game(int value){
        var g_file = game_files[value];
        current_game = value;
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games);}
        if(!File.Exists(base_path_games+"/"+g_file)){
            new_game(value);
        }else{
            var json_content = File.ReadAllText(base_path_games+"/"+g_file);
            game_progress = JsonUtility.FromJson<game_stat>(json_content);
        }
        SceneManager.LoadScene(((scenes)game_progress.level_index+2).ToString());
    }

    public void load_main_screen(){
        SceneManager.LoadScene(((scenes)0).ToString());
    }

    public game_stat get_game_metadata(int value){
        var g_file = game_files[value];
        if(!Directory.Exists(base_path_games)){Directory.CreateDirectory(base_path_games); return new game_stat();}
        if(!File.Exists(base_path_games+"/"+g_file)){
            new_game(value);
            return new game_stat();
        }else{
            var json_content = File.ReadAllText(base_path_games+"/"+g_file);
            var game_metadata = JsonUtility.FromJson<game_stat>(json_content);
            return game_metadata;
        }
    }

    enum scenes{
        main_menu,
        loading,
        stage_1
    };
}
