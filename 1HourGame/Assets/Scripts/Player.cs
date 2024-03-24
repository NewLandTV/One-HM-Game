using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Text hpText;

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

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;

    private bool isJump;
    private bool isAttack;
    private bool onDamaged;

    private Color defaultColor;

    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject attackArea;

    [SerializeField]
    private GameManager gameManager;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hp = maxHp;
        defaultColor = spriteRenderer.color;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (transform.position.y < -10f)
            {
                transform.position = Vector3.zero;
            }

            hpText.text = string.Format("HP : {0:n0}", hp);

            if (hp < 0)
            {
                gameManager.GameOver();

                yield break;
            }

            Attack();
            Jump();
            Move();

            yield return null;
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButton(0) && !isAttack)
        {
            attackArea.transform.position += Vector3.right * (spriteRenderer.flipX ? -1.75f : 1.75f);

            StartCoroutine(AttackCoroutine());
        }

        if (attackArea.transform.position.x - transform.position.x > 17.5f)
        {
            attackArea.transform.position = transform.position;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttack = true;

        attackArea.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        attackArea.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        isAttack = false;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;

            rigidBody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (h < 0)
        {
            spriteRenderer.flipX = true;
        }

        Vector3 direction = Vector3.right * h;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isJump = true;
    }

    private IEnumerator OnDamage()
    {
        onDamaged = true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        onDamaged = false;
        spriteRenderer.color = defaultColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 && !onDamaged)
        {
            hp -= collision.GetComponentInParent<Enemy>().Damage;

            StartCoroutine(OnDamage());
        }
    }
}
