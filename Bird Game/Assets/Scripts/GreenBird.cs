using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBird : MonoBehaviour
{
    //neither of these 2 work properly on green bird?
    [SerializeField] Sprite deadSprite;
    [SerializeField] ParticleSystem particleSyst;

    private Rigidbody2D rigidBody2D;
    private bool hasDied;
    public object FreezeRotation { get; private set; } //needed to freeze and unfreeze rotation

    //copied from monster class

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>(); //saves off ref to rigid body to dont have to call getcomp everytime
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D.isKinematic = true; //isKinematic means only moved and controlled by our codef
        // rigidBody2D.constraints = true; //supposed to set freeze position to false
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidBody2D.isKinematic = false;

        //fetches rigidbody from Gameobj with this script attached
        rigidBody2D = GetComponent<Rigidbody2D>();

        //
        rigidBody2D.freezeRotation = false;


        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool ShouldDieFromCollision(Collision2D collision)
    {
        if (hasDied)
        {
            return false;
        }

        Bird bird = collision.gameObject.GetComponent<Bird>(); //looking at collision, gameobj 
                                                               // seeing what we got hit by, if
                                                               // gameobj is a bird, bird != null
                                                               // if not, bird = null
        if (bird != null) //if bird collided with monster
        {
            rigidBody2D.velocity = collision.relativeVelocity/2; //move green bird at slower speed than red bird hit with
            return true; //should die from collision
        }
        // if falls on top
        if (collision.contacts[0].normal.y < -0.5 && collision.contacts[0].normal.y > 0.5)
        {                                         //contacts = array of points contact happened
                                                  // normal = vector collision came in at
                                                  // -0.5 = 45 degree angle tween y-axis
                                                  // x-axis, y is 0. y-axis, y is -1(above)
                                                  // below would be y = 1
            return true;
        }
        return false; //otherwise shouldnt die from collision
    }

    private IEnumerator Die() //any waiting required a method to be called with 'StartCoroutine(method)'
    {
        hasDied = true;

        GetComponent<SpriteRenderer>().sprite = deadSprite;

        particleSyst.Play();

        yield return new WaitForSeconds(2);

        gameObject.SetActive(false); //set monster gameobj to not active (dissapears)
    }
}
