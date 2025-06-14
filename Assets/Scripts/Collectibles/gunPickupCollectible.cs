using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Collider))]
    public class gunPickupCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] public GameObject gunPrefab;
        private bool taken = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!taken && (targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                GiveGun(other.GetComponent<PlayerMovement>());
            }
        }

        private void GiveGun(PlayerMovement player)
        {
            //Play weapon pickup sound
            audioManager.instance.PlayAudio("pickWeapon", true, transform.position);
            
            taken = true;
            foreach (Transform gun in player.gunsParent) 
            {
                if (!gun.GetComponent<Weapon>().defaultWeapon)
                {
                    //Destroy every gun except the pistol
                    StartCoroutine(gun.GetComponent<Weapon>().destroyAmmo());
                    Destroy(gun.gameObject);
                }
                else 
                {
                    //Hide and keep pistol
                    gun.gameObject.SetActive(false);
                }
            }
            GameObject spawnedGun = Instantiate(gunPrefab, player.gunsParent);
            player.GetComponent<PlayerInput>().weapon = spawnedGun.GetComponent<Weapon>();
            Destroy(this.gameObject);
        }
    }
}