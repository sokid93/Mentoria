using System;
using UnityEngine;
using Player;


    public class PlayerRunSkill : MonoBehaviour
    {
        [Header("[References]")]
        [SerializeField] private SimpleMovement playerMovement;
        [SerializeField] Animator DayAnimator;
        [SerializeField] Animator nightAnimator;
        [SerializeField] AudioSource tiredSound;
        [SerializeField] ParticleSystem sweatParticle;
        
        [field: SerializeField] public float timeSprinting { get; private set; }


        private void Update()
    {
        SprintSkill();

        if (IsTired())
        {
            TiredState();
        }
        else if (IsRested())
        {
            RestedState();
        }

        TiredSoundControl();
    }

    private void SprintSkill()
    {
        if (PlayerPressSprint() && CanSprint())
        {
            SetAnimatorSpeed(2f);
            timeSprinting += GetDeltaTime();

            if (IsOnMug())
                SetPlayerSlowRun();
            else
                SetPlayerRunSpeed();

        }
        else
        {
            if (IsOnMug())
            {
                SetPlayerSlowWalk();
                timeSprinting -= GetDeltaTime();
            }
            else
            {
                SetAnimatorSpeed(1f);
                SetPlayerNormalSpeed();
                timeSprinting -= GetDeltaTime();
            }

        }
    }

    private void SetPlayerSlowWalk()
    {
        SetPlayerMovementSpeed(0.5f);
    }

    private void SetPlayerSlowRun()
    {
        SetPlayerMovementSpeed(Configuration.runSpeed / 2);
    }

    private void SetPlayerNormalSpeed()
    {
        SetPlayerMovementSpeed(playerMovement.normalSpeed);
    }

    private void SetPlayerRunSpeed()
    {
        SetPlayerMovementSpeed(Configuration.runSpeed);
    }

    private static bool IsOnMug()
    {
        return Booleans.isOnMug;
    }

    private bool PlayerPressSprint()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    private bool CanSprint()
    {
        return timeSprinting < Configuration.timePlayerCanSprint && Booleans.canSprint;
    }

    private static float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    private void SetAnimatorSpeed(float newSpeed)
    {
        DayAnimator.speed = newSpeed;
        nightAnimator.speed = newSpeed;
    }

    private void SetPlayerMovementSpeed(float newSpeed)
    {
        playerMovement.speed = newSpeed;
    }
    private bool IsRested()
    {
        return timeSprinting <= 0;
    }

    private bool IsTired()
    {
        return timeSprinting >= Configuration.timePlayerCanSprint;
    }

    private void TiredState()
    {
        Booleans.canSprint = false;
        sweatParticle.Play();
    }

    private void RestedState()
    {
        timeSprinting = 0;
        Booleans.canSprint = true;
        sweatParticle.Stop();
    }

    private void TiredSoundControl()
    {
        if (!Booleans.canSprint)
        {
            if (Booleans.isPlayerTired)
            {
                PlayTiredSound();
            }
        }
        else
        {
            StopTiredSound();
        }
    }

    private void PlayTiredSound()
    {
        Booleans.isPlayerTired = false;
        tiredSound.Play();
    }

    private void StopTiredSound()
    {
        tiredSound.Stop();
        Booleans.isPlayerTired = true;
    }
}

