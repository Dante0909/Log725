using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private List<Image> healthImages;
    // Start is called before the first frame update
    void Start()
    {
        healthImages = new List<Image>();
        foreach (Transform t in transform)
        {
            healthImages.Add(t.GetComponent<Image>());
        }
        PlayerHealth.HealthChanged += PlayerHealth_HealthChanged;
    }

    void OnDestroy()
    {
        PlayerHealth.HealthChanged -= PlayerHealth_HealthChanged;
    }

    private void PlayerHealth_HealthChanged(int newHealth)
    {
        for (int i = 0; i < healthImages.Count; ++i)
        {
            healthImages[i].enabled = newHealth > i;
        }
    }
}
