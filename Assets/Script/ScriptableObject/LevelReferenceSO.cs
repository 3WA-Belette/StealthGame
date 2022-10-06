using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelReference")]
public class LevelReferenceSO : ScriptableObject
{
    Level _currentLevel;

    public Level CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value;
    }

}
