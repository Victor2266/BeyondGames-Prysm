using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class enemyWeapon : MonoBehaviour
{
    public ImperialKnightLongSword IKLS;
    public float knockbackX;
    public float knockbackY;
    public GameObject blowbackParticles;
    public float DMG;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IKLS.LookingLeft)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-knockbackX, knockbackY);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(knockbackX, knockbackY);
            }

            SetCollision(IKLS.player.position);
            IKLS.player.SendMessage("TakeDamage", DMG);
            CameraShaker.Instance.ShakeOnce(10f, 10f, 0.1f, 1f);

            return;
        }
    }

    public void SetCollision(Vector2 pos)//setting up position and direction of blood splatter
    {
        collisionPosition = pos;
        bloodDir = transform.position - new Vector3(pos.x, pos.y, 1f);
        ZAngle = Mathf.Atan2(bloodDir.y, bloodDir.x) * Mathf.Rad2Deg + bloodOffset;
        _lookRot = Quaternion.AngleAxis(ZAngle, Vector3.forward);
        bloodRot = _lookRot;

        Spray();
    }

    private Vector3 collisionPosition;
    private Vector3 bloodDir;
    private Quaternion bloodRot;
    public float bloodOffset;
    private float ZAngle;
    private Quaternion _lookRot;

    private GameObject lastSpray;
    public void Spray()
    {
        lastSpray = Instantiate(blowbackParticles, collisionPosition, bloodRot);
    }
}
