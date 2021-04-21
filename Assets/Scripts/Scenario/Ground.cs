using Character;
using UnityEngine;

namespace Scenario
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private CharacterScript character;
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                character.isGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.transform.CompareTag("CheckGround"))
            {
                character.isGround = false;
            }
        }
        
    }

}
