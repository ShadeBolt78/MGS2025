using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float inspectorMaxHealth = 100f;
    [SerializeField] private float inspectorDamageIncrement = 10f;
    [SerializeField] private float inspectorRegenIncrement = 10f;
    [SerializeField] private Image inspectorHealthBar;

    public static Image healthBar;
    public static float maxHealth = 100f;
    public static float health;
    public static float regenIncrement = 10f;
    public static float damageIncrement = 10f;

    void Start()
    {
        maxHealth = inspectorMaxHealth;
        damageIncrement = inspectorDamageIncrement;
        regenIncrement = inspectorRegenIncrement;
        healthBar = inspectorHealthBar;
        health = maxHealth;
    }

    public static void TakeDamage()
    {
        if (health - damageIncrement < 0) 
        { 
            health = 0;
            Debug.Log("player should die");
        }
        else
            health -= damageIncrement;
        Debug.Log("miss... hp is now at: " + Health.health);
        healthBar.fillAmount = health / 100f;
    }
    public static void Regen()
    {
        if (health + regenIncrement > maxHealth)
            health = maxHealth;
        else
            health += regenIncrement;
        Debug.Log("hit! hp is now at: " + Health.health);
        healthBar.fillAmount = health / 100f;    
    }
}
