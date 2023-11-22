using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class ResetLinked : MonoBehaviour
    {
        
        [Header("LinkedBlocks")]
        public List<GameObject> linkedObjects = new List<GameObject>();
        public List<Transform> loPositions = new List<Transform>();
    
        
        /// <summary>
        /// Reset all linked objects to their original positions
        /// </summary>
        public void resetLink()
        {
            for (int i = 0; i < linkedObjects.Count; i++)
            {
                linkedObjects[i].transform.position = loPositions[i].transform.position;
            }
        }
    }
}
