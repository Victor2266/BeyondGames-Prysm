using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTouchesGround : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public GameObject[] stain;

    private float TimePassed;
    private int soundsPlayed;
    public float SoundCapResetSpeed;
    public int MaxSounds;

    public AudioSource audioSource;
    public AudioClip[] sounds;

    GameObject lastStain;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        TimePassed += Time.deltaTime;
        if(TimePassed > SoundCapResetSpeed)
        {
            soundsPlayed = 0;
            TimePassed = 0;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            
        int i = 0;

        while(i < numCollisionEvents)
        {
            Vector3 pos = new Vector3 (collisionEvents[i].intersection.x, collisionEvents[i].intersection.y, -0.01f);
            Vector3 velocity = collisionEvents[i].velocity;

            if(velocity.magnitude > 0.2f)
            {
                lastStain = Instantiate(stain[Random.Range(0, stain.Length)], pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                BloodManager.instance.stains.Enqueue(lastStain);
                BloodManager.instance.currentStains++;

                while(BloodManager.instance.currentStains > BloodManager.instance.MaxStains)
                {
                    Destroy(BloodManager.instance.stains.Dequeue());
                    BloodManager.instance.currentStains--;
                }
                lastStain.transform.localScale = new Vector3(velocity.x, velocity.y, 1f);
                if(soundsPlayed < MaxSounds)
                {
                    soundsPlayed += 1;
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)], Random.Range(0.1f, 0.3f));
                }
            }

            i++;
        }
    }
}
