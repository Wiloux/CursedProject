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

    [SerializeField] private Animator[] hairsAnimator;
    [SerializeField] private Animator[] attackHairsAnimator;

    private float faceTimer;

    public float idleAnimationDuration = 2f;
    private float moanTimer;

    private Action StartShowingFace;
    private Action StopShowingFace;

    public Material DmgIndacatorMat;
    public float MaxAlphaIndMat;

    #region Wwise Events
    [Space(10)]
    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event startShowingFaceWEvent;
    [SerializeField] private AK.Wwise.Event stopShowingFaceWEvent;
    #endregion

    public override void Start()
    {
        base.Start();

        StartShowingFace = () => { animator.SetBool("attack", true); UpdateHairsAnimators("attack", true); };
        StopShowingFace = () => { animator.SetBool("attack", false); UpdateHairsAnimators("attack", false); };

        hitAnimation = () => { animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 3)); animator.SetTrigger("hit"); };

        faceState = FaceState.Hiding;
        moanTimer = idleAnimationDuration;

        foreach(Animator hair in attackHairsAnimator)
        {
            hair.SetBool("attack", true);
            hair.gameObject.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();
        if (agent.velocity != Vector3.zero) { UpdateHairsAnimators("moving", true); }
        else { UpdateHairsAnimators("moving", false); }
    }

    // Start is called before the first frame update
    public override void LaunchActions()
    {
        switch (state)
        {
            case State.Idle:
                // Loop moan sound
                LoopMoan();

                // If showing face
                if (faceState == FaceState.Showing) { state = State.Chasing; return; }
                // else if player is visible by raycast, start running
                else if (unit.isPlayerVisible(enemyProfile.runningRange)) { state = State.Running; Debug.Log("I was idle and the player is visible"); }
                break;
            case State.Looking:
                // Loop Moan sound
                LoopMoan();

                // Look for player and chase if player found
                unit.LookForPlayer(() => { ShowFace(); state = State.Chasing; });
                break;
            case State.Chasing:
                // If hiding face then run
                if (faceState == FaceState.Hiding) { state = State.Running; return; }
                // Else chase the player unitil a certain range 
                if(unit.GetDistanceFromPlayer() > enemyProfile.rangeToAttack) unit.ChaseThePlayer(enemyProfile.rangeToAttack, null);
                break;
            case State.Running:
                // Loop moan sound
                LoopMoan();

                // If showing face, chase
                if(faceState == FaceState.Showing) { unit.StopRunningFromPlayer(); state = State.Chasing; return; }

                // Else if run from the player to a certain distance, when far enough : "moan" and idle state 
                if(unit.GetDistanceFromPlayer() < enemyProfile.runningRange) unit.RunFromPlayer(0.25f, () => { state = State.Idle; moanTimer = idleAnimationDuration; enemyProfile.onSpawnWEvent?.Post(gameObject); });
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
                SetAttackHairs();
                if (unit.isPlayerVisible(enemyProfile.rangeToAttack))
                {
                    Debug.Log(true);
                    transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
                    float angle = Vector3.Angle(player.transform.forward, player.transform.position - transform.position);
                    if(angle > 90f) { PlayerHelper.instance.TakeDamage(enemyProfile.attackDamage * Time.deltaTime, false); GameHandler.instance.DisplayFacelessGirlDamageIndicator(DmgIndacatorMat, MaxAlphaIndMat); } // Here wiloux
                }
            }
        }
    }

    #region Face Gestion
    private void ShowFace()
    {
        startShowingFaceWEvent?.Post(gameObject);
        StartShowingFace?.Invoke();
        faceState = FaceState.Showing;
        faceTimer = showingFaceDuration;
        ToggleAttackHairs();
    }
    private void HideFace(bool needStopShowingFaceWEvent = true)
    {
        if(needStopShowingFaceWEvent) stopShowingFaceWEvent?.Post(gameObject);
        StopShowingFace?.Invoke();
        faceState = FaceState.Hiding;
        faceTimer = hidingFaceDuration;
        ToggleAttackHairs();
    }
    #endregion

    #region Hairs gestion

        #region Hairs Animator
    private void UpdateHairsAnimators(string animatorParameter, bool newParameterState)
    {
        foreach(Animator hairAnimator in hairsAnimator)
        {
            hairAnimator.SetBool(animatorParameter, newParameterState);
        }
    }
    private void DisableHairsAnimators() {
        foreach (Animator hairAnimator in hairsAnimator)
        {
            hairAnimator.enabled = false;
        }
    }
        #endregion

        #region Attack Hairs gestion
    private void ToggleAttackHairs()
    {
        foreach(Animator hair in attackHairsAnimator)
        {
            hair.gameObject.SetActive(!hair.gameObject.activeSelf);
        }
    }
    private void SetAttackHairs()
    {
        foreach (Animator hair in attackHairsAnimator)
        {
            hair.SetBool("attack", true);
        }
    }
    #endregion

    #endregion

    #region Moan Sound Gestion
    private void LoopMoan()
    {
        // Loop moan sound
        if (moanTimer > 0) moanTimer -= Time.deltaTime;
        else { moanTimer = idleAnimationDuration; enemyProfile.onSpawnWEvent?.Post(gameObject); }
    }

    #endregion

    public override void GetStaggered()
    {
        HideFace(false);
        unit.GetStaggered(enemyProfile.staggerDuration, () => state = State.Running);
    }

    public override void Die()
    {
        base.Die();

        UpdateHairsAnimators("attack", false);
        DisableHairsAnimators();
        if (attackHairsAnimator[0].gameObject.activeSelf) ToggleAttackHairs();
    }
}
