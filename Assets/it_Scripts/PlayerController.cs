using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    //Variables
    public TextMeshProUGUI scoreText;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int score = 0;
    [SerializeField] private EndGame endGame;
    private static bool frozen = false;

    //This is called when the player is spawned. Sets the camera to only follow the client
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        var cam = Camera.main;
        if (cam == null) return;

        var follow = cam.GetComponent<PlayerCamera>();
        if (follow != null)
        {
            follow.SetTarget(transform);
        }
    }

    //Sets movement method and restricts it to the client that owns that player - WASD to move
    private void Update()
    {
        if (!IsOwner || !Application.isFocused || frozen)
            return;

        if (IsOwner && scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }

        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical"); 

        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        transform.position += moveDirection * speed * Time.deltaTime;
    }

    //This is for handling present pickups - only client owner can request to pickup present
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner || frozen)
            return;
    
        if (!other.CompareTag("Present")) return;

        if (!other.TryGetComponent<NetworkObject>(out var presentNetObj))
        {
            return;
        }

        CollectPresentServerRpc(new NetworkObjectReference(presentNetObj));
    }

    //Collection of food for server - keeps track of each clients score
    [ServerRpc(RequireOwnership = true)]
    private void CollectPresentServerRpc(NetworkObjectReference presentRef)
    {
        if (!presentRef.TryGet(out NetworkObject presentNetObj))
        {
            return;
        }

        presentNetObj.Despawn(true);

        PresentSpawner spawner = FindObjectOfType<PresentSpawner>();
        if (spawner != null)
        {
            spawner.SpawnOnePresent();
        }

        score++;
        UpdateScoreClientRpc(score);

        //win criteria is 10 for testing but if it was released criteria would be higher.
        if (score >= 10)
        {
            EndMatchClientRpc(OwnerClientId);
            return;
        }

    }

    //Updates clients score if owner
    [ClientRpc]
    private void UpdateScoreClientRpc(int newScore)
    {
        score = newScore;

        // Score Text
        if (scoreText == null)
        {
            var go = GameObject.Find("ScoreText");
            if (go != null) scoreText = go.GetComponent<TextMeshProUGUI>();
        }

        if (scoreText != null)
            scoreText.text = $"Score: {score}";

    }

    //Freezes the game for everyone on server when one client reaches win quota
    [ClientRpc]
    private void EndMatchClientRpc(ulong winnerClientId)
    {
        frozen = true;

        // Win or lose text per client
        var endGame = FindFirstObjectByType<EndGame>();
        if (endGame != null)
        {
            bool iWon = NetworkManager.Singleton.LocalClientId == winnerClientId;
            endGame.SetEndText(true, iWon);
        }
    }
}