using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    public Button newGameButton;
    public Button quitGameButton;
    public Sprite newGameNormal;
    public Sprite newGameHover;
    public Sprite newGameClicked;
    public Sprite quitGameNormal;
    public Sprite quitGameHover;
    public Sprite quitGameClicked;
    private bool newGameHighlight;
    private bool quitGameHighlight;

	// Use this for initialization
	void Start () {
        newGameButton.image.sprite = newGameHover;
        newGameHighlight = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (newGameHighlight == true) {
            if (Input.GetKeyDown(KeyCode.DownArrow) || OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)) {
                newGameButton.image.sprite = newGameNormal;
                quitGameButton.image.sprite = quitGameHover;
                newGameHighlight = false;
                quitGameHighlight = true;
            }
            if (Input.GetKey(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.A))
            {
                newGameButton.image.sprite = newGameClicked;
                SceneManager.LoadScene("IndustrialPlant");
            }
        }
        if (quitGameHighlight == true) {
            if (Input.GetKeyDown(KeyCode.UpArrow) || OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
            {
                newGameButton.image.sprite = newGameHover;
                quitGameButton.image.sprite = quitGameNormal;
                newGameHighlight = true;
                quitGameHighlight = false;
            }
            if (Input.GetKey(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.A))
            {
                quitGameButton.image.sprite = quitGameClicked;
                Application.Quit();
            }
        }
    }
}
