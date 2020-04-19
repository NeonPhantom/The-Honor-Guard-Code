using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brady_StateController : MonoBehaviour {

    public Brady_State currentState;
    public GameObject[] navPoints;
    public GameObject enemyToChase;
    public int navPointNum;
    //public float remainingDistance;
    //public Transform destination;
    public UnityStandardAssets.Characters.ThirdPerson.BradyAICharacterControl ai;
    //public Renderer[] childrenRend;
    public GameObject enemy;
    public float detectionRange;
    public float attackRange;
    public float framesStart;
    public float framesEnd;

    public Animator bradyAnimator;
    public int bradyHealth;
    public int halfHealth;
    public GameObject punch;
    public bool spray = true;
    public GameObject knifeTarget;
    public Transform launchPosition;
    public GameObject knifePrefab;
    public GameObject acidPrefab;
    public Transform centerPosition;
    public int sprayTimer = 500;
    public bool acidSpray = false;
    public Text healthText;

    public Transform GetNextNavPoint()
    {
        navPointNum = (navPointNum + 1) % navPoints.Length;
        return navPoints[navPointNum].transform;
    }


    public bool CheckIfInRange(string tag)
    {
        enemy = GameObject.FindGameObjectWithTag(tag);
        
        if(Vector3.Distance(enemy.transform.position, transform.position) < detectionRange)
        {
            enemyToChase = enemy;
            return true;
        }
         

        return false;
    }

    public bool InAttackRange(string tag)
    {
        enemy = GameObject.FindGameObjectWithTag(tag);

        if (Vector3.Distance(enemy.transform.position, transform.position) < attackRange)
        {
            return true;
        }


        return false;
    }

    void Start () {
        ai = GetComponent<UnityStandardAssets.Characters.ThirdPerson.BradyAICharacterControl>();
        punch = this.transform.GetChild(0).GetChild(1).gameObject;
        knifeTarget = GameObject.FindGameObjectWithTag("Player");
        halfHealth = bradyHealth / 2;
	}

    void Update()
    {
        if (currentState != null)
        {
            currentState.CheckTransitions();
            currentState.Act();
        }

        checkHealth();
    }

    public void SetState(Brady_State state)
    {
        if(currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = state;
        gameObject.name = "AI agent in state " + state.GetType().Name;

        if(currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //HEALTH CHECKERS
        //Collision with punch decreases HP by 20
        if (other.gameObject.layer == 23)
        {
            bradyHealth -= 20;
            other.gameObject.SetActive(false);
            healthText.text = "Brady: " + bradyHealth;
        }

        //Collision with lasers decreases HP by 10
        if (other.gameObject.layer == 24)
        {
            bradyHealth -= 10;
            Destroy(other.gameObject);
            healthText.text = "Brady: " + bradyHealth;
        }
    }

    private void checkHealth()
    {
        if (bradyHealth <= 0)
        {
            SceneManager.LoadScene("DanikCutscene");
        }

    }

    public void sprayAcid()
    {
        var acidblob = Instantiate(acidPrefab, launchPosition.position, launchPosition.rotation);
        Vector3 dir = enemy.transform.position - transform.position;
        acidblob.GetComponent<Rigidbody>().AddForce(dir.normalized * Vector3.Distance(enemy.transform.position, transform.position), ForceMode.Impulse);
        //acidblob.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.transform.forward.x, 1, gameObject.transform.forward.z) * 20;
    }

    public IEnumerator throwKnives()
    {
        ai.agent.speed = 0f;
        ai.agent.acceleration = 0;
        if (bradyHealth > halfHealth)
        {
            ai.transform.LookAt(knifeTarget.transform);
            bradyAnimator.SetTrigger("Throw");
            var knife = Instantiate(knifePrefab, launchPosition.position, launchPosition.rotation);
            knife.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
        }
        else
        {
            ai.transform.LookAt(knifeTarget.transform);
            bradyAnimator.SetTrigger("Throw");
            Vector3 knife2Position = new Vector3(launchPosition.position.x - 5, launchPosition.position.y, launchPosition.position.z - 5);
            Vector3 knife3Position = new Vector3(launchPosition.position.x - 10, launchPosition.position.y, launchPosition.position.z - 10);
            Vector3 knife4Position = new Vector3(launchPosition.position.x + 5, launchPosition.position.y, launchPosition.position.z + 5);
            Vector3 knife5Position = new Vector3(launchPosition.position.x + 10, launchPosition.position.y, launchPosition.position.z + 10);

            Quaternion knife2Direction = new Quaternion(launchPosition.rotation.x, launchPosition.rotation.y - 5, launchPosition.rotation.z, launchPosition.rotation.w);
            Quaternion knife3Direction = new Quaternion(launchPosition.rotation.x, launchPosition.rotation.y - 10, launchPosition.rotation.z, launchPosition.rotation.w);
            Quaternion knife4Direction = new Quaternion(launchPosition.rotation.x, launchPosition.rotation.y + 5, launchPosition.rotation.z, launchPosition.rotation.w);
            Quaternion knife5Direction = new Quaternion(launchPosition.rotation.x, launchPosition.rotation.y + 10, launchPosition.rotation.z, launchPosition.rotation.w);

            var knife1 = Instantiate(knifePrefab, launchPosition.position, launchPosition.rotation);
            var knife2 = Instantiate(knifePrefab, knife2Position, knife2Direction);
            var knife3 = Instantiate(knifePrefab, knife3Position, knife3Direction);
            var knife4 = Instantiate(knifePrefab, knife4Position, knife4Direction);
            var knife5 = Instantiate(knifePrefab, knife5Position, knife5Direction);

            knife1.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
            knife2.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
            knife3.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
            knife4.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
            knife5.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z) * 100;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        ai.agent.speed = 50f;
        ai.agent.acceleration = 200;
    }


}
