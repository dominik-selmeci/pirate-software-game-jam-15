using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() == null) return;

        string[] levelParts = SceneManager.GetActiveScene().name.Split(' ');
        int levelNumber = Convert.ToInt16(levelParts[1]);
        SceneManager.LoadScene("Level " + (levelNumber + 1));
    }
}
