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
        private Vector3 goal;  // Location of the goal the car is trying to reach
        private Vector3 originalPosition;  // Starting position of the car
        private float currCountdownValue;  // For countdown timer
        private bool timerNotStopped;  // If the timer is still running
        private bool notPassable = false;  // If a car should act as a wall to other cars not allowing them past
        private bool checkForBlockingCars = true;  // If the cars should be checking for wall cars in front of them


        public void Awake()
        {
            originalPosition = GetComponentInParent<IsoTransform>().Position;
            base.Awake();
        }

        
        /**
         * Stop the cars moving to the goals
         */
        public void StopMoving()
        {
            base.StopAllCoroutines();
        }

        /**
         * Start the cars moving to the goals
         */
        public void StartMoving(Vector3 destination)
        {
            goal = destination;
            ResumeMoving();
        }

        private void FixedUpdate()
        {
            if (!notPassable && base.noPathFound) // If the car can't move to its goal, then make it block other cars
            {
                notPassable = true;
            }
        }

        /**
         * Resume the cars moving to the goals
         */
        public void ResumeMoving()
        {
            if (goal != null)
            {
                base.MoveTo(goal);
            }
        }

        /**
         * Pause the cars temporarily moving to the goals based off the wait time in milliseconds
         */
        public void PauseMoving(float milliseconds)
        {
            if (goal != null)
            {
                StopMoving();
                StartCoroutine(StartCountdown(milliseconds)); // Wait x milliseconds
                StartCoroutine(WaitCountdown());
            }
        }

        /**
         * Start the countdown timer
         */
        IEnumerator StartCountdown(float timeLimit)
        {
            float countdownValue = timeLimit;
            currCountdownValue = countdownValue;
            timerNotStopped = true;
            while (currCountdownValue >= 0 && timerNotStopped)
            {
                yield return new WaitForSeconds(0.001f);
                currCountdownValue--;
                Debug.Log(currCountdownValue);
            }
        }

        /**
         * Wait for timer to expire
         */
        IEnumerator WaitCountdown()
        {
            yield return new WaitUntil(() => currCountdownValue <= 0);
            timerNotStopped = false;
            ResumeMoving();
        }


        /**
         * Detect iso trigger collisions
         */
        void OnIsoTriggerEnter(IsoCollider isoCollider)
        {

                CivilCarAgent collidedCar = isoCollider.GetComponentInParent<CivilCarAgent>();
                if (collidedCar != null)  // If the car has collided with another car
                {
                    if (collidedCar.notPassable)  // And the car is a wall
                    {
                        StopMoving();
                    }
                }

        }

        /**
         * Reset cars to their original positions and checking for paths
         */
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
