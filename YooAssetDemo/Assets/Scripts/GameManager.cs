using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    private MonoBehaviour MonoBehaviour;
    public void SetMonoBehaviour(MonoBehaviour mono)
    {
        MonoBehaviour = mono;
    }
    private GameManager()
    {

    }

    public void StartCoroutine(IEnumerator routine)
    {
        MonoBehaviour.StartCoroutine(routine);
    }
}
