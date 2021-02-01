using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationStuff : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyBase;
    public void MeleeAttack() { enemyBase.Attack(); }

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

    public AK.Wwise.Event Attack2Sound;
    // Use this for initialization..
    public void PlayAttack2()
    {
        Attack2Sound.Post(gameObject);
    }

    public AK.Wwise.Event Attack3Sound;
    // Use this for initialization..
    public void PlayAttack3()
    {
        Attack3Sound.Post(gameObject);
    }
    public AK.Wwise.Event Other1Sound;
    // Use this for initialization..
    public void PlayOther1()
    {
        Other1Sound.Post(gameObject);
    }
    public AK.Wwise.Event Other2Sound;
    // Use this for initialization..
    public void PlayOther2()
    {
        Other2Sound.Post(gameObject);
    }
}
