using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Ferrous.Mechanics
{
    public class multiplePlateDoorOpen : MonoBehaviour
    {
        [SerializeField] private GameObject leftDoor;
        [SerializeField] private GameObject rightDoor;
        public GameObject[] requiredPlates;

        private Vector3 initialPositionLeft;
        private Vector3 initialPositionRight;
        private Vector3 targetPositionLeft;
        private Vector3 targetPositionRight;
        [SerializeField] private float openSpeed = 1.0f;
        private bool canOpen = false;

        // Start is called before the first frame update
        void Start()
        {
            initialPositionLeft = leftDoor.transform.position;
            initialPositionRight = rightDoor.transform.position;
            targetPositionLeft = initialPositionLeft + leftDoor.transform.right * 8f;
            targetPositionRight = initialPositionRight - rightDoor.transform.right * 8f;
        }

        void Update()
        {

            if (canOpen) {
                OpenDoors();
            }
            else {
                canOpen = true;
                for (int i =0; i<requiredPlates.Length; i++){
                    if (!requiredPlates[i].GetComponent<PressurePlate>().Activated){
                        canOpen = false;
                    }
                }
            }
            
        }

        public void OpenDoors()
        {
            rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, targetPositionRight, openSpeed * Time.deltaTime);
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, targetPositionLeft, openSpeed * Time.deltaTime);
        }

    }
}
