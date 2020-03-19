using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletSpin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(-1, 1, 0);
	}

}
