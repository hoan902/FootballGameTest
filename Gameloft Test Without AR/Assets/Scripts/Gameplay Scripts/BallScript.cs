using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            if (gameObject.transform.parent.CompareTag("Player1"))
            {
                gameObject.transform.parent.tag = "PlayerWithBall";
            }
        }
        else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            if (gameObject.transform.parent.CompareTag("Player2"))
            {
                gameObject.transform.parent.tag = "PlayerWithBall";
            }
        }
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            if (collision.gameObject.tag == "Player1")
            {
                Debug.LogWarning("Player1TouchedBall");
                gameObject.transform.parent = collision.gameObject.transform;
                if(gameObject.transform.parent.CompareTag("Player1"))
                {
                    collision.gameObject.tag = "PlayerWithBall";
                    GameObject troopMeshObj = collision.gameObject.transform.Find("Ch44").gameObject;
                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.green);
                    }
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, collision.gameObject.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
                }
                GetComponent<SphereCollider>().isTrigger = true;
            }else if (collision.gameObject.tag == "Deactivated")
            {
                gameObject.transform.parent = null;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, collision.gameObject.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
            }
            
        }else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            if (collision.gameObject.tag == "Player2")
            {
                Debug.LogWarning("Player2TouchedBall");
                gameObject.transform.parent = collision.gameObject.transform;
                if (gameObject.transform.parent.CompareTag("Player2"))
                {
                    collision.gameObject.tag = "PlayerWithBall";
                    GameObject troopMeshObj = collision.gameObject.transform.Find("Ch44").gameObject;
                    SkinnedMeshRenderer troopMeshRender = troopMeshObj.GetComponent<SkinnedMeshRenderer>();
                    Material[] troopMats = troopMeshRender.materials;
                    foreach (Material troopMat in troopMats)
                    {
                        troopMat.SetColor("_Color", Color.green);
                    }
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, collision.gameObject.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
                }
                GetComponent<SphereCollider>().isTrigger = true;
            }
            else if (collision.gameObject.tag == "Deactivated")
            {
                gameObject.transform.parent = null;
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, collision.gameObject.transform.Find("BallGrabPoint").position, 1.5f * Time.realtimeSinceStartup);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            if (other.gameObject.tag == "Player2Goal")
            {
                //P1 win
                Debug.Log("P1 win");
                UIGameScript.instance.p1Score++;
            }
        }
        else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            if (other.gameObject.tag == "PlayerGoal")
            {
                //P2 win
                Debug.Log("P2 win");
                UIGameScript.instance.p2Score++;
            }
        } 
    }
}
