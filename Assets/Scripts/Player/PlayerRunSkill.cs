using System;
using UnityEngine;

namespace Player
{

    public class PlayerRunSkill : MonoBehaviour
    {
        [Header("[References]")]
        [SerializeField] private SimpleMovement playerMovement;
        [SerializeField] Animator DayAnimator;
        [SerializeField] Animator nightAnimator;
        [SerializeField] AudioSource tiredSound;
        [SerializeField] ParticleSystem sweatParticle;

        [Header("[Configuration]")]
        [SerializeField] private float runSpeed;
        [SerializeField] private float timePlayerCanSprint;
        [SerializeField] float timeSprinting 
        {
            get => timeSprinting;
            set => timeSprinting = Math.Max(value, 0); 
        }

        [Header("[Values]")]
        bool canSprint = true;
        public bool isOnMug;
        private bool isPlayerTired;
        
        private void Update()
        {
            SprintSkill();

            if (timeSprinting >= timePlayerCanSprint)
            {
                TiredState();
            }
            else if (timeSprinting <= 0)
            {
                RestedState();
            }

            TiredSoundControl();
        }

        private void TiredState()
        {
            canSprint = false;
            sweatParticle.Play();
        }

        private void RestedState()
        {
            timeSprinting = 0;
            canSprint = true;
            sweatParticle.Stop();
        }

        private void TiredSoundControl()
        {
            if (!canSprint)
            {
                if (isPlayerTired)
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
            isPlayerTired = false;
            tiredSound.Play();
        }

        private void StopTiredSound()
        {
            tiredSound.Stop();
            isPlayerTired = true;
        }

        private void SprintSkill()
        {
            if (Input.GetKey(KeyCode.LeftShift) && timeSprinting < timePlayerCanSprint)
            {
                if (canSprint)
                {
                    SetAnimatorSpeed(2f);
                    timeSprinting += GetDeltaTime();

                    if (isOnMug)
                        SetPlayerMovementSpeed(runSpeed / 2);
                    else
                        SetPlayerMovementSpeed(runSpeed);
                }

            }
            else
            {
                if (!isOnMug)
                {
                    SetAnimatorSpeed(1f);
                    SetPlayerMovementSpeed(playerMovement.normalSpeed);
                    timeSprinting -= GetDeltaTime();
                }
                else
                {
                    SetPlayerMovementSpeed(0.5f);
                    timeSprinting -= GetDeltaTime();
                }
 
            }
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
    }
}
