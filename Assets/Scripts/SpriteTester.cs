using UnityEngine;

public class SpriteTester : MonoBehaviour
{

    public Sprite attackSprite;

    public SpriteRenderer characterSprite;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // when clicky button, play animation
        
    }

    public void changeAnimation() // called as an animation event
    {
        characterSprite.sprite = attackSprite;
    }
}
