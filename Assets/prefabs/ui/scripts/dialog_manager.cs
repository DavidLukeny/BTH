using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialog_manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<string> text_pages;
    [SerializeField] private int current_index=0;
    void Start()
    {
    }

    public void show_dialog(List<string> _tmp_list){
        text_pages = _tmp_list;
        current_index=0;
        transform.Find("Panel/txt").GetComponent<TMP_Text>().text = text_pages[current_index];
    }

    public bool next(){
        current_index+=1;
        if(current_index<text_pages.Count){
            transform.Find("Panel/txt").GetComponent<TMP_Text>().text = text_pages[current_index];
            return true;
        }else{return false;}
    }
}
