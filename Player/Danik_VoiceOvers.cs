using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danik_VoiceOvers : MonoBehaviour {

    public AudioClip[] sounds;
    private int soundNum = 0;
    public AudioClip lockedDoor;
    public AudioClip bossStart;
    public GameObject door;
    public Player playerScript;
    public CameraMovement cameraScript;
    public GameObject dancingBrady;
    public GameObject runningBrady;
    public Text subtitles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(26))
        {
            if (other.gameObject.tag.Equals("Gauntlet"))
            {
                door.GetComponent<Animation>().Play();
                playSound(other);
            } else if (other.gameObject.tag.Equals("VGhostEncounter"))
            {
                StartCoroutine(vGhostEncounter1(other));
            } else if (other.gameObject.tag.Equals("GetBackHere")) {
                StartCoroutine(vGhostEncounter2(other));
            } else
            {
                playSound(other);
            }
            
        }

        if (other.gameObject.layer.Equals(27))
        {
            GetComponent<AudioSource>().clip = lockedDoor;
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer.Equals(22))
        {
            GetComponent<AudioSource>().clip = bossStart;
            GetComponent<AudioSource>().Play();
        }
    }

    private void playSound(Collider other)
    {
        GetComponent<AudioSource>().clip = sounds[soundNum];
        GetComponent<AudioSource>().Play();
        //subtitles.GetComponent<Animator>();
        Destroy(other.gameObject);
        soundNum++;
    }

    IEnumerator vGhostEncounter1(Collider other)
    {
        playerScript.stun = true;
        cameraScript.followTarget = dancingBrady;
        cameraScript.cutsceneRotation = true;
        playSound(other);
        yield return new WaitForSecondsRealtime(5f);
        dancingBrady.GetComponent<Animator>().SetTrigger("Shocked");
        dancingBrady.GetComponent<AudioSource>().Stop();
        yield return new WaitForSecondsRealtime(1.5f);
        dancingBrady.GetComponent<Animator>().SetTrigger("RunAway");
        cameraScript.followTarget = this.gameObject;
        cameraScript.cutsceneRotation = false;
        dancingBrady.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 1) * 20;
        playerScript.stun = false;
        yield return new WaitForSecondsRealtime(2f);
        Destroy(dancingBrady.gameObject);
    }

    IEnumerator vGhostEncounter2(Collider other)
    {
        playSound(other);
        runningBrady.GetComponent<Animator>().SetTrigger("Shocked");
        yield return new WaitForSecondsRealtime(1f);
        runningBrady.GetComponent<Animator>().SetTrigger("RunAway");
        runningBrady.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 1) * 20;
        yield return new WaitForSecondsRealtime(2f);
        Destroy(runningBrady.gameObject);
    }
}
