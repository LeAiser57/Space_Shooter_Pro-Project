using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private int powerupID;
    [SerializeField] private AudioClip _clip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null)
            {
                if (powerupID == 0)
                {
                    player.FifLaserActive();
                }
                else if(powerupID == 1)
                {
                    player.SpeedPowerupActive();
                }
                else if(powerupID == 2)
                {
                    player.ShieldPowerupActive();
                }
            }
            Destroy(this.gameObject);
        }
    }

}
