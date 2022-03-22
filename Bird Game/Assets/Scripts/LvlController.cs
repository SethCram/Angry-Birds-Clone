using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //needed for SceneManager to work

public class LvlController : MonoBehaviour
{
    [SerializeField] string nextLevelName;
    private Monster[] monsters; // monsters array

    /// </summary>

    private void OnEnable()
    {
        monsters = FindObjectsOfType<Monster>(); //findObjects* not findObject
    }

    // Update is called once per frame
    void Update()
    {
        if (MonstersAreAllDead())
            GoToNextLvl();
    }

    private bool MonstersAreAllDead()
    {
        foreach (var monster in monsters)
        {
            if (monster.gameObject.activeSelf) //if any monther's active, return false
                return false;
        }

        return true;
    }

    private void GoToNextLvl()
    {
        Debug.Log("Go to lvl " + nextLevelName); //'Debug.Log(string)' displays its arg in the console window whenever this method called
        SceneManager.LoadScene(nextLevelName);
    }
}
