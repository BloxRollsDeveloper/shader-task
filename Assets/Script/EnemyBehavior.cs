using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float movementDirection = 1;
    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_rb.linearVelocity.x) < 0.1f)
        {
            //Flip();
        }
        _rb.linearVelocity = new Vector2(movementSpeed * movementDirection, _rb.linearVelocity.y);
        
        // Flip sprite
        
        switch (_rb.linearVelocity.x)
        {
            case < -0.1f:
                _renderer.flipX = false;
                break;
            case > 0.1f:
                _renderer.flipX = true;
                break;
        }

    }
    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scenes/Level1");
            return;
        }
        Flip();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().OnJump();
        }
    }
    
    private void Flip()
    {
        movementDirection *= -1;
    }
}
