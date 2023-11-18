using UnityEngine;

namespace Ferrous
{
    public class SwitchLightCheck : MonoBehaviour
    {
        // Start is called before the first frame update
        private Light light1;
        private Light light2;
        private Light light3;
        private Light light4;
        private GameObject LeftDoor1;
        private GameObject RightDoor1;
        private GameObject reds;
        private GameObject blues;
        public AudioSource openDoorAudio;
        private bool skipConditions;
        void Start()
        {
            light1 = GameObject.FindGameObjectsWithTag("SwitchLightOne")[0].GetComponent<Light>();
            light2 = GameObject.FindGameObjectsWithTag("SwitchLightTwo")[0].GetComponent<Light>();;
            light3 = GameObject.FindGameObjectsWithTag("SwitchLightThree")[0].GetComponent<Light>();;
            light4 = GameObject.FindGameObjectsWithTag("SwitchLightFour")[0].GetComponent<Light>();;
            reds = GameObject.FindGameObjectsWithTag("Red")[0];
            blues = GameObject.FindGameObjectsWithTag("Blue")[0];
            LeftDoor1 = GameObject.FindGameObjectsWithTag("LeftDoorOne")[0];
            RightDoor1 = GameObject.FindGameObjectsWithTag("RightDoorOne")[0];
            skipConditions = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (skipConditions)
            {
                ;
            }
        
            else if ((light1.intensity == 2) && (light2.intensity == 2) && (light3.intensity == 2) && (light4.intensity == 2))
            {
                Destroy(LeftDoor1);
                Destroy(RightDoor1);
                openDoorAudio.Play(0);
                skipConditions = true;

            }
            else
            {
                light1.intensity = 0;
                light2.intensity = 0;
                light3.intensity = 0;
                light4.intensity = 0;

                foreach (Transform red in reds.transform)
                {
                    foreach (Transform blue in blues.transform)
                    {
                        if ((((-3.2 < red.position.z && red.position.z < -1.6) && (-9.3 < blue.position.z && blue.position.z < -6.3))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2)))
                            || 
                            (((-3.2 < blue.position.z && blue.position.z < -1.6) && (-9.3 < red.position.z && red.position.z < -6.3))
                             && ((1.8 < blue.position.x && blue.position.x < 3.2) && (1.8< red.position.x && red.position.x < 3.2))))
                        {
                            light1.intensity = 2;
                        }
                        if ((((-3.2 < red.position.z && red.position.z < -1.6) && (-0.77 < blue.position.z && blue.position.z < 1.16))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2)))
                            || 
                            (((-3.2 < blue.position.z && blue.position.z < -1.6) && (-0.77 < red.position.z && red.position.z < 1.16))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2))))
                        {
                            light2.intensity = 2;
                        }
                        if ((((-0.77 < red.position.z && red.position.z < 1.16) && (1.16 < blue.position.z && blue.position.z < 4.32))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2)))
                            || 
                            (((-0.77 < blue.position.z && blue.position.z < 1.16) && (1.16 < red.position.z && red.position.z < 4.32))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2))))
                        {
                            light3.intensity = 2;
                        }
                        if ((((6.57 < red.position.z && red.position.z < 9.48) && (9.48 < blue.position.z && blue.position.z < 13.69))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2)))
                            || 
                            (((6.57 < blue.position.z && blue.position.z < 9.48) && (9.48 < red.position.z && red.position.z < 13.69))
                             && ((1.8 < red.position.x && red.position.x < 3.2) && (1.8< blue.position.x && blue.position.x < 3.2))))
                        {
                            light4.intensity = 2;
                        }
                    }
                }
            
        
            }

        }
    }
}
