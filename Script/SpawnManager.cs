using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _PowerupPrefab;
    [SerializeField] private GameObject _Container;
    [SerializeField] private GameObject _PlayerContainer;
    private bool _isSpawning = true;
    [SerializeField] private int _maxEnemy = 15;
    public void StartRoutine()
    {
        StartCoroutine(spawnEnemyRoutine());
        StartCoroutine(spawnPowerUpRoutine());
    }

    private IEnumerator spawnEnemyRoutine()
    {
        while (_isSpawning )
        {
            if (_Container.transform.childCount < _maxEnemy)
            {
                Vector3 enemyPos = new Vector3(Random.Range(-12.0f, 12.0f), 15, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, enemyPos, Quaternion.identity);
                newEnemy.transform.parent = _Container.transform;
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private IEnumerator spawnPowerUpRoutine()
    {
        while(_isSpawning)
        {
            yield return new WaitForSeconds(10f);
            Vector3 powereupPos = new Vector3(Random.Range(-8f, 8f), 15, 0);
            int randomPowerup = Random.Range(0,3);
            Instantiate(_PowerupPrefab[randomPowerup], powereupPos, Quaternion.identity);
        }
    }

    public void onPlayerDeath()
    {
        Debug.Log(_PlayerContainer.transform.childCount);
        if(_PlayerContainer.transform.childCount == 0) _isSpawning = false;
    }
}

 