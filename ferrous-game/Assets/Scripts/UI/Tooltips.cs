using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ferrous
{
    public class Tooltips : MonoBehaviour
    {
        public TextMeshProUGUI tooltipText;
        public string tooltipTextString;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                Debug.Log("Enter tooltip area");
                ShowTooltipText();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Exit tooltip area");
                HideTooltipText();
            }
        }

        private void ShowTooltipText()
        {
            tooltipText.text = tooltipTextString;
        }

        private void HideTooltipText()
        {
            tooltipText.text = "";
            Destroy(this.gameObject);
        }
    }
}
