using Character;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : MonoBehaviour
    {
        protected string _tag;
        protected GameObject _ability;
        protected int _size;
        protected float _timer;
        protected bool _cast;
        protected float _cooldown;

        public Ability(string tag, GameObject ability, int size, float timer, bool cast, float cooldown)
        {
            _tag = tag;
            _ability = ability;
            _size = size;
            _timer = timer;
            _cast = cast;
            _cooldown = cooldown;
        }


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