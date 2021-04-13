using UnityEngine;

namespace Ability.Abilities
{
    public class Wind : Ability, AbilityUtility
    {

        public Wind(string tag, GameObject ability, int size, float timer) : base(tag, ability, size, timer)
        {
        }
        
        public void abilityUtility(string tag, Ability ability, float timer, Vector3 position, GameObject character)
        {
            float xPositionSpawn;
            
            if (transform.position.x < position.x)
            {
                xPositionSpawn = 0.2f;
            }
            else
            {
                xPositionSpawn = -0.2f;
            }
            float angle = Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            ability.transform.position = new Vector3( transform.position.x + xPositionSpawn, transform.position.y, 0);
            //abilityToSpawn.transform.position = transform.position;
            ability.transform.rotation = targetRotation;
            ability.GetComponent<Rigidbody2D>().AddForce(new Vector2(position.x - transform.position.x, position.y - transform.position.y) * 100);
        }
        
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            gameObject.SetActive(false);
        }
    }
}