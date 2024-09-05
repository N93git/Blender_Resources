using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSc : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 10.0f;
    private Animator animator;
    public ParticleSystem attJ;
    public ParticleSystem dash;
    private Vector3 moveDirection = Vector3.zero;
    private bool hasAttack = false;
    public float jumpForce = 10f; // Lực dash của nhân vật
    public float rangeAttack = 3;
    public GameObject bullet;
    void Start()
    {
        animator = GetComponent<Animator>();
        attJ.Stop(); // Dừng particle system khi bắt đầu
        //dash.Stop();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (!hasAttack)
        {
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            if (moveDirection != Vector3.zero)
            {

                RotateCharacter();
                animator.SetInteger("State", 1);
            }
            else
            {
                if (!hasAttack)
                    animator.SetInteger("State", 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !hasAttack)
        {
            StartCoroutine(TransitionToDash());
        }

        if (Input.GetMouseButtonDown(0) && !hasAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y; // Giữ nguyên y để chỉ xoay xung quanh trục y

                Vector3 direction = targetPosition - transform.position;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
                var bulletScript = Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z), Quaternion.identity);
                BulletSC bulletSC = bulletScript.GetComponent<BulletSC>();
                bulletSC.isMove = true;
                bulletSC.transform.rotation = Quaternion.LookRotation(direction);
                StartCoroutine(TransitionToAttack());
            }

        }

    }
    IEnumerator TransitionToDash()
    {

        speed = jumpForce;
        hasAttack = true;
        animator.speed = 2f;
        animator.SetInteger("State", 3);
        yield return new WaitForSeconds(0.5f); // Thời gian chờ 0.3 giây
        animator.speed = 1f;                            // animator.speed = 1f;
        hasAttack = false;
        speed = 5.0f;
        animator.SetInteger("State", 0);

    }
    IEnumerator TransitionToAttack()
    {

        // GameObject closestObject = FindClosestObjectToPosition(this.gameObject.transform.position);
        // float distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
        // // Sử dụng closestObject tại đây
        // Vector3 direction = closestObject.transform.position - transform.position;

        // if (direction != Vector3.zero)
        // {
        //     transform.rotation = Quaternion.LookRotation(direction);
        // }
        moveDirection = new Vector3(0, 0, 0);
        hasAttack = true;
        animator.speed = 3f;
        animator.SetInteger("State", 2);

        attJ.Play();
        yield return new WaitForSeconds(0.5f); // Thời gian chờ 0.3 giây
        animator.speed = 1f;
        hasAttack = false;
        animator.SetInteger("State", 0);
        attJ.Stop(); // Dừng particle system khi bắt đầu
        //Kiểm tra nếu khoảng cách < 5 thì mới tìm object gan nhất de xoay
        // if (closestObject != null && distance < rangeAttack)
        // {

        // }
        // else
        // {
        // }
        //


    }
    void FixedUpdate()
    {
        transform.Translate(moveDirection * speed * Time.fixedDeltaTime, Space.World);
    }

    void RotateCharacter()
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    public GameObject FindClosestObjectToPosition(Vector3 position)
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Enemy"); // Thay "YourTag" bằng tag của object bạn muốn xem xét

        foreach (GameObject obj in allObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
