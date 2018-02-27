using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        velocity = new Vector3(Random.Range(minVelocity.x, maxVelocity.x), Random.Range(minVelocity.y, maxVelocity.y),0);
	}

    public Vector2 maxVelocity = new Vector2(-0.5f,2f);
    public Vector2 minVelocity = new Vector2(-0.1f, 0.3f);
    public float lifeSpan = 2f;

    Vector3 velocity;
    float timeAlive = 0;



	// Update is called once per frame
	void Update () {

        timeAlive += Time.deltaTime;
        if(timeAlive > lifeSpan)
        {
            Destroy(gameObject);
            
        }
        this.transform.Translate(velocity*Time.deltaTime);
	}
}
