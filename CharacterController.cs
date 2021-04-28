using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator anim;

    public Transform attackPoint;

    public float attackRange = 0.5f;

    public LayerMask enemyLayers;

    private Vector2 velocity;

    private float dashTimeLeft;
    private float attackTimeLeft;
    private float lastAttack = -100f;
    private float lastImageXpos;
    private float lastDash = -100f;

    private int comboNum = 0;

    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool isDashing = false;
    private bool canMove = true;
    private bool canFlip = true;
    private bool canDash = true;
    private bool canAttack = true;

    private Vector3 characterScale;


    public float movementSpeed = 10;
    public float jumpForce = 1;
    public float facingDirection = -1;
    // dash
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;
    // attack
    public float attackTime;
    public float isolTime;
    public float attackSpeed;
    public float attackCoolDown;
    public float timeBetweenCombo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        checkInput();
        checkDash();
        checkMove();
        checkFlip();
    }

    public void checkInput()
    {

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown)) AttempToDash();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void checkMove()
    {
        if (canMove)
        {
            var movement = Input.GetAxis("Horizontal");
            if (movement != 0) anim.SetBool("walking", true);
            else anim.SetBool("walking", false);
            characterScale = transform.localScale;


            rb.position += new Vector2(movement * movementSpeed * Time.deltaTime, 0);

            if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

        }
    }

    private void checkFlip()
    {
        if (canFlip)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                characterScale.x = -5f;
                facingDirection = -1;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                characterScale.x = 5f;
                facingDirection = 1;
            }
            transform.localScale = characterScale;
        }
    }

    private void checkDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                canAttack = false;
                rb.velocity = new Vector2(dashSpeed * movementSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0)
            {
                rb.velocity = Vector2.zero;
                isDashing = false;
                canMove = true;
                canFlip = true;
                canAttack = true;
                anim.SetBool("dashing", false);
            }
        }

    }


    void Attack()
    {
        if (canAttack && !isAttacking)
        {
            anim.SetTrigger("attack");
            isAttacking = true;
            attackTimeLeft = attackTime;
            // Play attack animation according to combo
            
            Debug.Log(Time.realtimeSinceStartup - lastAttack);


            // Detect enemies (TBD)

            /* Collider2D[] = hitEnemies = Physics2D.OverlapCirclaAll(attackPoint.position, attackRange, enemyLayers);
             * 
             * 
             */

            // Damage (TBD)

            /* foreach(Collider 2D enemy in hitEnemies) {
             *  Debug.Log("Hit!");
             * 
             * }
             * 
             * 
             */
        }
    }

    private void AttempToDash()
    {
        if (canDash)
        {
            anim.SetBool("dashing", true);
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
        }
    }

    public void animEvent(string message)
    {
        if (message.Equals("2"))
        {
            canMove = false;
            canFlip = false;
            canDash = false;
            rb.velocity = new Vector2(attackSpeed * movementSpeed * facingDirection, rb.velocity.y);
        }
        if (message.Equals("3"))
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (message.Equals("1"))
        {
            canMove = false;
            canFlip = false;
            canDash = false;
            rb.velocity = new Vector2(0, 0);
        }
        if (message.Equals("4"))
        {
            canMove = true;
            canFlip = true;
            canDash = true;
            isAttacking = false;
            lastAttack = Time.realtimeSinceStartup;
        }
    }
}
