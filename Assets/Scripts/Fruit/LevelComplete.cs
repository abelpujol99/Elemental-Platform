using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{

    public Text levelComplete;

    public GameObject transition;
    public void Update()
    {
        allFruitsCollected();       
    }
    public void allFruitsCollected()
    {
        if (transform.childCount == 0)
        {
            levelComplete.gameObject.SetActive(true);
            Invoke("ChangeScene", 2);
            transition.SetActive(true);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
