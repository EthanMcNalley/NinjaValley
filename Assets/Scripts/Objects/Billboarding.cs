using UnityEngine;

public class Billboarding : MonoBehaviour
{

    void LateUpdate(){
        transform.forward = Camera.main.transform.forward;
    }
}
