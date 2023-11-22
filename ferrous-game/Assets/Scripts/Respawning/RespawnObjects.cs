using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class RespawnObjects : MonoBehaviour
    {
        
        [SerializeField] private bool isRespawnTrigger;
        
        
        [Header("Respawning")]
        public List<GameObject> canRespawn = new List<GameObject>();
        private List<string> gameobjectNames = new List<string>();
        public List<Transform> spawnLocations = new List<Transform>();
        private Dictionary<string, Transform> spawnMapping = new Dictionary<string, Transform>();
        
        
        
        // Start is called before the first frame update
        private void Start()
        {
            // create the dict of object names and their spawn points
            for (int i = 0; i < canRespawn.Count; i++)
            {
                spawnMapping.Add(canRespawn[i].name, spawnLocations[i]);
                gameobjectNames.Add(canRespawn[i].name);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // if its a respawn trigger, respawn the item that was collided
            if (isRespawnTrigger)
            {
                respawnObject(other.gameObject);
            }
        }

        public void respawnAll()
        {
            foreach (GameObject go in canRespawn)
            {
                respawnObject(go);
            }

        }

        /// <summary>
        /// respawn the gameobject in its spawn position
        /// </summary>
        /// <param name="toRespawn">the gameobject to respawn</param>
        private void respawnObject(GameObject toRespawn)
        {
            // make sure the object can be respawned
            string enteredName = toRespawn.name;
            if (spawnMapping.TryGetValue(enteredName, out Transform respawnLocation))
            {
                // if player enters, move the player to the spawn point
                toRespawn.transform.position = respawnLocation.position;
            }
        }
        
       
    }
}
