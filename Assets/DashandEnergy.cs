using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DashandEnergy : MonoBehaviour
{
    public float dashDistance = 5f; // Jarak dash
    public float dashDuration = 0.5f; // Durasi dash
    public float dashCooldown = 1f; // Waktu tunggu antara dash
    public int maxEnergy = 100; // Energi maksimum
    public int dashEnergyCost = 25; // Energi yang dibutuhkan untuk dash
    public float energyRechargeRate = 40f; // Laju pengisian energi per detik
    public Slider energySlider; // Referensi ke Slider UI untuk energi

    private float currentEnergy; // Ubah dari int ke float
    private float lastDashTime;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentEnergy = maxEnergy; // Set energi awal
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            RechargeEnergy();
        }
    }

    private void RechargeEnergy()
    {
        // Tambahkan energi berdasarkan laju pengisian per detik
        currentEnergy += energyRechargeRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && Time.time >= lastDashTime + dashCooldown && currentEnergy >= dashEnergyCost)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        // Kurangi energi untuk dash
        currentEnergy -= dashEnergyCost;
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }

        // Hitung posisi dash
        Vector2 dashDirection = playerController.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 startPosition = rb.position;
        Vector2 dashTargetPosition = startPosition + (dashDirection * dashDistance);

        // Mulai animasi dash
        animator.SetTrigger(AnimationStrings.dashTrigger);

        // Interpolasi untuk mencapai target dalam waktu tertentu
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            rb.MovePosition(Vector2.Lerp(startPosition, dashTargetPosition, elapsedTime / dashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set waktu dash terakhir
        lastDashTime = Time.time;
    }

}
