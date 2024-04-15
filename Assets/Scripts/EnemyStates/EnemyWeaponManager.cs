using System.Collections;
using System.Drawing;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    #region Fire Rate
    [Header("Fire Rate")]
    [SerializeField]
    float fireRate = 0.25f;
    float fireRateTimer;
    #endregion

    #region Bullet Properties
    [Header("Bullet Properties")]
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform barrelPos;

    [SerializeField]
    float bulletVelocity = 500;

    [SerializeField]
    float bulletsPerShot = 1;
    public float damage = 5;
    #endregion

    #region Weapon Audio
    [Header("Weapon Audio")]
    public AudioClip gunShot;
    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip releaseSlideSound;

    [HideInInspector]
    public AudioSource audioSource;
    #endregion

    #region Weapon Ammo
    [Header("Weapon Ammo")]
    public int clipSize = 30;
    public int currentAmmo = 30;
    #endregion

    #region Visual Stuff
    [Header("Extra")]
    Light muzzleFlash;
    ParticleSystem muzzleParticles;
    float lightIntensity;
    public float lightReturnSpeed = 20;

    public Transform leftHandTarget,
        leftHandHint;

    float currentBloom;
    #endregion

    public bool isReloading = false;

    WeaponIK weaponIk;
    Animator anim;

    Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        muzzleFlash = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlash.intensity;
        muzzleFlash.intensity = 0;
        muzzleParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
        weaponIk = GetComponent<WeaponIK>();
    }

    public void SetTarget(Transform target){
        weaponIk.SetTargetTransform(target);
        currentTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire())
        {
            Fire();
        }
        muzzleFlash.intensity = Mathf.Lerp(
            muzzleFlash.intensity,
            0,
            lightReturnSpeed * Time.deltaTime
        );
        //Debug.Log(ammo.currentAmmo);
    }

    bool ShouldFire()
    {
        if (currentAmmo == 0)
        {
            Debug.Log("NoAmmo");
            return false;
        }
        if (isReloading == true)
        {
            Debug.Log("Reloading");
            return false;
        }
        else
        {
            return false;
        }
    }

    void Fire()
    {
        fireRateTimer = 0;
        currentAmmo--;

        audioSource.PlayOneShot(gunShot);
        TriggerMuzzleFlash();

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            bulletScript.dir = barrelPos.transform.forward;

            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }

        if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    private void Reload()
    {
        isReloading = true;
        anim.SetBool("Reloading", true);
        currentAmmo += clipSize;
    }

    public Vector3 BloomAngle(Transform barrelPos)
    {
        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation = new Vector3(randX, randY, randZ);
        return barrelPos.localEulerAngles + randomRotation;
    }

    void TriggerMuzzleFlash()
    {
        muzzleParticles.Play();
        muzzleFlash.intensity = lightIntensity;
    }
}
