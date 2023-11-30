using AdventureStorm.Gameplay.Player;
using UnityEngine;

namespace AdventureStorm.Gameplay.Key
{
    public class PickupKey : MonoBehaviour
    {
        #region Events / Delegates

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.PickupKey();
                Destroy(gameObject);
            }
        }

        #endregion
    }
}