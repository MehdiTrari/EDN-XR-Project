using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PinCounter : MonoBehaviour
{

     public FallingPin[] pins;

    public int fallenCount = 0;

    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        pins = GetComponentsInChildren<FallingPin>();
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;

        foreach (FallingPin pin in pins)
        {
            if (pin.isFallen)
            {
                count++;
            }
        }
        fallenCount = count;

        if (scoreText != null)
        {
            scoreText.text = "Quilles : " + fallenCount.ToString();
        }
    }
}
