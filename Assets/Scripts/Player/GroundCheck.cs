using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool is_grounded = true;

    void Update(){
        //Debug.Log(is_grounded);
    }
    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.layer != 3){
            is_grounded = true;
        }
    }

    void OnTriggerExit(Collider collision){
        if (collision.gameObject.layer != 3){
            is_grounded = false;
        }
    }
}
