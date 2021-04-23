using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class RubyController : MonoBehaviour
{

    Animator animator; 

    public GameObject projectilePrefab;

    Vector2 lookDirection = new Vector2(1, 0);

    public float speed = 6.0f;

    public int maxHealth = 5;
    public float timeInvencible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvencible;
    float invencibleTimer;

    //Creating variable called rigidbody2d, because you need to move the rigidbody and not the GameObject to avoid jittering
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public ParticleSystem hitEffect;

    AudioSource audioSource;

    public AudioClip throwCogClip;
    public AudioClip hitClip;

    // Start is called before the first frame update
    void Start()
    {
        //assiging to the variable the component info
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ///Changing FPSto 10 and removing VsyncCount
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //Creating movement variables receiving Horizontal and Vertical inputs. Input.GetAxis receives the values
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x,0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (isInvencible)
        {
            invencibleTimer -= Time.deltaTime;
            if (invencibleTimer <= 0)
            {
                isInvencible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider !=null)
            {
               
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                if (character != null)
                {
                    character.DisplayDialog();
                }
                
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }

    void FixedUpdate() 
    {
        //Creating variable position receving the actual of the rigidbody
        Vector2 position = rigidbody2d.position; 
        //changing the x and y values based on the input
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        //changing the rigidbody position
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvencible)
                return;
            isInvencible = true;
            invencibleTimer = timeInvencible;
            animator.SetTrigger("Hit");
            PlaySound(hitClip);
            Instantiate(hitEffect, rigidbody2d.transform);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);   
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwCogClip);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
