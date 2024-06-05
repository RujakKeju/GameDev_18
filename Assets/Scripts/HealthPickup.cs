using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 15;
    public float floatAmplitude = 0.25f; 
    public float floatFrequency = 5f;   
    private Vector3 startPosition;

    AudioSource pickupSource;


    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        startPosition = transform.position; // Menyimpan posisi awal
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damage = collision.GetComponent<Damageable>();

        if (damage)
        {
            bool wasHealed = damage.Heal(healthRestore);

            if (wasHealed)
            {
                if(pickupSource)
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                Destroy(gameObject);

            }
        }
    }

    private void Update()
    {
        // Hitung posisi baru dengan fungsi sinus untuk melayang
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Set posisi baru
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
