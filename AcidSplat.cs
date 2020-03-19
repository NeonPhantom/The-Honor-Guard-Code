using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSplat : MonoBehaviour {

    public GameObject splatPrefab;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer.Equals(0))
        {
            //Create acid splat
            Instantiate(splatPrefab, this.transform.position, new Quaternion(0,0,0,0));
            Destroy(this.gameObject);
        }
    }
}
