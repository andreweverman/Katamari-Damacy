using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private float f_angle;
    float x;
    float z;
    Vector2 unit_vector;
    public float speed;
    public float mass;

    public GameObject camera_ref;
    float camera_offset = 12;

    public static float pickupableRatio = 0.3f;
    private Rigidbody rigidbody;

    public float radius;
    private float stuck_count;

    private int largest;

    public Text mass_text;
    public Text time_text;
    public Text game_over;
    public RectTransform panel;

    private int current_thousand = 0;
    private int stuck_resets = 0;

    public float time_limit;

    // Start is called before the first frame update
    void Start() {

        rigidbody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate() {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * -100;
        z = Input.GetAxis("Vertical") * Time.deltaTime * 500;

        f_angle += x;
        unit_vector = new Vector2(Mathf.Cos(f_angle * Mathf.Deg2Rad), Mathf.Sin(f_angle * Mathf.Deg2Rad));

        // move the ball
        // was speed = 60 at start
        this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(unit_vector.x, 0, unit_vector.y) * z * speed);

        // move the camera
        camera_ref.transform.position = new Vector3(-unit_vector.x * camera_offset, camera_offset, -unit_vector.y * camera_offset) + this.transform.position;

        UpdateUI();

    }

    void UpdateUI() {

        mass_text.text = "Mass = " + Mathf.FloorToInt(rigidbody.mass);
        time_text.text = "Time Remaining:  " + Mathf.FloorToInt(time_limit - Time.time) + " seconds";

        if (time_limit - Time.time == 0) {
            EndGame();
        }

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name != "Floor") {
            if ((collision.gameObject.tag.StartsWith("Sticky")) &&
                collision.rigidbody.mass < rigidbody.mass * pickupableRatio) {

                Stick(collision);
                if (collision.transform.name.StartsWith("Barrier")) {
                    stuck_count = 0;
                    stuck_resets++;
                } else if (current_thousand != Mathf.FloorToInt(mass / 1000)) {
                    stuck_count = 0;
                    stuck_resets++;
                    current_thousand = Mathf.FloorToInt(mass / 1000);
                }
            }
        }

    }

    void Stick(Collision collision) {
        // keeps track of stuck objects
        stuck_count++;

        SphereCollider sphere = transform.GetComponent<SphereCollider>();
        // make sphere collision radius bigger the larger our ball gets, but scale it down over time so that radius doesn't get too big
        sphere.radius += 0.02f * (1 * (stuck_resets * .2f + 1) + .5f * Mathf.Log10(1f / stuck_count));
        radius = sphere.radius;
        rigidbody.mass += collision.rigidbody.mass;

        speed = rigidbody.mass * 4 / 5;

        collision.rigidbody.constraints = RigidbodyConstraints.None;

        Destroy(collision.rigidbody);
        Destroy(collision.collider);
        Destroy(collision.gameObject.GetComponent<CharacterController>());

        collision.transform.parent = transform;

    }

    void EndGame() {
        Time.timeScale = 0;
        game_over.text = "Time's Up!\nFinal Mass = " + rigidbody.mass;
        panel.GetComponent<Image>().enabled = true;

    }
}