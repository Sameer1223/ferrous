using UnityEngine;

namespace Ferrous.Blocks
{
    public class ObjectCharge : MonoBehaviour
    {

        private enum Charge
        {
            None, //0
            North, //1
            South, //2
        }

        [SerializeField] private Charge charge = Charge.None;
        private Rigidbody rb;

        private float sphereRadius;
        private float pullingForce = 7f;

        private void Start()
        {
            if (charge == Charge.None)
            {
                Destroy(this); 
                return;
            }

            rb = GetComponent<Rigidbody>();
            sphereRadius = transform.localScale.z + 1f;
        }

        // Update is called once per frame
        void Update()
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);

            foreach (Collider collider in hitColliders)
            {
                // Get component rigidbody
                Rigidbody hitRigidbody = collider.GetComponent<Rigidbody>();

                if (hitRigidbody != null && hitRigidbody != rb)
                {
                    Debug.Log(hitRigidbody);
                    // Calculate the direction
                    Vector3 forceDirection = (transform.position - hitRigidbody.transform.position).normalized;
                    if (charge == Charge.South) { forceDirection = -forceDirection; }

                    hitRigidbody.AddForce(forceDirection * pullingForce);
                }
            }
        }
    }
}
