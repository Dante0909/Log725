using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSlider : MonoBehaviour
{
    public TextMeshProUGUI numberText;

    public void setNumberText(float value)
    {
        int intValue = (int)value;
        numberText.text = intValue.ToString();
    }
}
