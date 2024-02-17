using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float reloadGameDelay = 3;
    
    public void OnGameOver()
    {
        StartCoroutine(ReloadGameCor());
    }
    
    private IEnumerator ReloadGameCor()
    {
        yield return new WaitForSeconds(reloadGameDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
