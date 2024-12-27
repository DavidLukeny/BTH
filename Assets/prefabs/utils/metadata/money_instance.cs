using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class money_instance : MonoBehaviour
{

    [SerializeField] private GameObject parent;
    [SerializeField] public int value;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        value = parent.GetComponent<money_metadata>().value;
        Destroy(transform.gameObject,20f);

    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.tag.Equals("Player")){
            parent.GetComponent<money_metadata>().destroy_parent();
        }
    }
}
