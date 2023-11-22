using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ferrous.Mechanics
{
    public class OpenDoor : MonoBehaviour
    {
        [SerializeField] private GameObject leftDoor;
        [SerializeField] private GameObject rightDoor;


        private Vector3 initialPositionLeft;
        private Vector3 initialPositionRight;
        private Vector3 targetPositionLeft;
        private Vector3 targetPositionRight;
        [SerializeField] private float openSpeed = 1.0f;

        private float slideDuration = 4f;
        private bool doorOpen = false;

        // Start is called before the first frame update
        void Start()
        {
            
        }


        public void OpenDoors()
        {
            if (!doorOpen)
            {

                StartCoroutine(SlideDoor());
            }
        }

        public void OpenLargeDoors()
        {
            if (!doorOpen)
            {
                StartCoroutine(SlideLargeDoor());
            }
        }


        IEnumerator SlideDoor()
        {
            doorOpen = true;
            initialPositionLeft = leftDoor.transform.position;
            initialPositionRight = rightDoor.transform.position;
            targetPositionLeft = initialPositionLeft + leftDoor.transform.right * 3.5f;
            targetPositionRight = initialPositionRight - rightDoor.transform.right * 3.5f;
            float startTime = Time.time;

            while (Time.time - startTime < slideDuration)
            {
        
                rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, targetPositionRight, openSpeed * Time.deltaTime);
                leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, targetPositionLeft, openSpeed * Time.deltaTime);
                yield return null;
            }
            rightDoor.transform.position = targetPositionRight;
            leftDoor.transform.position = targetPositionLeft;
        }

        IEnumerator SlideLargeDoor()
        {
            doorOpen = true;
            initialPositionLeft = leftDoor.transform.position;
            initialPositionRight = rightDoor.transform.position;
            targetPositionLeft = initialPositionLeft + leftDoor.transform.right * 8f;
            targetPositionRight = initialPositionRight - rightDoor.transform.right * 8f;
            float startTime = Time.time;

            while (Time.time - startTime < slideDuration)
            {
        
                rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, targetPositionRight, openSpeed * Time.deltaTime);
                leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, targetPositionLeft, openSpeed * Time.deltaTime);
                yield return null;
            }
            rightDoor.transform.position = targetPositionRight;
            leftDoor.transform.position = targetPositionLeft;
        }

    }
}

