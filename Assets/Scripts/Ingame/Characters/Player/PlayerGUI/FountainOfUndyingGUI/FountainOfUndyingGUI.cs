using System;
using UnityEngine;
using UnityEngine.UI;
using Warborn.Ingame.Settings;

public class FountainOfUndyingGUI : MonoBehaviour
{
    [SerializeField] private Text numberOfMinions;

    public event Action<int> onSpawnArmy;
    public void SpawnArmy()
    {
        if (!int.TryParse(numberOfMinions.text, out int _amountOfArmy)) { return; }

        onSpawnArmy?.Invoke(_amountOfArmy);

        HideTheGUI();
    }

    public void HideTheGUI()
    {
        CursorSettings.LockCursor();
        this.gameObject.SetActive(false);
    }

}
