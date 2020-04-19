using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public GameObject jumpPoint;
    public bool dead = false;
    public bool stun = false;
    public Animator emmaAnimator;
    public Animator valAnimator;
    public GameObject Emma;
    public GameObject Valerie;
    public GameObject punch;
    public int startHealth;
    public int emmaHealth;
    public Text healthText;
    public Text specialText;
    public int valHealth;
    public bool invisible = false;
    public Material solid;
    public Material transparency;
    public Transform launchPosition;
    public GameObject laserPrefab;
    public Vector3 startPos;
    public Jumping jumper;
    public Brady_StateController Brady;

    public GameObject levelMusic;
    public GameObject bossMusic;

    //public 

    void Start()
    {
        Emma = this.transform.GetChild(0).gameObject;
        Valerie = this.transform.GetChild(1).gameObject;
        punch = this.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        emmaHealth = startHealth;
        healthText.text = "Emma: " + startHealth;
        valHealth = startHealth;
    }

    void Update()
    {
        //Checks to make sure that health is below 100 and above 0.
        checkHealth();

        //Moving and rotating
        if (!dead && !stun)
        {
            var rotate = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
            var forward = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
            transform.Rotate(0, rotate, 0);
            transform.Translate(-forward, 0, 0);
            
            
            //The attack button
            if (Input.GetKeyDown(KeyCode.X) || OVRInput.GetDown(OVRInput.RawButton.B))
            {
                if (Emma.activeInHierarchy == true)
                {
                    if (!invisible)
                    {
                        StartCoroutine(attack());
                    }
                }
                else if (Valerie.activeInHierarchy == true)
                {
                    fireLaser();
                }
            }

            if (Input.GetKeyDown(KeyCode.V) || OVRInput.GetDown(OVRInput.RawButton.X))
            {
                if (Emma.activeInHierarchy == true)
                {
                    turnInvisible();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dead && !stun)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Vector3 jumpDirection = new Vector3(0, Input.GetAxis("Jump"), 0);

            if (moveDirection == Vector3.zero)
            {
                //Makes sure that the character's run animation is turned off
                emmaAnimator.SetBool("IsRunning", false);
                valAnimator.SetBool("IsRunning", false);
                //Switches characters
                if ((Input.GetKeyDown(KeyCode.C) || OVRInput.GetDown(OVRInput.RawButton.Y)) && jumper.grounded)
                {
                    if (Emma.activeInHierarchy == true)
                    {
                        Emma.SetActive(false);
                        Valerie.SetActive(true);
                        healthText.text = "Valerie: " + valHealth;
                        healthText.color = new Color(255, 0, 0);
                        specialText.gameObject.SetActive(false);
                    }
                    else if (Valerie.activeInHierarchy == true)
                    {
                        Emma.SetActive(true);
                        Valerie.SetActive(false);
                        healthText.text = "Emma: " + emmaHealth;
                        healthText.color = new Color(0, 0, 255);
                        specialText.gameObject.SetActive(true);
                    }
                }

            }
            else
            {
                //Turns run animation off if character is jumping
                if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.A))
                {
                    emmaAnimator.SetBool("IsRunning", false);
                    valAnimator.SetBool("IsRunning", false);
                }
                else
                {
                    emmaAnimator.SetBool("IsRunning", true);
                    valAnimator.SetBool("IsRunning", true);
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!dead && !stun)
        {
            //Collision with toxic waste or acid drops 20 HP
            if (collision.gameObject.layer.Equals(18))
            {
                subtractHealth(20);
            }

            //Instant death
            if (collision.gameObject.layer.Equals(28))
            {
                subtractHealth(100);
            }
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (!dead && !stun)
        {
            //HEALTH CHECKERS
            //Collision with Goon's knife or Brady's punch drops 5 HP
            if (collision.gameObject.layer.Equals(20))
            {
                subtractHealth(5);
            }
            //Collision with bullet or Brady's knife drops 10 HP
            if (collision.gameObject.layer.Equals(21))
            {
                Destroy(collision.gameObject);
                subtractHealth(10);

            }
            //Collision with health pickup increases HP by 10
            if ((Emma.activeInHierarchy && emmaHealth <= startHealth) || (Valerie.activeInHierarchy && valHealth <= startHealth))
            {
                if (collision.gameObject.layer.Equals(19))
                {
                    Destroy(collision.gameObject);
                    addHealth(10);
                }
            }

            //Changes the music
            if (collision.gameObject.layer.Equals(22))
            {
                print("Collided with music changer");
                levelMusic.GetComponent<AudioSource>().Stop();
                bossMusic.GetComponent<AudioSource>().Play();
                Brady.SetState(new Brady_RunState(Brady));
                Brady.bradyAnimator.SetBool("BossStart", true);
                Brady.healthText.gameObject.SetActive(true);
            }
        }
    }

    public void fireLaser()
    {
        var laser = Instantiate(laserPrefab, launchPosition.position, launchPosition.rotation);
        laser.GetComponent<Rigidbody>().velocity = new Vector3(Valerie.transform.forward.x, 0, Valerie.transform.forward.z) * 100;                //laser.transform.forward * speed;
    }

    public void turnInvisible()
    {
        if (!invisible)
        {
            Emma.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = transparency;
            invisible = true;
        } else
        {
            Emma.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = solid;
            invisible = false;
        }
    }

    private void checkHealth()
    {
        if (emmaHealth <= 0 || valHealth <= 0)
        {
            dead = true;
            StartCoroutine(deathTimer());
        }

        if (emmaHealth >= startHealth)
        {
            emmaHealth = startHealth;
        }

        if (valHealth >= startHealth)
        {
            valHealth = startHealth;
        }
    }

    private void subtractHealth(int health)
    {
        if (Emma.activeInHierarchy == true)
        {
            emmaHealth -= health;
            healthText.text = "Emma: " + emmaHealth;
        }
        else if (Valerie.activeInHierarchy == true)
        {
            valHealth -= health;
            healthText.text = "Valerie: " + valHealth;
        }
    }

    private void addHealth(int health)
    {
        if (Emma.activeInHierarchy == true)
        {
            emmaHealth += health;
            if (emmaHealth > 100)
            {
                emmaHealth = 100;
            }
            healthText.text = "Emma: " + emmaHealth;
        }
        else if (Valerie.activeInHierarchy == true)
        {
            valHealth += health;
            if (valHealth > 100)
            {
                valHealth = 100;
            }
            healthText.text = "Emma: " + valHealth;
        }
    }

    IEnumerator attack()
    {
        emmaAnimator.SetTrigger("Attack");
        punch.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        punch.SetActive(false);
    }

    IEnumerator deathTimer()
    {
        if (Emma.activeInHierarchy == true)
        {
            emmaAnimator.SetBool("Dead", true);
        } else if (Valerie.activeInHierarchy == true)
        {
            valAnimator.SetBool("Dead", true);
        }
        yield return new WaitForSecondsRealtime(3f);
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
