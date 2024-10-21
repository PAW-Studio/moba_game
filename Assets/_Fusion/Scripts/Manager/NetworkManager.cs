using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : NetworkBehaviour
{

    // Static instance of the singleton
    private static NetworkManager _instance;

    [Networked, OnChangedRender(nameof(CharecterIndexChnage))]
    public int charecterIndex { get; set; }


    // Property to access the singleton instance
    public static NetworkManager Instance
    {
        get
        {
            // If the instance doesn't exist, find it in the scene or create a new GameObject for it
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetworkManager>();

                // If it's still not found, create a new GameObject and attach the singleton script to it
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("NetworkManager");
                    _instance = singletonObject.AddComponent<NetworkManager>();
                }
            }
            return _instance;
        }
    }

    // Ensure this instance is not destroyed on scene load
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void CharecterIndexChnage()
    {
        Debug.Log("Charecter index ::  " + charecterIndex);
    }

    void Update()
    {
        if (HasInputAuthority && Runner.IsForward)
        {
            // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
            charecterIndex = Random.Range(1, 200);
        }
    }

}
