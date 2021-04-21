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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Tilemap1") || collision.transform.CompareTag("Tilemap2") || collision.transform.CompareTag("Rock") || collision.transform.CompareTag("Enemy"))
            {
                gameObject.SetActive(false);
            } 
            else if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().playerDamage();
            }
        }
    }
}