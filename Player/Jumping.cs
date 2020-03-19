using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour {

    [Range(1, 40)]
    public float jumpVelocity;
    public float groundedSkin = 0.1f;
    public LayerMask mask;

    public bool jumpRequest;
    public bool grounded;

    Vector3 playerSize;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    Rigidbody rb;

    public Animator emmaAnimator;
    public Animator valAnimator;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerSize = GetComponent<BoxCollider>().size;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ((Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.A)) && grounded)
        {
            jumpRequest = true;

        }

        if (!grounded)
        {
            emmaAnimator.SetBool("IsJumping", true);
            valAnimator.SetBool("IsJumping", true);
        } else
        {
            emmaAnimator.SetBool("IsJumping", false);
            valAnimator.SetBool("IsJumping", false);
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest == true)
        {
            //rb.velocity += Vector3.up * jumpVelocity;
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            jumpRequest = false;
            grounded = false;
        } else
        {
            Vector3 rayStart = transform.position + Vector3.down * playerSize.y * 0.5f;
            grounded = Physics.Raycast(rayStart, Vector3.down, groundedSkin, mask);
        }



        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.A)))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
