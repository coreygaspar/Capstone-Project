using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D exit)
    {
        if(exit.CompareTag("Player"))
        {
            SceneController.instance.NextLevel();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
