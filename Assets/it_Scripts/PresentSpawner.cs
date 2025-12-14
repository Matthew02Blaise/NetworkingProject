using UnityEngine;
using Unity.Netcode;

//Server authoritave method for spawning presents
public class PresentSpawner : NetworkBehaviour
{
    //Variables
    public GameObject presentPrefab;
    public int presentCount = 20;
    public Vector2 spawnArea = new Vector2(8f, 4.5f);

    //Spawns first set of presents
    public override void OnNetworkSpawn()
    {
        // Only the server spawns present pickups
        if (!IsServer) return;

        for (int i = 0; i < presentCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                0);
            GameObject present = Instantiate(presentPrefab, pos, Quaternion.identity);
            present.GetComponent<NetworkObject>().Spawn();
        }
    }

    //This method is for spawning more presents when one is collected
    public void SpawnOnePresent()
    {
        Vector3 pos = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            0);

        GameObject present = Instantiate(presentPrefab, pos, Quaternion.identity);
        present.GetComponent<NetworkObject>().Spawn();
    }
}
