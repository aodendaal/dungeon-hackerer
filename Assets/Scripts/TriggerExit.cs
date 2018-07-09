using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour {

    private GlobalManager manager;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("DungeonManager").GetComponent<GlobalManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Triggered level change");
            manager.ChangeLevel();
        }
    }
}
