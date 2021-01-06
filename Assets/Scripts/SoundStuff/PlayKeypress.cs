using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayKeypress : MonoBehaviour
{
    public void onClick()
    {
        AkSoundEngine.PostEvent("PlayKeypress", gameObject);
    }
}
