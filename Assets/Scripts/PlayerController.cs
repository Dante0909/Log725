using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    private Vector3 moveVec;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform cam;

    // Keys collected
    private int countCollectedKeys = 0;
    public int nbKeys = 0;
    public TextMeshProUGUI remainingKeysText;
    public GameObject door;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        MovePlayer(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.velocity = moveVec * moveSpeed * Time.fixedDeltaTime;
        Position.Value = transform.position;
    }

    public void MovePlayer(float horizontal, float vertical)
    {
        if (!IsOwner) return;

        var camForward = cam.forward;
        camForward.y = 0;

        print(camForward);

        var camRight = cam.right;
        camRight.y = 0;

        Vector3 relativeMove = vertical * camForward + horizontal * camRight;

        moveVec = relativeMove;

        print(relativeMove);

        var characterRotateNeeded = horizontal != 0;

        if(moveVec != Vector3.zero && characterRotateNeeded){
            Quaternion toRotate = Quaternion.LookRotation(moveVec, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            countCollectedKeys++;


            SetCountText();
        }

        if (other.gameObject.CompareTag("TriggerVictory") && countCollectedKeys == nbKeys)
        {
            SceneManager.LoadScene("EndPlayerWin");
        }
    }

    void SetCountText()
    {
        int remainingKeys = nbKeys - countCollectedKeys;
        remainingKeysText.text = "Remaining keys: " + remainingKeys.ToString();

        if (countCollectedKeys == nbKeys)
        {
            door.SetActive(false);
            remainingKeysText.text = "Find the exit !";
        }
    }
}
