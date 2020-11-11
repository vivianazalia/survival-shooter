using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    float h, v;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");

        anim = GetComponent<Animator>();

        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PowerSpeed();
        }
    }

    private void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");

        v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void PowerSpeed()
    {
        StartCoroutine(ChangeSpeed());
        Move(h, v);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            //mendapatkan jarak antara posisi player dan posisi floorhit
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0;

            //membuat player dapat menghadap sesuai arah input mouse position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0 || v != 0;

        if (walking)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    IEnumerator ChangeSpeed()
    {
        speed = 10;

        yield return new WaitForSeconds(3);

        speed = 6;
    }
}
