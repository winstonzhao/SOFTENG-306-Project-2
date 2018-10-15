using System;
using System.Collections;
using System.Collections.Generic;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace Ultimate_Isometric_Toolkit.Scripts.Pathfinding
{
    /// <summary>
    /// Astar monobehaviour
    /// </summary>
    [RequireComponent(typeof(IsoTransform)), AddComponentMenu("UIT/Pathfinding/A* Agent")]
    public class CivilCarAgent : AstarAgent
    {
        private Vector3 goal;
        private Vector3 originalPosition;
        private float currCountdownValue;
        private bool timerNotStopped;

        private bool notPassable = false;

        private bool checkForBlockingCars = true;


        public void Awake()
        {
            originalPosition = GetComponentInParent<IsoTransform>().Position;
            base.Awake();
        }

        public void StopMoving()
        {
            // stop the movement
            Debug.Log("CivilCarAgent " + gameObject.name + ": stopped moving");
            base.StopAllCoroutines();
        }

        public void StartMoving(Vector3 destination)
        {
            goal = destination;
            ResumeMoving();
        }

        private void FixedUpdate()
        {
            if (!notPassable && base.noPathFound)
            {
                notPassable = true;
            }
        }

        public void ResumeMoving()
        {
            if (goal != null)
            {
                base.MoveTo(goal);
            }
        }

        public void PauseMoving(float milliseconds)
        {
            if (goal != null)
            {
                StopMoving();
                StartCoroutine(StartCountdown(milliseconds));
                StartCoroutine(WaitCountdown());
            }
        }


        IEnumerator StartCountdown(float timeLimit)
        {
            float countdownValue = timeLimit;
            currCountdownValue = countdownValue;
            timerNotStopped = true;
            Debug.Log("start timer: " + currCountdownValue);
            while (currCountdownValue >= 0 && timerNotStopped)
            {
                yield return new WaitForSeconds(0.001f);
                currCountdownValue--;
                Debug.Log(currCountdownValue);
            }
        }

        IEnumerator WaitCountdown()
        {
            yield return new WaitUntil(() => currCountdownValue <= 0);
            Debug.Log("timer ends");
            timerNotStopped = false;
            ResumeMoving();
        }


        void OnIsoTriggerEnter(IsoCollider isoCollider)
        {

                CivilCarAgent collidedCar = isoCollider.GetComponentInParent<CivilCarAgent>();
                if (collidedCar != null)
                {
                    if (collidedCar.notPassable)
                    {
                        StopMoving();
                    }
                }

        }

        public void ResetCar()
        {
            GetComponentInParent<IsoTransform>().Position = originalPosition;
            GetComponent<Animator>().Rebind();
            hasReachedGoal = false;
            noPathFound = false;
            notPassable = false;
        }
    }

}
