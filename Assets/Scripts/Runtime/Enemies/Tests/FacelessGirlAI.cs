using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacelessGirlAI : EnemyBaseAI
{
    private enum FaceState
    {
        Hiding,
        Showing
    }
    private FaceState faceState;
    [SerializeField] private float showingFaceDuration;
    [SerializeField] private float hidingFaceDuration;

    private float faceTimer;

    private Action StartShowingFace;
    private Action StopShowingFace;

    #region Wwise Events
    [Space(10)]
    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event startShowingFaceWEvent;
    [SerializeField] private AK.Wwise.Event stopShowingFaceWEvent;
    #endregion

    public override void Start()
    {
        base.Start();
        HideFace();
    }

    // Start is called before the first frame update
    public override void LaunchActions()
    {
        switch (state)
        {
            case State.Idle:
                if(faceState == FaceState.Showing) { state = State.Chasing; return; }
                else if (unit.isPlayerVisible(enemyProfile.runningRange)) { state = State.Running; }
                break;
            case State.Looking:
                unit.LookForPlayer(() => { ShowFace(); state = State.Chasing; });
                break;
            case State.Chasing:
                if (faceState == FaceState.Hiding) { state = State.Running; return; }
                unit.ChaseThePlayer(enemyProfile.rangeToAttack, null);
                break;
            case State.Running:
                if(faceState == FaceState.Showing) { state = State.Chasing; return; }
                if(unit.GetDistanceFromPlayer() < enemyProfile.runningRange) unit.RunFromPlayer(0.25f, () => state = State.Idle);
                break;
        }

        if(state != State.Looking)
        {
            faceTimer -= Time.deltaTime;
            if(faceTimer < 0)
            {
                if (faceState == FaceState.Showing) HideFace();
                else ShowFace();
            }
            //switch (faceState)
            //{
            //    case FaceState.Hiding:
            //        break;
            //    case FaceState.Showing:
            //        break;
            //}
            if(faceState == FaceState.Showing)
            {
                if (unit.isPlayerVisible(enemyProfile.rangeToAttack))
                {
                    float angle = Vector3.Angle(player.transform.forward, transform.forward);
                    if(angle > 90f) { Debug.Log("Stealing health"); }
                }
            }
        }
    }

    private void ShowFace()
    {
        startShowingFaceWEvent?.Post(gameObject);
        StartShowingFace?.Invoke();
        faceState = FaceState.Showing;
        faceTimer = showingFaceDuration;
    }
    private void HideFace()
    {
        stopShowingFaceWEvent?.Post(gameObject);
        StopShowingFace?.Invoke();
        faceState = FaceState.Hiding;
        faceTimer = hidingFaceDuration;
    }
}
