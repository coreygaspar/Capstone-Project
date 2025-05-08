using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    AudioSource pickupSource;

     private void Awake() 
    {
        pickupSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
