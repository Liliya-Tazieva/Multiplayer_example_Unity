using UnityEngine;
using UnityEngine.Networking;
namespace Assets.Scripts
{
    public class PlayerController : NetworkBehaviour
    {
        public GameObject BulletPrefab;
        public Transform BulletSpawn;
        
        // Update is called once per frame
        void Update ()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire();
            }
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        }

        [Command]
        private void CmdFire()
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = Instantiate(BulletPrefab,  BulletSpawn.position, BulletSpawn.rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            // Spawn the bullet on the Clients
            NetworkServer.Spawn(bullet);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }
}
