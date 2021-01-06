using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public AK.Wwise.Event FootstepSound1;
    // Use this for initialization.
    void PlayFootstepSound1()
    {
        FootstepSound1.Post(gameObject);
    }
    public AK.Wwise.Event FootstepSound2;
    // Use this for initialization.
    void PlayFootstepSound2()
    {
        FootstepSound2.Post(gameObject);
    }
    public AK.Wwise.Event FootstepSound3;
    // Use this for initialization.
    void PlayFootstepSound3()
    {
        FootstepSound3.Post(gameObject);
    }
    public AK.Wwise.Event FootstepSound4;
    // Use this for initialization.
    void PlayFootstepSound4()
    {
        FootstepSound4.Post(gameObject);
    }
    public AK.Wwise.Event FootstepSoundKiller;
    // Use this for initialization.
    void PlayFootstepSoundKiller()
    {
        FootstepSoundKiller.Post(gameObject);
    }
}