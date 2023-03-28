using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChar : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5;
    public bool mvHoz;
    public bool mvVer;
    public Sprite Up;
    public Sprite Right;
    public Sprite Down;
    public Sprite Left;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float xPos = 0, yPos = 0;
        if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") != 0){
            if (mvHoz){
                yPos = Input.GetAxis("Vertical");
            }else if  (mvVer){
                xPos = Input.GetAxis("Horizontal");
            }
        }else {
            mvHoz = Input.GetAxis("Horizontal") != 0;
            xPos = Input.GetAxis("Horizontal");
            mvVer = Input.GetAxis("Vertical") != 0;
            yPos = Input.GetAxis("Vertical");
        }
        if (xPos>0) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Right;
        }
        if (xPos<0) { 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Left; 
        }
        if (yPos>0) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Up; 
        }
        if (yPos<0) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Down; 
        } 
 
        transform.position += new Vector3(xPos, yPos, 0).normalized * speed * Time.deltaTime;
    }
}

       /*float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;
        rb.velocity = new Vector2(h,rb.velocity.x);
        rb.velocity = new Vector2(v,rb.velocity.y);*/