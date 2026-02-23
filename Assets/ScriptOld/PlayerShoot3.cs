using UnityEngine;

public class PlayerShoot3 : MonoBehaviour
{
    [SerializeField] private float muzzleVelocity;
    [SerializeField] private GameObject bulletBallPrefab;
    [SerializeField] private Transform muzzle;

    // Change Keybind in inspector
    //[SerializeField] private KeyCode ShootKey = KeyCode.None;

    GameObject Bullet;

    // Update is called once per frame
    void Update()
    {
        CannonInputs();
    }
    void CannonInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // GetKey(ShootKey)) Input when held down
        {
            FireCannon();
        }
    }
    void FireCannon()
    {
        {
            GameObject muzzle = GameObject.Find("Muzzle");
            Bullet = (GameObject)Instantiate(bulletBallPrefab, muzzle.transform.position, muzzle.transform.rotation);
            Bullet.transform.GetComponent<Rigidbody>().linearVelocity = muzzle.transform.forward * muzzleVelocity;
        }
    }
}
