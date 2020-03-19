using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StartCoroutine(waiter());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(this.gameObject);
    }
}
