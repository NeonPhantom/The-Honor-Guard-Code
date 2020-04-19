using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goon2Controller : MonoBehaviour {

    public GameObject enemyToShoot;
    public UnityStandardAssets.Characters.ThirdPerson.Goon2AICharacterControl ai;
    public GameObject enemy;
    Transform destination;
    public float detectionRange;
    public float framesStart;
    public float framesEnd;
    public Transform launchPosition;
    public GameObject bulletPrefab;
    public int attackTimer;
    public Player hero;
    //public SoundFX gunshot;

    public Animator enemyAnimator;
    public int enemyHealth;

    
    public bool CheckIfInRange(string tag)
    {
        enemy = GameObject.FindGameObjectWithTag(tag);
        
        if(Vector3.Distance(enemy.transform.position, transform.position) < detectionRange)
        {
            enemyToShoot = enemy;
            return true;
        }
         

        return false;
    }

    void Start () {
        ai = GetComponent<UnityStandardAssets.Characters.ThirdPerson.Goon2AICharacterControl>();
	}
	
	void Update () {
        if (!ai.dead)
        {
            if (CheckIfInRange("Player") && (!hero.invisible || !hero.Emma.activeInHierarchy))
            {
                destination = enemy.transform;
                ai.SetTarget(destination);
                ai.gameObject.transform.LookAt(ai.target);
                enemyAnimator.SetBool("Attack", true);
                if (attackTimer == 500)
                {
                    var bullet = Instantiate(bulletPrefab, launchPosition.position, launchPosition.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
                }
                attackTimer--;
                if (attackTimer <= 0)
                {
                    attackTimer = 500;
                }
            }
            else
            {
                enemyAnimator.SetBool("Attack", false);
                //Stop shooting
            }

            checkHealth();
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //HEALTH CHECKERS
        //Collision with punch decreases HP by 20
        if (other.gameObject.layer == 23)
        {
            enemyHealth -= 20;
            other.gameObject.SetActive(false);
            print(enemyHealth);
        }

        //Collision with lasers decreases HP by 10
        if (other.gameObject.layer == 24)
        {
            enemyHealth -= 10;
            Destroy(other.gameObject);
            print(enemyHealth);
        }
    }

    private void checkHealth()
    {
        if (enemyHealth <= 0)
        {
            ai.dead = true;
            enemyAnimator.SetBool("Dead", true);
            StartCoroutine(waiter());
        }

    }
    
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(this.gameObject);
    }

}
