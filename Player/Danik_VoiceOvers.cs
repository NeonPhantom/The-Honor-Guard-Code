using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danik_VoiceOvers : MonoBehaviour {

    public AudioClip[] sounds;
    private int soundNum = 0;
    public AudioClip lockedDoor;
    public AudioClip bossStart;
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(26))
        {
            if (other.gameObject.tag.Equals("Gauntlet"))
            {
                door.GetComponent<Animation>().Play();
            }
            GetComponent<AudioSource>().clip = sounds[soundNum];
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
            soundNum++;
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
}
