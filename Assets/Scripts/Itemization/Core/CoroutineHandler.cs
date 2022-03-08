using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    public static CoroutineHandler Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) { return; }
        Instance = this;
    }
}
