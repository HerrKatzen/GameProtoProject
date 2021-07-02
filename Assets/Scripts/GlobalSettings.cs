using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public string Difficulty { get; set; }

    public static GlobalSettings Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

}
