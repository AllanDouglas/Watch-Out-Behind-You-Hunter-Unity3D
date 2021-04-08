using System;
using UnityEngine;
using UnityEngine.UI;

namespace WOBH
{
    public class AmountDisplayer : MonoBehaviour
    {
        [SerializeField] Text amountText;

        public void SetText(int value)
        {
            amountText.text = value.ToString();
        }

        
    }
}