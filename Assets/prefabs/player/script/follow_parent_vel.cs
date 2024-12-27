using UnityEngine;

public class follow_parent_vel : MonoBehaviour
{
    private Rigidbody p_ref;
    private Vector3 current_direction;
    private void Start() {
        p_ref = transform.parent.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        current_direction = ((current_direction+p_ref.velocity)/2).normalized; 
        current_direction = new Vector3(current_direction.x,0,current_direction.z).normalized;
        if(current_direction.magnitude>0){
            transform.rotation = Quaternion.LookRotation(current_direction, Vector3.up);
        }
    }
}
