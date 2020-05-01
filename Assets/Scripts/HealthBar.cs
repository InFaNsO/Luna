using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentHealthSlider;

    public void UpdateHealthBar(float percentage)
    {
        if (percentage < 0.0f)
            percentage = 0.0f;
        if (percentage > 1.0f)
            percentage = 1.0f;
        currentHealthSlider.gameObject.transform.localScale = new Vector3(percentage, 1.0f,1.0f);
    }
}
