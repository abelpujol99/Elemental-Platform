using UnityEngine;

namespace Enemy
{
    public class EnemyWithHealth : MonoBehaviour
    {
        [SerializeField] protected float _health;

        public float getHealth()
        {
            return _health;
        }

        public void setHealth(float health)
        {
            this._health = health;
        }
    }
}