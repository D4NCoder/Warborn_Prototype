using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectsDatabase : MonoBehaviour
{
    private static EffectsDatabase Instance;
    [SerializeField] private string PathToEffectDatas = "";
    public List<Effect> Effects;
    public void Start()
    {
        if (Instance == null) { Instance = this; }
        InitializeEffects();
    }

    private void InitializeEffects()
    {
        Effects = new List<Effect>();

        AddNewEffect(new LSElectrocute(), nameof(LSElectrocute));
    }

    public static EffectsDatabase GetInstance()
    {
        return Instance;
    }

    public Effect GetEffectById(int id)
    {
        List<Effect> copies = Effects.Select(x => (Effect)x.Clone()).ToList();
        return copies.Where(x => x.effectData.Id == id).FirstOrDefault();
    }

    private void AddNewEffect(Effect effect, string effectName)
    {
        effect.effectData = (EffectData)Resources.Load(PathToEffectDatas + effectName);
        Effects.Add(effect);
    }
}
