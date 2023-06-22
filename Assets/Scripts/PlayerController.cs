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

    // Start is called before the first frame update
    void Start()
    {
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveVec * moveSpeed * Time.deltaTime);
        Position.Value = transform.position;    
    }

    public void OnMove(InputValue input)
    {
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
