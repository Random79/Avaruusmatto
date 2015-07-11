using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "Avaruusmatto";
	private const string gameName = "matto1";
	private HostData[] hostList;

	private static bool created=false;
	void Awake()
	{
		if(!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created=true;
		}
		else Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartServer()
	{
		if(Network.isClient || Network.isServer) return;

		MasterServer.ipAddress = "127.0.0.1";
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	public void JoinServer()
	{
		if(!Network.isClient && !Network.isServer)
		{
			MasterServer.RequestHostList(typeName);
		}
	}


	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
			if(hostList.Length>0)
			{
				Network.Connect(hostList[0]);
			}
		}
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}
}
