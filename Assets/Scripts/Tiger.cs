using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour {

    public float speed;
    public Vector3 starting_pos;
    public float[] direction;
    // Start is called before the first frame update
    void Start() {

        starting_pos = transform.position;
        direction = new float[2];
        direction[0] = 0;
        direction[1] = 0;

    }

    // Update is called once per frame
    void FixedUpdate() {

        var rotation = transform.rotation;

        var angle = rotation.eulerAngles.y % 360;

        if (angle < 89) {
            direction[0] = 0;
            direction[1] = -speed;
        } else if (angle < 89 + 90) {
            direction[0] = -speed;
            direction[1] = 0;

        } else if (angle < 89 + 90 + 90) {
            direction[0] = 0;
            direction[1] = speed;
        } else {
            direction[0] = speed;
            direction[1] = 0;
        }
        // val - speed

        Vector3 current_pos = transform.position;
        current_pos = new Vector3(current_pos.x - direction[0], current_pos.y, current_pos.z - direction[1]);
        transform.position = current_pos;

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.StartsWith("Edge")) {
            // hit edge, rotate and keep running
            Vector3 current_pos = transform.position;
            current_pos = new Vector3(current_pos.x + 6 * direction[0], current_pos.y, current_pos.z + 6 * direction[1]);
            transform.position = current_pos;
            transform.Rotate(0, -90, 0);

        } else if (collision.gameObject.name.StartsWith("Player")) {
            this.enabled = false;
        }

    }

}