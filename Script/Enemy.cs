using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    private SpawnManager _spawnManager;
    private Player _player;
    [SerializeField] private GameObject _ExplosionPrefab;
    [SerializeField] private GameObject _LaserPrefab;
    [SerializeField] private float _fireRate = 4;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(shootRoutine());
    }
    void Update()
    {
        CalculateMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.getDamage(1f);
            }
            self_Destroy();
        }

        if(other.tag == "Bullet")
        {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser.checkIsPLayer())
            {
                Destroy(other.gameObject);
                _player.increaseScore(10);
                self_Destroy();
            }
        }

        if(other.tag == "Obstacle")
        {
            Asteroid _asteroid = other.transform.GetComponent<Asteroid>();
            _asteroid.selfDestroy();
            self_Destroy();
        }
    }

    IEnumerator shootRoutine()
    {
        while(true)
        {
            GameObject laser = Instantiate(_LaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = laser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].isEnemy();
            }
            yield return new WaitForSeconds(_fireRate);
        }
    }

    public void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 15, 0);
        }
    }

    public void self_Destroy()
    {
        Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
