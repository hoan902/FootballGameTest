using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    #region Singleton
    public static PlayerManagement instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject Player1;
}
