using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
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
    private NetworkVariable<int> countCollectedKeys = new NetworkVariable<int>();
    private int nbKeys = 0;
    public TextMeshProUGUI remainingKeysTextMeshPro;
    public NetworkVariable<FixedString64Bytes> remainingKeysText = new NetworkVariable<FixedString64Bytes>();
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
        remainingKeysText.OnValueChanged += OnRemainingKeysTextChanged;
        rb = GetComponent<Rigidbody>();
        nbKeys = GridManager.Singleton.NumberOfKeys;
        animator = GetComponentInChildren<Animator>();
        if(IsHost)
            camera.SetActive(true);
        else
            camera.SetActive(false);
        remainingKeysTextMeshPro = GameObject.FindGameObjectWithTag("KeyUI").GetComponent<TextMeshProUGUI>();
        if (IsHost)
        {
            SetCountText();
        }
        else
        {
            remainingKeysTextMeshPro.text = remainingKeysText.Value.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lightGemCooldownTimer)
        {
            lightGemCooldownTimer = Time.time + lightGemCooldown;
            OnCreateLightGem();
        }

        camera.transform.position = transform.position + new Vector3(0, 7f, -3.5f);

        if(moveVec != Vector3.zero){
            HandleAnimationClientRpc(1.0f);
            Quaternion toRotate = Quaternion.LookRotation(moveVec, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        } else {
            HandleAnimationClientRpc(0f);
        }
    }

    
    [ClientRpc]
    private void HandleAnimationClientRpc(float speed){
        if(animator != null)
            animator.SetFloat("Speed", speed);
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
            countCollectedKeys.Value++;


            SetCountText();
        }

        if (other.gameObject.CompareTag("Chest"))
        {
            other.gameObject.SetActive(false);

            PlayerHealth playerHealth = this.gameObject.GetComponent<PlayerHealth>();
            playerHealth.IncreaseHealth();
        }
    }

    void SetCountText()
    {
        int remainingKeys = nbKeys - countCollectedKeys.Value;
        AudioSource.PlayClipAtPoint(keySound, transform.position);
        remainingKeysText.Value = "Remaining keys: " + remainingKeys.ToString();

        if (countCollectedKeys.Value == nbKeys)
        {
            Debug.Log("Keys collected");
            GridManager.Singleton.TriggerVictory.SetActive(true);
            Debug.Log("Door spawned");
            AudioSource.PlayClipAtPoint(doorSound, transform.position);
            remainingKeysText.Value = "Find the exit !";
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

    private void OnRemainingKeysTextChanged(FixedString64Bytes prevValue, FixedString64Bytes newValue)
    {
        remainingKeysTextMeshPro.text = newValue.Value;
    }
}
