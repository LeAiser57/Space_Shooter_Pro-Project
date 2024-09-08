using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private bool _isGameover;
    [SerializeField] private bool _isSingleMode = true;
    [SerializeField] private GameObject PlayerContainer;
    [SerializeField] private UIManager _UIManager;
    private void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameover)
        {
            if(_isSingleMode)
                SceneManager.LoadScene(1);
            else SceneManager.LoadScene(2);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _UIManager.PauseMenuDisplay();
            pauseGame();
        }
    }

    public bool Gameover()
    {
        _isGameover = (PlayerContainer.transform.childCount == 0);
        return _isGameover;
    }

    public bool IsSingleMode()
    {
        return _isSingleMode;
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _UIManager.PauseMenuHiding();
    }

    public void returnMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
