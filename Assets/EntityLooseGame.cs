using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLooseGame : MonoBehaviour
{
    [SerializeField] LevelReferenceSO _level;

    internal void Activate()
    {
        _level.CurrentLevel.Lose();
    }
}
