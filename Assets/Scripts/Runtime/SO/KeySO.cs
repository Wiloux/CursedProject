using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Key")]
public class KeySO : ObjectSO
{
    public enum KeyColor { red, green, blue, white, black};

    public new string name;
    public KeyColor color;
}
