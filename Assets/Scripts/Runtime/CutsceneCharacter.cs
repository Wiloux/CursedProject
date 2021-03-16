using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CutsceneCharacter : MonoBehaviour
{
    public Rig animationRig;
    public MultiAimConstraint headAimConstraint;

    public void SetHeadAimConstraints(float value)
    {
        if (headAimConstraint == null) return;
        headAimConstraint.weight = value;
    }

    public void SetAnimationRiggingWeight(float value) 
    {
        if (value < 0 || value > 1 || animationRig == null) return;
        animationRig.weight = value; 
    }
}
