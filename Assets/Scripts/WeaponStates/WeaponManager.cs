using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    #region Fire Rate
    [Header("Fire Rate")]
    [SerializeField]
    float fireRate;

    [SerializeField]
    bool semiAuto;
    float fireRateTimer;
    #endregion

    #region Bullet Properties
    [Header("Bullet Properties")]
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform barrelPos;

    [SerializeField]
    float bulletVelocity;

    [SerializeField]
    float bulletsPerShot;
    public float damage = 20;
    AimStateManager aim;
    #endregion

    #region Weapon
    [SerializeField]
    AudioClip gunShot;

    [HideInInspector]
    public AudioSource audioSource;

    [HideInInspector]
    public WeaponAmmo ammo;
    WeaponBloom bloom;
    ActionStateManager actions;
    WeaponRecoil recoil;
    Light muzzleFlash;
    ParticleSystem muzzleParticles;
    float lightIntensity;

    [SerializeField]
    float lightReturnSpeed = 20;
    #endregion

    public float enemyKickbackForce = 100;

    public Transform leftHandTarget,
        leftHandHint;
    public Text In,
        Size,
        Left,
        Equipped;
    public string weaponName;
    WeaponClassManager weaponClass;

    // Start is called before the first frame update
    void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
        bloom = GetComponent<WeaponBloom>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlash = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlash.intensity;
        muzzleFlash.intensity = 0;
        muzzleParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    private void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            ammo = GetComponent<WeaponAmmo>();
            audioSource = GetComponent<AudioSource>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
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
        In.text = "" + ammo.currentAmmo.ToString();
        Size.text = "" + ammo.clipSize.ToString();
        Left.text = "" + ammo.extraAmmo.ToString();
        Equipped.text = "" + weaponName.ToString();
        //Debug.Log(ammo.currentAmmo);
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate)
        {
            return false;
        }
        if (ammo.currentAmmo == 0)
        {
            return false;
        }
        if (actions.currentState == actions.Reload)
        {
            return false;
        }
        if (actions.currentState == actions.Swap)
        {
            return false;
        }
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0))
        {
            return true;
        }
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Fire()
    {
        fireRateTimer = 0;
        ammo.currentAmmo--;

        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.BloomAngle(barrelPos);

        audioSource.PlayOneShot(gunShot);
        TriggerMuzzleFlash();
        recoil.TriggerRecoil();

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            bulletScript.weapon = this;
            bulletScript.dir = barrelPos.transform.forward;

            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleParticles.Play();
        muzzleFlash.intensity = lightIntensity;
    }
}
