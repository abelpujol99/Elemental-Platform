using Character;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : MonoBehaviour
    {
        private string _tag;
        private GameObject _ability;
        private int _size;
        private float _timer;
        [SerializeField] private CharacterScript character;

        public Ability(string tag, GameObject ability, int size, float timer)
        {
            _tag = tag;
            _ability = ability;
            _size = size;
            _timer = timer;
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

        public void setSize(int size)
        {
            this._size = size;
        }

        public void setTimer(float timer)
        {
            this._timer = timer;
        }
        
        public abstract void abilityUtility(string tag, Ability ability, float timer, Vector3 position, GameObject character);

        public void aseijbsi()
        {
            Debug.Log("test");
        }

    }
}