using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;



public class NetworkController : NetworkManager
{

    public int index = 0;
    public int connectCount = 0;

    void Start()
    {

    }

    // Client
    public override void OnClientConnect(NetworkConnection conn)
    {
        // A custom identifier we want to transmit from client to server on connection
        //int id = GetCustomValue();

        // Create message which stores our custom identifier
        IntegerMessage msg = new IntegerMessage(index);

        if (!clientLoadedScene)
        {
            ClientScene.AddPlayer(conn, 0, msg);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }


    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        // Create message which stores our custom identifier
        IntegerMessage msg = new IntegerMessage(index);

        bool addPlayer = (ClientScene.localPlayers.Count == 0);
        bool foundPlayer = false;
        for (int i = 0; i < ClientScene.localPlayers.Count; i++)
        {
            if (ClientScene.localPlayers[i].gameObject != null)
            {
                foundPlayer = true;
                ClientScene.Ready(conn);
                Debug.Log(ClientScene.localPlayers[i].gameObject);
                if (ClientScene.localPlayers[i].gameObject.tag == "Player")
                {
                    ClientScene.localPlayers[i].gameObject.transform.position = GameObject.Find("HumanRespawnPos").transform.position;
                    MonoBehaviour[] comps = ClientScene.localPlayers[i].gameObject.GetComponentsInChildren<MonoBehaviour>();
                    foreach(MonoBehaviour c in comps)
                    {
                        c.enabled = true;
                    }
                }
                else if(ClientScene.localPlayers[i].gameObject.tag == "Enemy")
                {
                    ClientScene.localPlayers[i].gameObject.transform.position = GameObject.Find("MonsterRespawnPos").transform.position;
                    MonoBehaviour[] comps = ClientScene.localPlayers[i].gameObject.GetComponentsInChildren<MonoBehaviour>();
                    foreach (MonoBehaviour c in comps)
                    {
                        c.enabled = true;
                    }
                }
                break;
                
            }
        }
        if (!foundPlayer)
        {
            // there are players, but their game objects have all been deleted
            addPlayer = true;
        }
        if (addPlayer)
        {
            // Call Add player and pass the message
            ClientScene.AddPlayer(conn, 0, msg);
        }
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        connectCount = 0;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        connectCount = 0;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        connectCount -= 1;
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    // Server
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, GameObject.Find("ChargePos").transform.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        connectCount += 1;
    }

   
    
}
