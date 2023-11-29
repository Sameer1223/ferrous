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

            // 计算新的位置
            Vector3 newPosition = transform.position + moveDirection * moveDistance;

            // 移动物体到新的位置
            transform.position = newPosition;
        }
    }
}
