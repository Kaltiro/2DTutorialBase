using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region physics components
    Rigidbody2D PlayerRB;

    #endregion

    #region attack variables
    public float damage;
    public float attackspeed;
    float attacktimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion 

    #region animation components
    Animator anim;
    #endregion

    #region health variables
    public float maxHealth;
    float currHealth;
    public Slider hpSlider;
    #endregion

    #region unity functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        attacktimer = 0;
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        hpSlider.value = currHealth / maxHealth;
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (attacktimer <= 0)
            {
                Attack();
            } 

        }
        else
        {
            attacktimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Interact();
        }
    }
    #endregion

    #region movement functions
    private void Move()
    {
        anim.SetBool("Moving", true);
        if (x_input > 0)
        {
            PlayerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        } else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        } else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        } else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }

        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);

    }
    #endregion

    #region Attack Functions
    private void Attack()
    {
        Debug.Log("attacking now");
        Debug.Log(currDirection);
        attacktimer = attackspeed;
        //handles all attack animations and handles hitboxes
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attacktrig");

        FindObjectOfType<AudioManager>().Play("PlayerAttack");
        PlayerRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(hitboxtiming);
        //Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(endanimationtiming);
        isAttacking = false;

        yield return null;
    }
    #endregion

    #region Health functions
    public void TakeDamage(float value)
    {

        FindObjectOfType<AudioManager>().Play("PlayerHurt");
        currHealth -= value;
        hpSlider.value = currHealth / maxHealth; 

        if (currHealth <= 0)
        {
            //Die
            Die();

        }
    }
    #endregion

    public void Heal(float value)
    {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        hpSlider.value = currHealth / maxHealth;

    }

    private void Die()
    {

        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }

    #region Interact Functions
    private void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2(.5f, .5f), 0f, Vector2.zero, 0f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }
    #endregion
}
