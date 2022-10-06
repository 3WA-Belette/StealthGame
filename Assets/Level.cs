using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] LevelReferenceSO _levelRef;

    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;
    [SerializeField] PlayerBrain _playerBrain;

    private void Start()
    {
        _levelRef.CurrentLevel = this;

    }

    public void Lose()
    {
        _loseScreen.SetActive(true);
        _playerBrain.StopInput();
    }

    public void Win()
    {
        _winScreen.SetActive(true);
        _playerBrain.StopInput();
    }

}
