using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("채력 설정")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI 참조")]
    public Slider healthBar;

    [Header("무적 시간")]
    public float invincibleTime = 1f;
    public bool isInvincible = false;

    [Header("이펙트")]
    public SpriteRenderer spriteRenderer;

    void Start()
    {
      
    }

   
    void Update()
    {
        
    }
}
