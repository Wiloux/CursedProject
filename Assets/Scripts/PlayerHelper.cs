using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public Player_Movement playerMovement;
    public static PlayerHelper instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }


    public void ToggleControls() {
        playerMovement.stopControlls = !playerMovement.stopControlls;
    }
}
