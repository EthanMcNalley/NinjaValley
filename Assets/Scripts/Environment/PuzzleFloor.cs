using UnityEngine;

public class PuzzleFloor : MonoBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED)
        {
            anim.speed = TimeManager.slowed_amount;
        }
        else
        {
            anim.speed = 1f;
        }
    }
}
