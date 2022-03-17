using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EffectsDatabase : NetworkBehaviour
{
    public static EffectsDatabase Instance;
    public List<Effect> AllEffects = new List<Effect>();

    void Start()
    {
        if (Instance == null) { Instance = this; }
    }
}
