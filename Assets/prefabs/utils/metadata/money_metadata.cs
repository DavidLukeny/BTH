using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class money_metadata : MonoBehaviour
{
    [SerializeField] public int value=5;
    [SerializeField] private List<GameObject> render_obj;
    private GameObject child_render;
    private void Start() {
        render();
    }

    private void render(){
        if(value<20){
            child_render=Instantiate(render_obj[0],transform.position,Quaternion.identity,transform);
        }else if(value<100){
            child_render=Instantiate(render_obj[1],transform.position,Quaternion.identity,transform);
        }else if(value<500){
            child_render=Instantiate(render_obj[2],transform.position,Quaternion.identity,transform);
        }else if(value<2000){
            child_render=Instantiate(render_obj[3],transform.position,Quaternion.identity,transform);
        }else if(value<8000){
            child_render=Instantiate(render_obj[4],transform.position,Quaternion.identity,transform);
        }else if(value<32000){
            child_render=Instantiate(render_obj[5],transform.position,Quaternion.identity,transform);
        }
    }

    public int get_value(){
        return value;
    }

    public void set_value(int _value){
        if(child_render){Destroy(child_render);}
        value = _value;
        render();
    }

    public void destroy_parent(){
        Destroy(transform.gameObject);
    }
}


