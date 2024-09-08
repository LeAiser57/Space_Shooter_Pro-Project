using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private bool _isPLayer = true;
    void Update()
    {
        if(_isPLayer)
        {
            MoveUp();
        }
        else if(!_isPLayer)
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7f)
        {
            selfDestroy();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            selfDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !_isPLayer)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.getDamage(1f);
                selfDestroy();
            }
            else
            {
                Debug.LogError("cant find player");
            }
        }
    }

    private void selfDestroy()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }

    public bool checkIsPLayer()
    {
        return _isPLayer;
    }

    public void isEnemy()
    {
        this._isPLayer = false; 
    }
}

