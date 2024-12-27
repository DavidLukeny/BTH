using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hud_manager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject player_info;
    [SerializeField] private GameObject game_manager;
    [SerializeField] private List<GameObject> ally_info;

    [SerializeField] private List<Sprite> damage_types;
    void Start()
    {
        game_manager = GameObject.FindWithTag("GameController");
        player_info = transform.Find("player_info").gameObject;
        ally_info = new List<GameObject>{
            transform.Find("allys/ally1_info").gameObject,
            transform.Find("allys/ally2_info").gameObject,
            transform.Find("allys/ally3_info").gameObject,
            transform.Find("allys/ally4_info").gameObject,
            transform.Find("allys/ally5_info").gameObject,
            transform.Find("allys/ally6_info").gameObject,
        };
    }

    private void FixedUpdate() {
        update_player_info();
        update_allys_info();
    }

    public void update_player_info(){
        var game_stat = game_manager.GetComponent<config>().get_game_progress();
        var game_player_info = game_stat.player;
        var _money = game_stat.money;
        player_info.transform.Find("life").GetComponent<Slider>().value = game_player_info.life/game_player_info.max_life;
        player_info.transform.Find("life/life_info").GetComponent<TMP_Text>().text = game_player_info.life.ToString()+"/"+game_player_info.max_life.ToString();
        player_info.transform.Find("primary_weapon/ammo/ammo_info").GetComponent<TMP_Text>().text = game_player_info.ammo_primary.ToString();
        player_info.transform.Find("primary_weapon/damage/ammount").GetComponent<TMP_Text>().text = game_player_info.primary.damage.ToString();
        player_info.transform.Find("primary_weapon/type/type").GetComponent<TMP_Text>().text = "["+default_weapons.weapon_snames[game_player_info.primary.Wtype]+"]";
        player_info.transform.Find("secondary_weapon/ammo/ammo_info").GetComponent<TMP_Text>().text = game_player_info.ammo_secondary.ToString();
        player_info.transform.Find("secondary_weapon/damage/ammount").GetComponent<TMP_Text>().text = game_player_info.secondary.damage.ToString();
        player_info.transform.Find("secondary_weapon/type/type").GetComponent<TMP_Text>().text = "["+default_weapons.weapon_snames[game_player_info.secondary.Wtype]+"]";
        player_info.transform.Find("money/money_info").GetComponent<TMP_Text>().text = _money.ToString();
        player_info.transform.Find("damage_type/damage_type").GetComponent<Image>().sprite = damage_types[(int)game_player_info.character_damage_type];
    }

    public void update_allys_info(){
        var game_stat = game_manager.GetComponent<config>().get_game_progress();
        var game_allys_info = game_stat.allys;
        for(int index=0; index<6;index++){
            try{
                ally_info[index].transform.Find("Panel/name").GetComponent<TMP_Text>().text = game_allys_info[index].name;
                ally_info[index].transform.Find("Panel/life").GetComponent<Slider>().value = game_allys_info[index].life/game_allys_info[index].max_life;
                ally_info[index].transform.Find("Panel/ammo").GetComponent<Slider>().value = game_allys_info[index].ammo_primary/10;
                ally_info[index].transform.Find("Panel/damage_type").GetComponent<Image>().sprite = damage_types[(int)game_allys_info[index].character_damage_type];
                ally_info[index].SetActive(true);
            }catch{
                ally_info[index].SetActive(false);
            }
        }
    }
}
