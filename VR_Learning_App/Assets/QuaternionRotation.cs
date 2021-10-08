using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionRotation : MonoBehaviour
{
    [SerializeField] private float x = 0;
    [SerializeField] private float y = 0;
    [SerializeField] private float z = 0;

    [SerializeField] GameObject vector;


    // Update is called once per frame
    void Update()
    {
        Vector3 eulerAngles = transform.localRotation.eulerAngles;
        x = eulerAngles.x;
        y = eulerAngles.y;
        z = eulerAngles.z;

        vector.transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x * 180
            , transform.localRotation.y * 180
            , transform.localRotation.z *180));
    }
}
