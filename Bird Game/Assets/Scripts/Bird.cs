using System;
using System.Collections;
using System.Collections.Generic; // greyed out bc not being used rn 
using UnityEngine; // need unity to run

public class Bird : MonoBehaviour
{
    [SerializeField] float launchForce = 500; //serialized vars used by other developers+to show customer stuff
                                              // shows up in unity to change during runtime or whenever
    [SerializeField] float maxDragDistance = 5;

    private Vector2 startPosition; // field def'd (x,y) for start
    Rigidbody2D rigidBody2D;
    SpriteRenderer spriteRend;

    private void Awake(){ //awake is executed first for all our objs, so usually used for caching vars
                         // found thru "unity execution order"
    
        rigidBody2D = GetComponent<Rigidbody2D>(); //saves off ref to rigid body to dont have to call getcomp everytime
                                                   // called 'caching', speeds up things w/ use getcomp alot
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() //called third 
    {
        // vars in classes called 'fields'
        startPosition = rigidBody2D.position; //sets var to position of bird
        rigidBody2D.isKinematic = true; //anything tween '< >' is the type, isKinematic means only
                                        //moved and controlled by our code
        rigidBody2D.freezeRotation = true; //stops bird from rotating, but doesn't reset direction to point in
    }

    void OnMouseDown() //can specify method as 'private' but private by default as well
    {
        spriteRend.color = Color.red; // or could: 'new Color(1, 0, 1);'
                                                          // args = lvls of primary colors
    }

    void OnMouseUp() // when not pressing m1 on bird
    {
        var currentPosition = rigidBody2D.position; // could have type as 'var' or 'Vector2'
                                                                    //  'var' lets editor assign type
        Vector2 direction = startPosition - currentPosition; //2d vector subtraction
        direction.Normalize(); //changes our existing vector (how?) (abs val?) (usually normalize vects)


       rigidBody2D.isKinematic = false; //need for adding force below
       
       rigidBody2D.AddForce(direction * launchForce); //amt of force adjustable

        spriteRend.color = Color.white;

        // unfreeze rotation when release bird to fly
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.freezeRotation = false;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // vector3 = can be used for
                                                                                     //  x,y, or z coord.
                                                                                     // var is camelcase, usual for vars
                                                                                     //  in C# 
                                                                                     // gives us mouse's position wrt cam

        Vector2 desiredPosition = mousePosition;
        double angle; // angle bird pointing
        double x, y; //Ph's to find angle
        Vector2 direction = desiredPosition - startPosition;

        rigidBody2D.freezeRotation = false; //allows to rot


        float distance = Vector2.Distance(desiredPosition, startPosition); //gives us distance in meters tween 2 positions
        if(distance > maxDragDistance)
        {
            direction = desiredPosition - startPosition;
            direction.Normalize(); //turns direction into vector from magnitude, needed?
            desiredPosition = startPosition + (direction * maxDragDistance);
        }

        // don't think this is necessary but rotation of red bird doesn't work without it
        
        if (desiredPosition.x > startPosition.x)
        {
            desiredPosition.x = startPosition.x;   //doesn't allow to drag bird to the right
        }
        

        //old code: transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z); // transforms bird to
        //  mouse position
        // x,y wrt cam, z wrt
        //  us? (ignore z)

        //find and set orientation when dragging
        x = -1*(startPosition.x - desiredPosition.x); //x always negative bc of start position, so mult by -1
        y = desiredPosition.y - startPosition.y;
        if (x != 0)
        {
            angle = Math.Atan(y/x); //Atan specifies inverse of tan, also only works with rads as arg
            angle = angle * 180 / Math.PI;
            rigidBody2D.MoveRotation((float)angle); //this doesn't work?

        }

        rigidBody2D.position = desiredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) //called everytime bird collides w/ something
    {                                                      // param has info ab collision: what hit, angle hit, how hard hit
        StartCoroutine(ResetAfterDelay()); //'ResetAfterDelay' = method so use pascal case (camelcase w/ starting uppercase)
                                           //  Corourtines used to 'wait' a time 
    }

    private IEnumerator ResetAfterDelay() //waits for 3 secs
    {
        yield return new WaitForSeconds(3);
        rigidBody2D.position = startPosition; //reset bird's position to start
        
        rigidBody2D.SetRotation(0);
        rigidBody2D.freezeRotation = true; //necessarry to stop rotate
       
        rigidBody2D.isKinematic = true; //so gravity won't affect him
      
        rigidBody2D.velocity = Vector2.zero; //stops bird from moving
    }
}
