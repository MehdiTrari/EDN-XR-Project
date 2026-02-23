using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPin : MonoBehaviour
{

    float fallAngleThreshold = 45f;

    public bool isFallen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!isFallen);
        if (!isFallen)
        {
            float angleDiff = Quaternion.Angle(transform.rotation, Quaternion.Euler(-90, 0, 0));

            if (angleDiff > fallAngleThreshold)
            {
                isFallen = true;
                Debug.Log(gameObject.name + " est tombee !");
            }
        }
    }
}
