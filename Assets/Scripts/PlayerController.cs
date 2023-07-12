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

    // Keys collected
    private int countCollectedKeys = 0;
    public int nbKeys = 0;
    public TextMeshProUGUI remainingKeysText;
    public GameObject door;
    private Rigidbody rb;
    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = this.transform.GetChild(0).gameObject;
        if(IsHost)
            camera.SetActive(true);
        remainingKeysText = GameObject.FindGameObjectWithTag("KeyUI").GetComponent<TextMeshProUGUI>();
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.velocity = moveVec * moveSpeed * Time.fixedDeltaTime;
        Position.Value = transform.position;
    }

    public void OnMovePlayer(InputValue input)
    {
        if (!IsOwner) return;
        Vector2 inputVec = input.Get<Vector2>();

        moveVec = new Vector3(inputVec.x, 0, inputVec.y);
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
