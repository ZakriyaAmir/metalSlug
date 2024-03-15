using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunAndGun.Space
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private int ammoCapacity = 15;
        [SerializeField] private Transform bulletPrefab;
        [SerializeField] public bool infiniteAmmo = false;
        [SerializeField] private float bulletSpeed = 300f;
        [SerializeField] private float fireRateInSeconds = 0.2f;
        [SerializeField] private float reloadSpeed = 1f;
        [SerializeField] private string weaponAudioName;
        private RecoilControl recoilControl;
        private Transform[] ammo;
        private SimpleBullet[] bulletRefernce;
        private int ammoLeft;
        private int ammoIndex;
        private float fireRateTimer = 0f;
        private float reloadTimer = 0f;
        public bool reloading = false;
        public bool defaultWeapon;
        public Transform leftHandIk;
        public Transform rightHandIk;

        private void Awake()
        {
            GameManager.Instance.weapon = this;
        }

        private void OnEnable()
        {
            Invoke("setLeftHandPosition", 0.1f);
        }

        private void setLeftHandPosition() 
        {
            GameManager.Instance.UpdateAmmo();
            GameManager.Instance.playerMovement.leftHandIK.data.target.position = leftHandIk.position;
            GameManager.Instance.playerMovement.leftHandIK.data.target.rotation = leftHandIk.rotation;
        }


        private void Start()
        {
            ammoLeft = ammoCapacity;
            GlobalBuffer.gamePoints.CurrentAmmoCount = ammoLeft;
            recoilControl = GameManager.Instance.recoilControl;
            StartCoroutine(InstantiateAmmoCapacity());
            if (defaultWeapon)
            {
                GameManager.Instance.defaultWeapon = this;
            }
        }

        private void Update()
        {
            if (fireRateTimer > 0)
            {
                fireRateTimer -= Time.deltaTime;
            }
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
                if(reloadTimer < 0)
                {
                    ReloadWeaponEnd();
                }
            }
        }

        private IEnumerator InstantiateAmmoCapacity()
        {
            ammo = new Transform[ammoCapacity];
            bulletRefernce = new SimpleBullet[ammoCapacity];
            ammoIndex = 0;
            for (int i = 0; i < ammoCapacity; i++)
            {
                ammo[i] = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bulletRefernce[i] = ammo[i].GetComponent<SimpleBullet>();
            }
            yield return null;
        }

        public void TryShoot()
        {
            if (infiniteAmmo)
            {
                if (ammoLeft <= 0)
                {
                    ammoLeft = ammoCapacity;
                }
            }
            if (ammoLeft > 0 && !reloading )
            {
                Shoot();
            }
            else
            {
                ReloadWeaponStart();
            }
        }

        private void Shoot()
        {
            if (fireRateTimer <= 0)
            {
                fireRateTimer = fireRateInSeconds;
                ammoLeft--;
                ammo[ammoIndex].position = firePoint.position;
                bulletRefernce[ammoIndex].SendBullet(firePoint.transform.forward, bulletSpeed);
                ammoIndex++;
                if(ammoIndex > ammoCapacity - 1)
                {
                    ammoIndex = 0;
                }
                recoilControl.CallRecoil();
                GlobalBuffer.gamePoints.CurrentAmmoCount = ammoLeft;
                GameManager.Instance.UpdateAmmo();
                audioManager.instance.PlayAudio(weaponAudioName, true, transform.position);
            }
        }

        public void ReloadWeaponStart()
        {
            if(!reloading && ammoLeft != ammoCapacity)
            {
                Debug.Log("Select Default Gun");
                GameManager.Instance.selectDefaultWeapon();
                StartCoroutine(destroyAmmo());
                Destroy(gameObject);
            }
        }

        public IEnumerator destroyAmmo() 
        {
            foreach (Transform arm in ammo) 
            {
                Destroy(arm.gameObject);
            }
            yield return null;
        }

        private void ReloadWeaponEnd()
        {
            reloadTimer = 0f;
            ammoLeft = ammoCapacity;
            reloading = false;
            GlobalBuffer.gamePoints.CurrentAmmoCount = ammoLeft;
            GameManager.Instance.UpdateAmmo();
            GameManager.Instance.ReloadWeaponEnd();
        }
    }
}