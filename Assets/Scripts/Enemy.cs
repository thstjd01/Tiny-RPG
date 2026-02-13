using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("공격력")]
    public int damage = 10; // 플레이어에게 주는 데미지

    [Header("이펙트")]
    public GameObject hitEffect;
    public GameObject deathEffect;

    [Header("넉백")]
    public float knockbackForce = 5f;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;

        Debug.Log($"{gameObject.name} 피격! 남은 체력: {currentHealth}");

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        StartCoroutine(HitFlash());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator HitFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    void Die()
    {
        Debug.Log($"{gameObject.name} 사망!");

        // 사망 이펙트
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}