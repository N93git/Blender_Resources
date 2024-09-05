using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    public float speed = 15f;
    public bool isMove = false;
    public float timeDestroy = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TransitionToDestroy());
    }
    IEnumerator TransitionToDestroy()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isMove)
            this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
