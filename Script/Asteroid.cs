using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotateSpeed = 3.5f;
    [SerializeField] private GameObject _Explosion;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bullet")
        {
            selfDestroy();
            Destroy(other.gameObject);
        }
        if(other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();
            _player.getDamage(1f);
            selfDestroy();
        }
    }

    public void selfDestroy()
    {
        _spawnManager.StartRoutine();
        Instantiate(_Explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
