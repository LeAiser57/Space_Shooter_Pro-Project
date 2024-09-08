using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Sprite[] _lives;
    [SerializeField] private Text _gameoverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Image _livesDisplayPlayer1;
    [SerializeField] private Image _livesDisplayPlayer2;
    [SerializeField] private Image _PauseMenu;
    [SerializeField] private GameObject _HighScoreText;
    [SerializeField] private Text _HighScoreValText;
    [SerializeField] private Image _Tutorial;
    private Player _player, _player1, _player2;

    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if( _gameManager.IsSingleMode())
            _player = GameObject.Find("Player").GetComponent<Player>();
        else
        {
            _player1 = GameObject.Find("Player_1").GetComponent<Player>();
            _player2 = GameObject.Find("Player_2").GetComponent<Player>();
        }
    }

    void Update()
    {
        if (_gameManager.IsSingleMode())
        {
            _scoreText.text = "Score: " + _player.getScore();
            _livesDisplay.sprite = _lives[_player.getLives()];
        }
        else
        {
            _scoreText.text = "Score: " + (_player1.getScore() + _player2.getScore());
            _livesDisplayPlayer1.sprite = _lives[_player1.getLives()];
            _livesDisplayPlayer2.sprite = _lives[_player2.getLives()];
        }
    }

    public void gameoverDisplay()
    {
        if (_gameManager.Gameover())
        {
            _HighScoreText.gameObject.SetActive(true);
            if(_gameManager.IsSingleMode() )
                _HighScoreValText.text = "" + _player.getBestScore();
            else
            {
                _HighScoreValText.text = "" + ((_player1.getBestScore()+_player2.getBestScore())/2);
            }
            _gameoverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(_gameoverFlicker());
        }
    }

    IEnumerator _gameoverFlicker()
    {
        while(true)
        {
            _gameoverText.text = "YOU FOOL!";
            _restartText.text = "Press 'R' to restart";
            yield return new WaitForSeconds(0.5f);
            _gameoverText.text = "";
            _restartText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PauseMenuDisplay()
    {
        _PauseMenu.gameObject.SetActive(true);
    }

    public void PauseMenuHiding()
    {
        _PauseMenu.gameObject.SetActive(false);
    }

    public void TutorialDisplay()
    {
        _Tutorial.gameObject.SetActive(true);
    }

    public void TutorialHiding()
    {
        _Tutorial.gameObject.SetActive(false);
    }
}
