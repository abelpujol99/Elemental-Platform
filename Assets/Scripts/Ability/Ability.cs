using Character;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected string _tag;
        [SerializeField] protected GameObject _ability;
        [SerializeField] protected int _size;
        [SerializeField] protected float _timer;
        [SerializeField] protected bool _cast;
        [SerializeField] protected float _cooldown;

        public string getTag()
        {
            return _tag;
        }

        public GameObject getAbility()
        {
            return _ability;
        }

        public int getSize()
        {
            return _size;
        }

        public float getTimer()
        {
            return _timer;
        }

        public bool isCast()
        {
            return _cast;
        }

        public float getCooldown()
        {
            return _cooldown;
        }

        public void setTag(string tag)
        {
            _tag = tag;
        }

        public void setAbility(GameObject ability)
        {
            _ability = ability;
        }

        public void setSize(int size)
        {
            _size = size;
        }

        public void setTimer(float timer)
        {
            _timer = timer;
        }

        public void setCast(bool cast)
        {
            _cast = cast;
        }

        public void setCoolDown(float cooldown)
        {
            _cooldown = cooldown;
        }
        
        public abstract void abilityUtility(GameObject ability, Vector3 position, GameObject character);
    }
}