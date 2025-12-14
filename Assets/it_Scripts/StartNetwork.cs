using UnityEngine;
using Unity.Netcode;

public class StartNetwork : MonoBehaviour
{
    //Starts server Instance
    //Also all of these methods disable the network ui when the buttons are pressed and the game starts
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        gameObject.SetActive(false);
    }

    //Starts host Instance - for testing
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    //Starts Client instance to connect to server
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }
}
