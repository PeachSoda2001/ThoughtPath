using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public GameObject LeftBubble;
    public GameObject RightBubble;
    private Vector3 oriScale;
    private Vector3 oriPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        oriScale = this.transform.localScale;
        oriPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(x!=0 || y!=0)
        {
            this.GetComponent<Animator>().SetBool("IsWalk", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("IsWalk", false);
        }
        float multiplier = this.transform.position.x >= 1 ? GetScalePercentage() : 1;
        if(this.transform.position.x>1f)
        {
            LeftBubble.SetActive(false);
            RightBubble.SetActive(true);
        }
        else if(this.transform.position.x<0f)
        {
            LeftBubble.SetActive(true);
            RightBubble.SetActive(false);
        }
        else
        {
            LeftBubble.SetActive(true);
            RightBubble.SetActive(true);
        }
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(x * multiplier, y* multiplier);
        this.transform.localScale = oriScale * GetScalePercentage();
    }

    public float GetScalePercentage()
    {
        return Mathf.Lerp(0.3f, 1f, 1 - (this.transform.position.y - (-2.5f)) / 4.3f);
    }

    public void HandleResetPlayer()
    {
        this.transform.localScale = oriScale;
        this.transform.position = oriPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LEVEL1_SUCCESS")
        {
            GameManager.Instance.HandlePlayerGetToEndInInsideWorld(true);
        }
        if (collision.tag == "LEVEL1_FAILED")
        {
            GameManager.Instance.HandlePlayerGetToEndInInsideWorld(false);
        }
    }

}
