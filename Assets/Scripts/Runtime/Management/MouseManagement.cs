using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManagement : MonoBehaviour
{
    public static MouseManagement instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleMouseLock()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
        else Cursor.lockState = CursorLockMode.Locked;
    }
}
