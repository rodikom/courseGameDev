using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy took damage! Current HP: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("Enemy died!");
            Destroy(gameObject);
        }
    }
}