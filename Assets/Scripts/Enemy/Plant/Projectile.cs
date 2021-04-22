using System;
using Character;
using UnityEngine;

namespace Enemy.Plant
{
    public class Projectile : MonoBehaviour
    {
        private string _tag;
        private GameObject _projectile;
        private int _size;

        public string getTag()
        {
            return _tag;
        }

        public GameObject getProjectile()
        {
            return _projectile;
        }

        public int getSize()
        {
            return _size;
        }

        public void setTag(string tag)
        {
            _tag = tag;
        }

        public void setProjectile(GameObject projectile)
        {
            _projectile = projectile;
        }

        public void setSize(int size)
        {
            _size = size;
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("Tilemap1") || trigger.transform.CompareTag("Tilemap2") || trigger.transform.CompareTag("Rock") || trigger.transform.CompareTag("Enemy") || trigger.transform.CompareTag("GreenProjectile"))
            {
                gameObject.SetActive(false);
            } 
            else if (trigger.transform.CompareTag("Player"))
            {
                trigger.transform.GetComponent<PlayerRespawn>().playerDamage();
            }
        }
    }
}