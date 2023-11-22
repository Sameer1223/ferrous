using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class InstantiateObject : MonoBehaviour
    {
        [SerializeField] private GameObject toInstantiate;
        [SerializeField] private Transform spawnPoint;

        public void SpawnObject()
        {
            var spawnedObject = GameObject.Find(toInstantiate.name +"(Clone)");
            if (!spawnedObject)
            {
                Instantiate(toInstantiate, position: spawnPoint.position, rotation: spawnPoint.rotation);
            }
        }
    }
}
