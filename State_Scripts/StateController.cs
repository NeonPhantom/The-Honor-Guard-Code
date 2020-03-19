using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public GameObject[] navPoints;
    public GameObject enemyToChase;
    public int navPointNum;
    //public float remainingDistance;
    //public Transform destination;
    public UnityStandardAssets.Characters.ThirdPerson.AICharacterControl ai;
    public GameObject enemy;
    public Player hero;
    public float detectionRange;
    public float attackRange;
    public float framesStart;
    public float framesEnd;

    public Animator enemyAnimator;
    public int enemyHealth;

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
        //navPoints = GameObject.FindGameObjectsWithTag("navpoint");
        ai = GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();
        //childrenRend = GetComponentsInChildren<Renderer>();
        SetState(new PatrolState(this));
	}
	
	void Update () {
        currentState.CheckTransitions();
        currentState.Act();

        if (InAttackRange("Player") && (!hero.invisible || !hero.Emma.activeInHierarchy))
        {
            enemyAnimator.SetTrigger("Attack");
        }

        checkHealth();
    }
    public void SetState(State state)
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
