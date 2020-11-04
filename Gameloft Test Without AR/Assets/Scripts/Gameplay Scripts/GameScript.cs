using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject AtkZone;
    public GameObject DefZone;
    public GameObject Ball;
    private bool m_spawned = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIGameScript.instance.RoleP1 == "(Attacker)")
        {
            if (m_spawned == false)
            {
                SpawnBallP1();
            }
        }
        else if (UIGameScript.instance.RoleP1 == "(Defender)")
        {
            if (m_spawned == false)
            {
                SpawnBallP2();
            }
        }
    }
    private void SpawnBallP1()
    {
        Vector3 pos = new Vector3(
            Random.Range(-AtkZone.transform.position.x + 2, AtkZone.transform.position.x - 2),
            AtkZone.transform.position.y,
            Random.Range(AtkZone.transform.position.z + 2, AtkZone.transform.position.z - 2)
            );
        Instantiate(Ball, pos, Quaternion.identity);
        m_spawned = true;
    }

    private void SpawnBallP2()
    {
        Vector3 pos = new Vector3(
                Random.Range(-DefZone.transform.position.x + 2, DefZone.transform.position.x - 2),
                AtkZone.transform.position.y,
                Random.Range(DefZone.transform.position.z + 2, DefZone.transform.position.z - 2)
                );
        Instantiate(Ball, pos, Quaternion.identity);
        m_spawned = true;
    }

    //when math end, reset m_spawned to false (not code yet)

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
