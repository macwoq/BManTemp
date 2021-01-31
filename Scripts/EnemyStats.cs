using UnityEngine.UI;
using UnityEngine;



public class EnemyStats : MonoBehaviour {

    public Transform healthBar;
    public Slider healthFill;

    public float currentHealth;

    public float maxHealth;

    public float healthBarYOffset = 2;

    void Update()
    {
        PositionHealthBar();
    }

    public void ChangeHealth(int ammount)
    {
        currentHealth += ammount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthFill.value = currentHealth / maxHealth;
    }

    private void PositionHealthBar()
    {
        Vector3 currentPos = transform.position;
        healthBar.position = new Vector3(currentPos.x, currentPos.y + healthBarYOffset, currentPos.z);

        healthBar.LookAt(Camera.main.transform);
    }
}


