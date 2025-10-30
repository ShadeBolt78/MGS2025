using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    //static bool hurt = false;
    //public bool hurtcopy = hurt;

    [Header("Identifiers")] // "What player is this?"
    public bool isPlayer1 = false;
    public bool isDuo = false;

    [Header("Sprites")]
    public Sprite sprBase;
    public Sprite sprAttack1;
    public Sprite sprAttack2;
    public Sprite sprAttack3;
    public Sprite sprOuch;

    //Off-screen Location Vector 
    Vector3 offscreen = new Vector3(-10f, 0.5f, 1.1f);
    public Vector3 baseGeneral;

    [SerializeField]
    private int resetcounter = 0;

    int resetmax = 500; //not changed in code, but variable if want to change it
    int attackcount = 0;

    private void Start()
    {
        //do newPos when press
        InputManager.Instance.OnLanePressed += newPos;

        Vector3 baseP1 = GameObject.Find("Lane1").transform.position;
        Vector3 baseP2 = GameObject.Find("Lane3").transform.position;
        Vector3 basePDuo = GameObject.Find("Lane2").transform.position;

        //Set up general position
        if (isPlayer1 && !isDuo) //P1
        {                               //-5.65, 0.5, 2.6
            baseGeneral = new Vector3(baseP1.x - 5.65f, baseP1.y + 0.5f, baseP1.z + 0.6f);
            transform.position = baseGeneral;
        }
        else if (!isPlayer1 && !isDuo) // P2
        {
            baseGeneral = new Vector3(baseP2.x - 5.65f, baseP2.y + 0.5f, baseP2.z + 0.6f);
            transform.position = baseGeneral;
        }
        else if (isDuo) //hide duo as default
        {
            baseGeneral = new Vector3(basePDuo.x - 5.65f, basePDuo.y + 0.5f, basePDuo.z + 0.6f);
            transform.position = offscreen; // hide Duo as default
        }

    }//END OF START()

    //run every frame
    private void Update()
    {

        if (resetcounter <= resetmax) // just to not skyrocket the value when afk
            resetcounter += 1;

       /* hurtcopy = hurt;

        if (hurt)
        {
            Debug.Log("test");
            resetcounter = 0;
            hurt = false;
            hurtcopy = hurt;

            if(hurtcopy)
                Debug.Log("real");

            gameObject.GetComponent<SpriteRenderer>().sprite = sprOuch;

        }*/

        //reset position of character after attack
        if (resetcounter >= resetmax)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprBase;
            attackcount = 0;

            if (isDuo) //PDuo
                transform.position = offscreen;
            else
                transform.position = baseGeneral;

        }


    }//END OF UPDATE()

    //newPos: Update where character is
    public void newPos(int lane)
    {
        resetcounter = 0;
        //hurt = false;
        //hurtcopy = hurt;

        //Sprite Changer
        // Only operate on single character: 1 2 D
        if ( (isPlayer1 == (lane <= 2)) || isDuo)  //is P1 & <2   or   P2 & >2   or   Duo
        { 
            if (attackcount == 0)
                gameObject.GetComponent<SpriteRenderer>().sprite = sprAttack1;
            else if (attackcount == 1)
                gameObject.GetComponent<SpriteRenderer>().sprite = sprAttack2;
            else if (attackcount == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprAttack3;
                attackcount = -1; //cus of the +1 below
            }

            attackcount += 1;
        }   

        //Lane Placement Changer
        if (lane == 2)
        {
            if (isDuo)//show duo, hide solo
                transform.position = baseGeneral;
            else
                transform.position = offscreen;
        }
        else if (lane != 2)
        {

            if (isDuo) //hide duo, show solo
                transform.position = offscreen;

            else if ((isPlayer1 && lane <= 2) || (!isPlayer1 && lane >= 2))
                transform.position = new Vector3(GameObject.Find("Lane"+lane).transform.position.x - 5.65f, 
                                                 GameObject.Find("Lane" + lane).transform.position.y + 0.5f,
                                                 GameObject.Find("Lane" + lane).transform.position.z + 0.6f);

        }
    }// END OF NEWPOS()

    public static void missed()
    {
        //hurt = true;
    }

}
