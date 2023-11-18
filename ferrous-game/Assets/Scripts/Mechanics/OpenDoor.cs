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

        // Start is called before the first frame update
        void Start()
        {
            initialPositionLeft = leftDoor.transform.position;
            initialPositionRight = rightDoor.transform.position;
            targetPositionLeft = initialPositionLeft + leftDoor.transform.right * 3.5f;
            targetPositionRight = initialPositionRight - rightDoor.transform.right * 3.5f;
        }


        public void OpenDoors()
        {
            rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, targetPositionRight, openSpeed * Time.deltaTime);
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, targetPositionLeft, openSpeed * Time.deltaTime);
        }

    }
}

