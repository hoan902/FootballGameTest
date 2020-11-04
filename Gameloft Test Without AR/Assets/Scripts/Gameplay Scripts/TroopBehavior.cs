using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TroopBehavior : MonoBehaviour
{
    public float detectorZone = 1f;

    public float atkNormalSpeed = 1.5f;
    public float defNormalSpeed = 1;
    public float ballCarrySpeed = 0.75f;

    bool isDeactivated = false;

    public GameObject troop;
    public NavMeshAgent troopAgent;

    public GameObject atkZone;
    public GameObject defZone;

    public string p1Tag = "Player1";
    public string p2Tag = "Player2";
    public string playerHoldBallTag = "PlayerWithBall";

    private Vector3 m_SpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        m_SpawnPos = transform.position;
        troop = GetComponent<GameObject>();
        troopAgent = GetComponent<NavMeshAgent>();
        InvokeRepeating("UpadteTarget", 0f, 2.5f);
        Transform originTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpadteTarget()
    {
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            StartCoroutine(UpdateP1Attack());
        }
        else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            StartCoroutine(UpdateP2Attack());
        }
    }

    void FaceBall()
    {
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject Ball in Balls)
        {
            Vector3 direction = (Ball.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    #region CollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            if (gameObject.CompareTag(playerHoldBallTag))
            {
                if (collision.gameObject.CompareTag(p2Tag))
                {
                    isDeactivated = true;
                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.gray);
                    }

                }
            }
            else if (gameObject.CompareTag(p2Tag))
            {
                if (collision.gameObject.CompareTag(playerHoldBallTag))
                {
                    isDeactivated = true;
                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.gray);
                    }
                }
            }
            if (collision.gameObject.CompareTag("P2Fence") && gameObject.CompareTag(p1Tag))
            {
                if(!gameObject.transform.Find("ball(clone)"))
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            if (gameObject.CompareTag(playerHoldBallTag))
            {
                if (collision.gameObject.CompareTag(p1Tag))
                {
                    isDeactivated = true;
                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.gray);
                    }
                }
            }
            else if (gameObject.CompareTag(p1Tag))
            {
                if (collision.gameObject.CompareTag(playerHoldBallTag))
                {
                    isDeactivated = true;
                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.gray);
                    }
                }
            }
            if (collision.gameObject.CompareTag("P1Fence") && gameObject.CompareTag(p2Tag))
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion

    #region IEnumerator Update
    private IEnumerator UpdateP1Attack()
    {
        GameObject[] playerHoldBall = GameObject.FindGameObjectsWithTag(playerHoldBallTag);
        GameObject[] P1Troops = GameObject.FindGameObjectsWithTag(p1Tag);
        GameObject[] P2Troops = GameObject.FindGameObjectsWithTag(p2Tag);
        GameObject[] DeactivatedTroops = GameObject.FindGameObjectsWithTag("Deactivated");
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject p1Goal = GameObject.FindGameObjectWithTag("PlayerGoal");
        GameObject p2Goal = GameObject.FindGameObjectWithTag("Player2Goal");


        Debug.Log("Ded: " + DeactivatedTroops.LongLength + "++" + "Live" + P1Troops.LongLength);
        if (DeactivatedTroops.LongLength > 0)
        {
            yield return new WaitForSeconds(1f);
            if (playerHoldBall.Length == 0 && P1Troops.Length == 0)
            {
                Debug.Log("Def win");
                UIGameScript.instance.p2Score++;
            }
        }


        // player 1 behavior
        if (CompareTag(p1Tag))
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject Ball in Balls)
            {
                detectorZone = 9f;
                float BallDistance = Vector3.Distance(Ball.transform.position, m_SpawnPos);
                if (GameObject.FindGameObjectWithTag(playerHoldBallTag))
                {
                    MeshCollider AtkTroopCol = GetComponent<MeshCollider>();
                    AtkTroopCol.isTrigger = true;
                    troopAgent.SetDestination(new Vector3(transform.position.x, transform.position.y, -1.4f));
                    troopAgent.speed = atkNormalSpeed;
                    if (transform.position == troopAgent.destination)
                    {
                        AtkTroopCol.isTrigger = false;
                    }
                }
                else
                {
                    if (BallDistance <= detectorZone)
                    {
                        troopAgent.SetDestination(Ball.transform.position);
                        troopAgent.speed = atkNormalSpeed;
                        if (BallDistance <= troopAgent.stoppingDistance)
                        {
                            FaceBall();
                        }
                    }
                }
            }
        }


        //Player that have the ball
        else if (CompareTag(playerHoldBallTag))
        {
            foreach (GameObject Ball in Balls)
            {
                troopAgent.SetDestination(p2Goal.transform.position);
                troopAgent.speed = 0.75f;
                if (isDeactivated)
                {
                    Debug.Log("now stop atk");
                    gameObject.tag = "Deactivated";
                    troopAgent.enabled = false;
                    gameObject.transform.position = transform.position;
                    foreach (GameObject p1Troop in P1Troops)
                    {
                        SphereCollider ballCollider = Ball.GetComponent<SphereCollider>();
                        Rigidbody ballRd = Ball.GetComponent<Rigidbody>();
                        //NavMeshAgent ballAgent = Ball.GetComponent<NavMeshAgent>();
                        ballCollider.isTrigger = false;
                        Ball.transform.parent = null;
                        //float BallDistance = Vector3.Distance(p1Troop.transform.position, Ball.transform.position);
                        // ballAgent.SetDestination(p1Troop.transform.position);
                        //ballAgent.speed = 15f;
                        if (p1Troop.CompareTag(p1Tag))
                        {
                            //p1Troop.transform.position = gameObject.transform.Find("BallGrabPoint").position;
                            Ball.transform.position = Vector3.MoveTowards(Ball.transform.position, p1Troop.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
                        }
                    }


                    yield return new WaitForSeconds(2.5f);

                    troopAgent.enabled = true;
                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.blue);
                    }
                    gameObject.tag = p1Tag;
                    Debug.Log("now active atk");
                    isDeactivated = false;
                }
            }
        }



        //player 2 behavior
        else if (CompareTag(p2Tag))
        {
            yield return new WaitForSeconds(0.5f);
            detectorZone = 2f;
            foreach (GameObject troopHoldBall in playerHoldBall)
            {
                float distanceP1HoldBall = Vector3.Distance(troopHoldBall.transform.position, m_SpawnPos);
                if (distanceP1HoldBall <= detectorZone)
                {
                    troopAgent.SetDestination(troopHoldBall.transform.position);
                    troopAgent.speed = 1;
                }
                if (isDeactivated)
                {
                    troopAgent.SetDestination(m_SpawnPos);
                    troopAgent.speed = 2;
                    
                    Debug.Log("now stop def");


                    yield return new WaitForSeconds(4f);

                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.red);
                    }
                    troopAgent.isStopped = false;
                    Debug.Log("now active def");
                    isDeactivated = false;
                }
            }
        }
    }



    private IEnumerator UpdateP2Attack()
    {
        
        GameObject[] playerHoldBall = GameObject.FindGameObjectsWithTag(playerHoldBallTag);
        GameObject[] P1Troops = GameObject.FindGameObjectsWithTag(p1Tag);
        GameObject[] P2Troops = GameObject.FindGameObjectsWithTag(p2Tag);
        GameObject[] DeactivatedTroops = GameObject.FindGameObjectsWithTag("Deactivated");
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject p1Goal = GameObject.FindGameObjectWithTag("PlayerGoal");
        GameObject p2Goal = GameObject.FindGameObjectWithTag("Player2Goal");


        Debug.Log("Ded: " + DeactivatedTroops.LongLength + "++" + "Live" + P2Troops.LongLength);
        if (DeactivatedTroops.LongLength > 0)
        {
            yield return new WaitForSeconds(1f);
            if (playerHoldBall.Length == 0 && P2Troops.Length == 0)
            {
                Debug.Log("Def win");
                UIGameScript.instance.p1Score++;
            }
        }


        // player 2 behavior (atk)
        if (CompareTag(p2Tag))
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject Ball in Balls)
            {
                detectorZone = 9f;
                float BallDistance = Vector3.Distance(Ball.transform.position, m_SpawnPos);
                if (GameObject.FindGameObjectWithTag(playerHoldBallTag))
                {
                    MeshCollider AtkTroopCol = GetComponent<MeshCollider>();
                    AtkTroopCol.isTrigger = true;
                    troopAgent.SetDestination(new Vector3(transform.position.x, transform.position.y, -9));
                    troopAgent.speed = atkNormalSpeed;
                    if (transform.position == troopAgent.destination)
                    {
                        AtkTroopCol.isTrigger = false;
                    }
                }
                else
                {
                    if (BallDistance <= detectorZone)
                    {
                        troopAgent.SetDestination(Ball.transform.position);
                        troopAgent.speed = atkNormalSpeed;
                        if (BallDistance <= troopAgent.stoppingDistance)
                        {
                            FaceBall();
                        }
                    }
                }
            }
        }


        //Player that have the ball
        else if (CompareTag(playerHoldBallTag))
        {
            foreach (GameObject Ball in Balls)
            {
                troopAgent.SetDestination(p1Goal.transform.position);
                troopAgent.speed = 0.75f;
                if (isDeactivated)
                {
                    Debug.Log("now stop atk");
                    gameObject.tag = "Deactivated";
                    troopAgent.isStopped = true;
                    troopAgent.transform.position = transform.position;
                    foreach (GameObject p2Troop in P2Troops)
                    {
                        SphereCollider ballCollider = Ball.GetComponent<SphereCollider>();
                        Rigidbody ballRd = Ball.GetComponent<Rigidbody>();
                        //NavMeshAgent ballAgent = Ball.GetComponent<NavMeshAgent>();
                        ballCollider.isTrigger = false;
                        detectorZone = 9f;
                        Ball.transform.parent = null;
                        //float BallDistance = Vector3.Distance(p1Troop.transform.position, Ball.transform.position);
                        // ballAgent.SetDestination(p1Troop.transform.position);
                        //ballAgent.speed = 15f;
                        if (p2Troop.CompareTag(p2Tag))
                        {
                            //p1Troop.transform.position = gameObject.transform.Find("BallGrabPoint").position;
                            Ball.transform.position = Vector3.MoveTowards(Ball.transform.position, p2Troop.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
                        }
                    }


                    yield return new WaitForSeconds(2.5f);

                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.blue);
                    }
                    gameObject.tag = p1Tag;
                    Debug.Log("now active atk");
                    troopAgent.isStopped = false;
                    isDeactivated = false;
                }
            }
        }



        //player 1 behavior (Def)
        else if (CompareTag(p1Tag))
        {
            yield return new WaitForSeconds(0.5f);
            detectorZone = 2f;
            foreach (GameObject troopHoldBall in playerHoldBall)
            {
                float distanceP1HoldBall = Vector3.Distance(troopHoldBall.transform.position, m_SpawnPos);
                if (distanceP1HoldBall <= detectorZone)
                {
                    troopAgent.SetDestination(troopHoldBall.transform.position);
                    troopAgent.speed = 1;
                }
                if (isDeactivated)
                {
                    troopAgent.SetDestination(m_SpawnPos);
                    troopAgent.speed = 2;

                    Debug.Log("now stop def");


                    yield return new WaitForSeconds(4f);

                    GameObject troopMeshObj = gameObject.transform.Find("Ch44").gameObject;

                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.red);
                    }
                    troopAgent.isStopped = false;
                    Debug.Log("now active def");
                    isDeactivated = false;
                }
            }
        }
    }
    #endregion
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectorZone);
    }
}
