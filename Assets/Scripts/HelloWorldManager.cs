
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
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

                CambiarEquipoAzul();

                CambiarEquipoRojo();
                //SubmitNewColor();
                CambiarSinEquipo();
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

        /*static void SubmitNewPosition() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Mover" : "Peticion Cambio Posicion "))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.Move();
                Debug.Log("Me muevo");
            }
        }
        */
        static void CambiarEquipoAzul() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo Azul" : "Peticion Cambio Posicion "))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.MoverAzul();
                Debug.Log("Me muevo a la izquierda");
                player.ColoreaAzul();
                Debug.Log("Color Azul");
            }
        }
        
        static void CambiarEquipoRojo() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo Rojo" : "Peticion Cambio Posicion "))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.MoverRojo();
                Debug.Log("Me muevo a la derecha");
                player.ColoreaRojo();
                Debug.Log("Color Rojo");
            }
        }

        static void CambiarSinEquipo() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Sin Equipo" : "Peticion Cambio Posicion "))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.MoverCentro();
                Debug.Log("Me muevo al centro");
                player.ColoreaBlanco();
                Debug.Log("Color Blanco");
            }
        }

        static void SubmitNewColor() {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Color" : "Peticion Cambio Color"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.Colorea();
                Debug.Log("Color cambiado");
            }
        }
    }
}


    

