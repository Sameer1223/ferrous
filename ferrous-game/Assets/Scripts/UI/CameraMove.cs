using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ferrous
{
    public class CameraMove : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private float _cameraSpeed = 10f;

        private Vector3 moveDirection = Vector3.forward;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float moveDistance = _cameraSpeed * Time.deltaTime;

            // �����µ�λ��
            Vector3 newPosition = transform.position + moveDirection * moveDistance;

            // �ƶ����嵽�µ�λ��
            transform.position = newPosition;
        }
    }
}
