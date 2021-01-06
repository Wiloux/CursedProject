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
    public AK.Wwise.Event Attack;
    // Use this for initialization..
    public void PlayAttack()
    {
        Attack.Post(gameObject);
    }
}
