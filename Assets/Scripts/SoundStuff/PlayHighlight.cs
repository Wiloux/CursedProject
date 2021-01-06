using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHighlight : MonoBehaviour
{
    public void OnPointerEnter()
    {
        AkSoundEngine.PostEvent("PlayHighlight", gameObject);
    }
}
