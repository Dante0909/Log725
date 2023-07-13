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

    // LightGems
    public GameObject[] lightGems;
    public float lightGemCooldown = 3.0f;
    private float lightGemCooldownTimer = 0.0f;

    // Keys collected
    private int countCollectedKeys = 0;
    private int nbKeys = 0;
    public TextMeshProUGUI remainingKeysText;
    private Rigidbody rb;
    private GameObject camera;

    // Sounds
    public AudioClip keySound;
    public AudioClip doorSound;
    public AudioClip lightGemSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = this.transform.GetChild(0).gameObject;
        nbKeys = GridManager.Singleton.NumberOfKeys;
        if(IsHost)
            camera.SetActive(true);
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
