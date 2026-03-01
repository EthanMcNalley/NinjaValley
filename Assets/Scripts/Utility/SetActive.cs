using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject gm;
    public void SetActiveForAnimator(int option)
    {
        if (option == 0)
        {
            gameObject.SetActive(false);
        } else if (option == 1)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetActiveForAnimatiorGm(int option)
    {
        if (option == 0)
        {
            gm.SetActive(false);
        } 
        else if (option == 1) 
        { 
            gm.SetActive(true);
        }
    }
}
