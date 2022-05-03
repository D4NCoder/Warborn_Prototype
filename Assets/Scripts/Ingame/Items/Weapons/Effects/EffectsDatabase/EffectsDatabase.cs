using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Warborn.Ingame.Items.Weapons.Effects.Core;

namespace Warborn.Ingame.Items.Weapons.Effects.EffectsDatabase
{
    public class EffectsDatabase : MonoBehaviour
    {
        private static EffectsDatabase Instance;
        [SerializeField] private string PathToEffectDatas = "";
        public List<Effect> Effects;

        #region Initialization
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
        #endregion

        #region Getters
        public static EffectsDatabase GetInstance()
        {
            return Instance;
        }
        public Effect GetEffectById(int _id)
        {
            List<Effect> _copies = Effects.Select(x => (Effect)x.Clone()).ToList();
            return _copies.Where(x => x.effectData.Id == _id).FirstOrDefault();
        }

        public List<int> GetEffectsIds(List<Effect> _effects)
        {
            return _effects.Select(x => x.effectData.Id).ToList();
        }
        #endregion

        private void AddNewEffect(Effect _effect, string _effectName)
        {
            _effect.effectData = (EffectData)Resources.Load(PathToEffectDatas + _effectName);
            Effects.Add(_effect);
        }
    }

}

