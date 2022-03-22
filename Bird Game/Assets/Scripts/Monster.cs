using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase] //makes easier to select parent when has children (ex: particle syst)
public class Monster : MonoBehaviour
{
    
    [SerializeField] Sprite deadSprite;
    [SerializeField] ParticleSystem particleSyst;
    
    private bool hasDied;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }
    }

    private bool ShouldDieFromCollision(Collision2D collision) 
    {
        if(hasDied)
        {
            return false;
        }

        Bird bird = collision.gameObject.GetComponent<Bird>(); //looking at collision, gameobj 
                                                               // seeing what we got hit by, if
                                                               // gameobj is a bird, bird != null
                                                               // if not, bird = null
        if (bird != null) //if bird collided with monster
        {
            return true; //should die from collision
        }

        // if falls on top
        if (collision.contacts[0].normal.y < -0.5)
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
