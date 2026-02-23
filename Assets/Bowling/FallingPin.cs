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
            float angle = Vector3.Angle(transform.up, Vector3.up);

            if (angle < fallAngleThreshold)
            {
                isFallen = true;
                Debug.Log(gameObject.name + " est tombee !");
            }
        }
    }
}
