using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHp;
    private int hp;
    [SerializeField]
    private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
    }

    private Color defaultColor;

    private bool onDamaged;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Transform target;

    private void Awake()
    {
        hp = maxHp;
        defaultColor = spriteRenderer.color;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (hp < 0)
            {
                Destroy(gameObject, 5f);

                yield break;
            }

            if (target != null)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Random.Range(0.0015f, 0.05f));
            }

            yield return null;
        }
    }

    private IEnumerator OnDamage()
    {
        onDamaged = true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        onDamaged = false;
        spriteRenderer.color = defaultColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 10 && !onDamaged)
        {
            hp -= collision.collider.GetComponentInParent<Player>().Damage;

            StartCoroutine(OnDamage());
        }
    }
}
