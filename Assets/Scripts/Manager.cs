using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class Manager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitEquipoAzul1();
                SubmitEquipoRojo2();
                SubmitSinEquipo();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        
        static void SubmitEquipoAzul1()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo Azul" : "Equipo Azul"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.JugadorEquipo(1);
            }
        }
        static void SubmitEquipoRojo2()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo Rojo" : "Equipo Rojo"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.JugadorEquipo(2);
            }
        }
        static void SubmitSinEquipo()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Sin Equipo" : "Sin Equipo"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.JugadorEquipo(0);
            }
        }
    }
}
