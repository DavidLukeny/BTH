using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_parent_pos : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject parent;
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var parent_pos = parent.transform.position;
        transform.position = new Vector3(parent_pos.x,1f,parent_pos.z);
    }
}
