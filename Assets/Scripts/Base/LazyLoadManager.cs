using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyLoadManager<AnyClass> : MonoBehaviour where AnyClass : MonoBehaviour
{
    private static AnyClass _instance;

    private void Awake() {
        Instance();
    }
    private AnyClass Instance (){
        if (_instance is null) _instance = FindObjectOfType<AnyClass>();
        return _instance;
    }
    public static AnyClass Get(){
        return _instance;
    }
}