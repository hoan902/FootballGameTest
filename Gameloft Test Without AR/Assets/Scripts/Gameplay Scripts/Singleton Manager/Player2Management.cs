using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Management : MonoBehaviour
{
    #region Singleton
    public static Player2Management instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject Player2;
}
