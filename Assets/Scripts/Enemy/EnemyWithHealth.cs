using UnityEngine;

namespace Enemy
{
    public class EnemyWithHealth : MonoBehaviour
    {
        protected float _health;

        public float getHealth()
        {
            return _health;
        }

        public void setHealth(float health)
        {
            _health = health;
        }
    }
}