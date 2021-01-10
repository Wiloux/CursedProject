using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationStuff : MonoBehaviour
{
    public AK.Wwise.Event Footsteps;
    // Use this for initialization..
    public void PlayFootsteps()
    {
        Footsteps.Post(gameObject);
    }
    public AK.Wwise.Event Attack1;
    // Use this for initialization..
    public void PlayAttack1()
    {
        Attack1.Post(gameObject);
    }
    public AK.Wwise.Event Attack2;
    // Use this for initialization..
    public void PlayAttack2()
    {
        Attack2.Post(gameObject);
    }
    public AK.Wwise.Event Attack3;
    // Use this for initialization..
    public void PlayAttack3()
    {
        Attack3.Post(gameObject);
    }
}
