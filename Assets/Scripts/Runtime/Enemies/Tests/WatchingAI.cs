using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchingAI : EnemyBaseAI
{
    private enum FaceState
    {
        Hiding,
        Showing
    }
    private FaceState faceState;
    [SerializeField] private float showingFaceDuration;

    private float faceTimer;

    private Action StartShowingFace;
    private Action StopShowingFace;

    #region Wwise Events
    [Space(10)]
    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event startShowingFaceWEvent;
    [SerializeField] private AK.Wwise.Event stopShowingFaceWEvent;
    #endregion

    // Start is called before the first frame update
    public override void LaunchActions()
    {
        switch (state)
        {
            case State.Looking:
                unit.LookForPlayer(() => state = State.Chasing);
                break;
            case State.Chasing:
                unit.ChaseThePlayer(enemyProfile.rangeToAttack, () => { if (faceState == FaceState.Hiding) state = State.Attacking; });
                break;
            case State.Attacking:
                // show her face for limited time
                faceState = FaceState.Showing;
                faceTimer = showingFaceDuration;
                // Audio && animations
                startShowingFaceWEvent?.Post(gameObject);
                StartShowingFace?.Invoke();
                // Continue Chasing
                state = State.Chasing;
                break;
        }
        if(faceState == FaceState.Showing)
        {
            faceTimer -= Time.deltaTime;
            if(faceTimer < 0)
            {
                // Audio && animations
                stopShowingFaceWEvent?.Post(gameObject);
                StopShowingFace?.Invoke();
                // Hide face
                faceState = FaceState.Hiding;
            }
        }
    }
}
