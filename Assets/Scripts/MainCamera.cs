using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    public GameObject ball;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start() {
        offset = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(ball.transform.position + offset);
    }

}