using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Character;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Enemy.Plant
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private int _projectilesNum;
        
        [SerializeField] private float _health;
        [SerializeField] private float _cadence;

        [SerializeField] private bool _fixedPosition;

        [SerializeField] private GameObject _projectile;

        private Animator _animator;

        private SpriteRenderer _spriteRenderer;

        private List<Projectile> _projectiles;

        private Dictionary<string, Queue<GameObject>> _projectileDictionary;

        private float _projectilePositionXSpawn;

        private float _cadenceAux;

        private bool _canAttack;

        private void Start()
        {
            _projectilePositionXSpawn = 1f;
            _cadenceAux = _cadence;
            _animator = GetComponent<Animator>();
            _canAttack = true;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer.flipX)
            {
                _projectilePositionXSpawn = Mathf.Abs(_projectilePositionXSpawn);
            }
            else
            {
                _projectilePositionXSpawn = Mathf.Abs(_projectilePositionXSpawn) * -1;
            }
            SetProjectiles();
        }

        private void Update()
        {
            _cadence -= Time.deltaTime;
            
            if (!_fixedPosition)
            {
                if (_target.position.x > transform.position.x)
                {
                    _spriteRenderer.flipX = true;
                    _projectilePositionXSpawn = Mathf.Abs(_projectilePositionXSpawn);
                }
                else
                {
                    _spriteRenderer.flipX = false;
                    _projectilePositionXSpawn = Mathf.Abs(_projectilePositionXSpawn) * -1;
                }
            }

            if (_canAttack && _cadence <= 0)
            {
                StartToAttack();
                Attack("Projectile");
                _cadence = _cadenceAux;
            }

        }

        private void SetProjectiles()
        {
            _projectiles = new List<Projectile>();
            
            Projectile projectileAux = _projectile.GetComponent<Projectile>();
            projectileAux.setTag("Projectile");
            projectileAux.setProjectile(_projectile);
            projectileAux.setSize(_projectilesNum);
            
            _projectiles.Add(projectileAux);

            _projectileDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Projectile projectile in _projectiles)
            {
                Queue<GameObject> projectilePool = new Queue<GameObject>();

                for (int i = 0; i < projectile.getSize(); i++)
                {
                    GameObject obj = Instantiate(projectile.getProjectile());
                    obj.SetActive(false);
                    projectilePool.Enqueue(obj);
                }
                
                _projectileDictionary.Add(projectile.getTag(), projectilePool);
                
            }
        }

        private void StartToAttack()
        {
            StartCoroutine(Attack("Projectile"));
            _animator.SetBool("Attack", true);
        }

        private IEnumerator Attack(string tag)
        {
            GameObject projectileToSpawn;

            Vector3 projectileSpawnPosition = new Vector3(transform.position.x + _projectilePositionXSpawn, transform.position.y,
                transform.position.z);

            projectileToSpawn = _projectileDictionary[tag].Dequeue();
            projectileToSpawn.transform.position = (projectileSpawnPosition - transform.position).normalized * 0.3f + transform.position;

            yield return new WaitForSeconds(0.33f);

            projectileToSpawn.SetActive(true);
            _projectileDictionary[tag].Enqueue(projectileToSpawn);
            projectileToSpawn.GetComponent<Rigidbody2D>().AddForce(new Vector2((transform.position.x + _projectilePositionXSpawn) - transform.position.x, transform.position.y - transform.position.y).normalized * 200);
            _animator.SetBool("Attack", false);
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerRespawn>().playerDamage();
            }
        }
    }
}