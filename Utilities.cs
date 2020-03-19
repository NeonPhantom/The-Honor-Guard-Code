using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {


    public float canJump = 0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Allows the player to rotate when using the left/right arrow and A/D keys.
        var rotate = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
        transform.Rotate(0, rotate, 0);

        //Allows the player to move forward and backward when using the up/down arrows and W/S keys.
        var forward = Input.GetAxis("Vertical") * Time.deltaTime * 50.0f;
        transform.Translate(-forward, 0, 0);







        //Checks for when the player presses the Spacebar and Time.time is greater than the canJump number.
        //Allows the player to jump.
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canJump)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 50);         //The number multiplied by the Vector3.up will need to be different depending on how much mass the player has on its rigidbody
            canJump = Time.time + 1.5f;
        }








        //This allows the ability to switch back and forth between 2 child characters:

        //First, create an empty game object, and give it 2 children to represent 2 characters.
        //Add this script to the empty parent object.
        if (Input.GetKeyDown(KeyCode.C))                                            //Any key can work here
        {
            //Checks if the 1st child is active.
            if (this.transform.GetChild(0).gameObject.activeInHierarchy == true)
            {
                //If the 1st child is active, then the 1st child is made inactive, while the 2nd child is made active.
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(true);
            }
            //Else checks if the 2nd child is active.
            else if (this.transform.GetChild(1).gameObject.activeInHierarchy == true)
            {
                //If the 2nd child is active, then the 2nd child is made inactive, while the 1st child is made active.
                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

    }
}
