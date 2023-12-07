using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : MonoBehaviour
{
    public Transform turbine;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {       
        turbine.localEulerAngles += new Vector3(0, 0, Random.Range(0, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        turbine.localEulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
    }
}
