using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private GameObject _laserPrefab; 
    [SerializeField] private GameObject _fifLaserPrefab;
    [SerializeField] private GameObject _ExplosionPrefab;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private bool _fifLaser = false;
    //[SerializeField] private bool _speedPowerupActive = false;
    [SerializeField] private float _speedMultiplie = 2f;
    [SerializeField] private bool _shieldActive = false;
    [SerializeField] private GameObject _shieldVisualize;
    [SerializeField] private GameObject _Fire1_Visualize;
    [SerializeField] private GameObject _Fire2_Visualize;
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private int _id;
    [SerializeField] private int _lives = 1;
    private float _canFire = -1f;
    private SpawnManager _SpawnManager;
    private int _score = 0;
    private UIManager _uiManager;
    private AudioSource _audio;
    private int _bestScoreSingleMode = 0;
    private int _bestScoreDualMode = 0;
    void Start()
    {
        _bestScoreSingleMode = PlayerPrefs.GetInt("HighScoreSingle", 0);
        _bestScoreDualMode = PlayerPrefs.GetInt("HighScoreDual", 0);
        _SpawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_SpawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.TutorialDisplay();
        _audio = GetComponent<AudioSource>();
        _audio.clip = _shotAudioClip;
    }
    void Update()
    {
        if (Input.anyKey)
        {
            _uiManager.TutorialHiding();
        }
        CalculateMovement();
        if(Time.time > _canFire)
        {
            if((_id == 0 && Input.GetKey(KeyCode.Space))|| 
               (_id == 1 && Input.GetKey(KeyCode.Space))||
               (_id == 2 && Input.GetKey(KeyCode.Return)))
            fireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = 0, verticalInput = 0;
        Vector3 direction = new Vector3(0,0,0);
        if(_id == 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
        }
        else if(_id == 1)
        {
            horizontalInput = Input.GetAxis("Horizontal1");
            verticalInput = Input.GetAxis("Vertical1");
        }
        else if (_id == 2)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
        }

        direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 4), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void fireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_fifLaser)
        {
            Instantiate(_fifLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audio.Play();
    }
    public void getDamage(float dmg)
    {
        if (_shieldActive)
        {
            return;
        }
        _lives-=(int)dmg;
        if(_lives == 2)
        {
            _Fire1_Visualize.SetActive(true);
        }
        if(_lives == 1)
        {
            _Fire2_Visualize.SetActive(true);
        }
        if (_lives<1)
        {
            seflDestroy();
        }
    }
    public void FifLaserActive()
    {
        _fifLaser = true;
        StartCoroutine(FifLaserAtiveRoutine());
    }
    IEnumerator FifLaserAtiveRoutine()
    {
        yield return new WaitForSeconds(7f);
        _fifLaser = false;
    }
    public void SpeedPowerupActive()
    {
        _speed *= _speedMultiplie;
        StartCoroutine(SpeedPowerupActiveRoutine());
    }

    public void ShieldPowerupActive()
    {
        StartCoroutine(ShieldPowerupActiveRoutine());
    }

    IEnumerator SpeedPowerupActiveRoutine()
    {
        yield return new WaitForSeconds(7f);
        _speed/= _speedMultiplie;
    }
    IEnumerator ShieldPowerupActiveRoutine()
    {
        _shieldActive=true;
        _shieldVisualize.SetActive(true);
        yield return new WaitForSeconds(7f);
        _shieldActive = false;
        _shieldVisualize.SetActive(false);
    }
    public void increaseScore(int score)
    {
        _score+=score;
        if(_score > _bestScoreSingleMode && _id == 0)
        {
            _bestScoreSingleMode = _score;
            PlayerPrefs.SetInt("HighScoreSingle", _bestScoreSingleMode);
        }
        else if(_score > _bestScoreDualMode && (_id == 1||_id==2))
        {
            _bestScoreDualMode = _score;
            PlayerPrefs.SetInt("HighScoreDual", _bestScoreDualMode);
        }
    }
    public int getScore()
    {
        return _score;
    }
    public int getLives()
    {
        if (_lives < 0) _lives = 0;
        return _lives;
    }

    public int getBestScore()
    {
        if (_id == 0) return _bestScoreSingleMode;
        return _bestScoreDualMode;
    }

    private void seflDestroy()
    {
        transform.parent = null;
        _SpawnManager.onPlayerDeath();
        _uiManager.gameoverDisplay();
        Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.25f);
    }
}