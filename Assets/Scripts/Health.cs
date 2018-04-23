using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Health : NetworkBehaviour
    {
        public const int MaxHealth = 100;

        [SyncVar(hook = "OnChangeHealth")]
        public int CurrentHealth = MaxHealth;

        public RectTransform HealthBar;

        public void TakeDamage(int amount)
        {
            if (!isServer)
            {
                return;
            }

            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = MaxHealth;

                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }

        void OnChangeHealth(int health)
        {
            HealthBar.sizeDelta = new Vector2(health, HealthBar.sizeDelta.y);
        }

        [ClientRpc]
        void RpcRespawn()
        {
            if (isLocalPlayer)
            {
                // move back to zero location
                transform.position = Vector3.zero;
            }
        }
    }
}
