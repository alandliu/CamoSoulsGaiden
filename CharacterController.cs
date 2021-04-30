using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPoint;

    public int ID;
    public int tempID;
    public string[] clipNames = new string[] { "cSlash", "Punch", "kSlash", "Fire" };
    public string[] deathClips = new string[] { "Boom1", "Boom2" };
    public string[] start = new string[] { "kS", "rS", "aS" };
    public string[] jump = new string[] { "kJ", "rJ", "aJ" };
    public string[] dash = new string[] { "kDash", "rDash", "aDash" };
    public string[] death = new string[] { "kDeath", "rDeath", "aDeath" };
    public string[] hurt = new string[] { "kH1", "kH2", "rH1", "rH2", "aH1", "aH2" };
    public string[] attack = new string[] { "kA1", "kA2", "kA3", "rA1", "rA2", "rA3", "aA1", "aA2", "aA3" };

    public HealthBar hb;

    public GameObject opponent;


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
    public int maxLife = 3;
    public int curLife;

    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool isDashing = false;
    private bool canMove = true;
    private bool canFlip = true;
    private bool canDash = true;
    private bool canAttack = true;
    private bool canDJump = true;

    public bool canKnock = true;

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

    public int mult = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curLife = maxLife;
        hb.SetMaxHealth(maxLife + 1);
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

        if (Input.GetKeyDown(KeyCode.R))
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

            if (Mathf.Abs(rb.velocity.y) < 0.001f) canDJump = true;

            if (!isAttacking)
            {

                if (Input.GetButtonDown("Jump") && (Mathf.Abs(rb.velocity.y) < 0.001f || canDJump))
                {
                    if (Mathf.Abs(rb.velocity.y) > 0.001f)
                    {
                        canDJump = false;
                        rb.velocity = Vector2.zero;
                    }
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    anim.SetBool("jumping", true);
                    anim.SetBool("falling", false);
                    anim.SetBool("walking", false);

                    if (ID > 2)
                    {
                        tempID = ID - 1;
                        FindObjectOfType<AudioManager>().Play(jump[tempID]);
                    }
                    else if (ID < 2)
                    {
                        tempID = ID;
                        FindObjectOfType<AudioManager>().Play(jump[tempID]);
                    }
                }

                if (rb.velocity.y < -0.5f)
                {
                    anim.SetBool("falling", true);
                    anim.SetBool("jumping", false);
                    anim.SetBool("walking", false);
                }
                
            }

            if (Mathf.Abs(rb.velocity.y) < 0.001f)
            {
                anim.SetBool("falling", false);
                anim.SetBool("jumping", false);
            }

        }
    }

    private void checkFlip()
    {
        if (canFlip)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                characterScale.x = -4f;
                facingDirection = -1;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                characterScale.x = 4f;
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
                canKnock = false;
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
                canKnock = true;
                rb.velocity = Vector2.zero;
                isDashing = false;
                canMove = true;
                canFlip = true;
                canAttack = true;
                anim.SetBool("dashing", false);
                Physics2D.IgnoreLayerCollision(9, 10, false);
                Physics2D.IgnoreCollision(opponent.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            }
        }

    }


    void Attack()
    {
        if (canAttack && !isAttacking && (Time.realtimeSinceStartup - lastAttack >= attackCoolDown))
        {
            canKnock = false;
            anim.SetTrigger("attack");
            isAttacking = true;
            attackTimeLeft = attackTime;
        }
    }

    private void AttempToDash()
    {
        if (canDash)
        {
            Physics2D.IgnoreCollision(opponent.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            anim.SetBool("dashing", true);
            isDashing = true;
            canKnock = false;
            dashTimeLeft = dashTime;
            lastDash = Time.time;
            Physics2D.IgnoreLayerCollision(9, 10, true);

            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
            if (ID > 2)
            {
                tempID = ID - 1;
                FindObjectOfType<AudioManager>().Play(dash[tempID]);
            }
            else if (ID < 2)
            {
                tempID = ID;
                FindObjectOfType<AudioManager>().Play(dash[tempID]);
            }
        }
    }

    public void animEvent(string message)
    {
        if (message.Equals("2"))
        {
            canKnock = false;
            canMove = false;
            canFlip = false;
            canDash = false;
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(attackSpeed * movementSpeed * facingDirection, rb.velocity.y);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.gameObject.transform.GetChild(0).position, attackRange, enemyLayers);


            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<CharacterControllerP2>().canKnock)
                {
                    opponent.GetComponent<CharacterControllerP2>().knockBack(facingDirection);
                }
            }

            FindObjectOfType<AudioManager>().Play(clipNames[ID]);

            if (ID > 2)
            {
                tempID = ID - 1;
                FindObjectOfType<AudioManager>().Play(hurt[3 * tempID + Random.Range(0, 3)]);
            }
            else if (ID < 2)
            {
                tempID = ID;
                FindObjectOfType<AudioManager>().Play(hurt[3 * tempID + Random.Range(0, 3)]);
            }


        }
        if (message.Equals("3"))
        {
            rb.velocity = new Vector2(0, 0);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.gameObject.transform.GetChild(0).position, attackRange, enemyLayers);


            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<CharacterControllerP2>().canKnock)
                {
                    opponent.GetComponent<CharacterControllerP2>().knockBack(facingDirection);
                }
            }
        }
        if (message.Equals("1"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canKnock = false;
            canMove = false;
            canFlip = false;
            canDash = false;
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(0, 0);
        }
        if (message.Equals("4"))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canKnock = true;
            canMove = true;
            canFlip = true;
            canDash = true;
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
            isAttacking = false;
            lastAttack = Time.realtimeSinceStartup;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death" && curLife > 0)
        {
            Debug.Log("Died");
            transform.position = spawnPoint.transform.position;
            rb.velocity = Vector2.zero;
            mult = 1;
            curLife -= 1;
            hb.SetHealth(curLife + 1);
            FindObjectOfType<AudioManager>().Play(deathClips[Random.Range(0, 2)]);

            if (ID > 2)
            {
                tempID = ID - 1;
                FindObjectOfType<AudioManager>().Play(death[tempID]);
            }
            else if (ID < 2)
            {
                tempID = ID;
                FindObjectOfType<AudioManager>().Play(death[tempID]);
            }

        } else if (curLife <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(this.gameObject.transform.GetChild(0).position, attackRange);
    }

    public void knockBack(float oFacingDirection)
    {
        rb.AddForce(new Vector2(oFacingDirection * 5 * mult, 5.0f * mult), ForceMode2D.Impulse);
        mult++;
        anim.SetTrigger("knocked");
        anim.SetBool("jumping", false);
        anim.SetBool("falling", false);
        if (ID > 2)
        {
            tempID = ID - 1;
            FindObjectOfType<AudioManager>().Play(hurt[2 * tempID + Random.Range(0, 2)]);
        }
        else if (ID < 2)
        {
            tempID = ID;
            FindObjectOfType<AudioManager>().Play(hurt[2 * tempID + Random.Range(0, 2)]);
        }
    }
}
