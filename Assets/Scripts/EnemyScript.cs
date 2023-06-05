
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float MaxHealth;
    public int Money;

    private Transform canvas;
    private Slider healthBar;
    private float health;

    private void OnEnable()
    {
        canvas = transform.Find("Canvas");
        healthBar = canvas.Find("HealthBar").GetComponent<Slider>();
        canvas.gameObject.SetActive(false);

        health = MaxHealth;
        healthBar.maxValue = MaxHealth;
        healthBar.value = health;
    }

    private void Update()
    {
        canvas.rotation = Quaternion.identity;
        canvas.localScale = Vector3.one * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf) return;

        if (collision.CompareTag("finish"))
        {
            //GameManager.Instance.EnemyEscaped(gameObject);
        }

        else if ((collision.CompareTag("bullet") && !CompareTag("plane")) || (collision.CompareTag("rocket") && !CompareTag("soldier")))
        {
            var flyingShot = collision.gameObject.GetComponent<FlyingShotScript>();
            var damage = flyingShot.Damage;
            health -= damage;
            healthBar.value = health;
            canvas.gameObject.SetActive(true);
            flyingShot.BlowUp();

            if (health <= 0)
            {
                if (CompareTag("plane") || CompareTag("tank"))
                {
                    Pool.Instance.ActivateObject("bigExplosionSoundEffect").SetActive(true);
                    var explosion = Pool.Instance.ActivateObject("explosionParticle");
                    explosion.transform.position = transform.position;
                    explosion.SetActive(true);
                }

                //GameManager.Instance.EnemyKilled(gameObject);
                Pool.Instance.DeactivateObject(gameObject);
                EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "finish")
        {
            EnemyManagerScript.Instance.DeleteEnemy(gameObject);
            Pool.Instance.DeactivateObject(gameObject);
        }
    }
}
