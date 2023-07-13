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
    private GameObject camera;

    // LightGems
    public GameObject[] lightGems;
    public float lightGemCooldown = 3.0f;
    private float lightGemCooldownTimer = 0.0f;

    // Keys collected
    private int countCollectedKeys = 0;
    private int nbKeys = 0;
    public TextMeshProUGUI remainingKeysText;
    private Rigidbody rb;

    // Sounds
    public AudioClip keySound;
    public AudioClip doorSound;
    public AudioClip lightGemSound;

    //Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nbKeys = GridManager.Singleton.NumberOfKeys;
        animator = GetComponent<Animator>();
        if(IsHost)
            camera.SetActive(true);
        else
            camera.SetActive(false);
        remainingKeysText = GameObject.FindGameObjectWithTag("KeyUI").GetComponent<TextMeshProUGUI>();
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lightGemCooldownTimer)
        {
            lightGemCooldownTimer = Time.time + lightGemCooldown;
            OnCreateLightGem();
        }
        
        print("PLAYER: " + transform.position);
        camera.transform.position = transform.position + new Vector3(0, 7f, -3.5f);

        if(moveVec != Vector3.zero){
            animator.SetFloat("speed", 2);
            Quaternion toRotate = Quaternion.LookRotation(moveVec, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.fixedDeltaTime);
        } else {
            animator.SetFloat("speed", 0);
        }
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
    }

    void SetCountText()
    {
        int remainingKeys = nbKeys - countCollectedKeys;
        AudioSource.PlayClipAtPoint(keySound, transform.position);
        remainingKeysText.text = "Remaining keys: " + remainingKeys.ToString();

        if (countCollectedKeys == nbKeys)
        {
            GridManager.Singleton.TriggerVictory.SetActive(true);
            AudioSource.PlayClipAtPoint(doorSound, transform.position);
            remainingKeysText.text = "Find the exit !";
        }
    }

    public void OnCreateLightGem()
    {
        // Create a new light gem at the player's position on ground
        Vector3 playerPos = transform.position;
        Vector3 lightGemPos = new Vector3(playerPos.x, 0.0f, playerPos.z);

        // Randomly choose a light gem
        int randomIndex = Random.Range(0, lightGems.Length);
        GameObject lightGem = lightGems[randomIndex];

        
        GameObject gem = Instantiate(lightGem, lightGemPos, Quaternion.identity);
        NetworkObject gemNetworkObject = gem.GetComponent<NetworkObject>();
        gem.SetActive(true);
        gemNetworkObject.Spawn();

        AudioSource.PlayClipAtPoint(lightGemSound, transform.position);
    }
}
