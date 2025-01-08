using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float jumpForce;

    //value added to the roation of the player to find it's rotation when going up or down
    [SerializeField] private int rotAngle;
    //value by wich the rotation change
    [SerializeField] private int rotSpeed;

    [SerializeField] byte pipeLayer;
    private int pipeId;

    #region unity functions
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        #region for my dumb ass
        if (rotSpeed == 0)
            Debug.LogError("rotation speed is  0");
        if (rotAngle == 0)
            Debug.LogError("rotation angle is  0");
        #endregion
    }

    private void Update()
    {
        Rotate();
        UpdateScore();
    }

    private void OnEnable()
    {
        InputManager.JumpEvent += OnJump;
    }

    private void OnDisable()
    {
        InputManager.JumpEvent -= OnJump;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.ChangeGameState(GameState.GameOver);
        Destroy(gameObject);
    }
    #endregion

    private void UpdateScore()
    {
        RaycastHit2D[] collisionRaycast = new RaycastHit2D[1];

        LayerMask LayerMask = 1 << pipeLayer;

        int PipeInCollider = Physics2D.RaycastNonAlloc(transform.position, Vector2.down * 2, collisionRaycast, 5, LayerMask);
        Debug.DrawRay(transform.position, Vector2.down * 5, Color.red);

        if (collisionRaycast[0].collider == null)
            return;

        int colliderId = collisionRaycast[0].collider.gameObject.GetInstanceID();

        if (PipeInCollider > 0 && pipeId != colliderId)
        {
            ScoreManager.Instance.ScoreChanger();
            pipeId = colliderId;
        }
    }

    private void OnJump()
    {
        //rb.linearVelocity = Vector2.up * jumpForce;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Rotate()
    {
        Quaternion StartRot = transform.rotation;

        Vector3 rotation = Vector3.zero;

        if (rb.linearVelocityY > 0)
        {
            rotation.z = rotAngle;
            transform.rotation = Quaternion.RotateTowards(StartRot, Quaternion.Euler(rotation), rotSpeed);
        }
        else
        {
            rotation.z = -rotAngle;
            transform.rotation = Quaternion.RotateTowards(StartRot, Quaternion.Euler(rotation), rotSpeed);
        }
    }
}
