using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public static PlayerHelper instance;
    public Player_Movement playerMovement;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }


    public void StopControls() {
        playerMovement.stopControlls = !playerMovement.stopControlls;
    }
}
