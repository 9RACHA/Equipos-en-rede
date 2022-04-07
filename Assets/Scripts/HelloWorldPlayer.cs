using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> PosicionJugador = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> colorJugador = new NetworkVariable<Color>();

        //network variable de color sino de material
        public static List<Color> coloresDisponibles = new List<Color>(); //colores disponibles

        public static List<Color> equipoAzul = new List<Color>();

        public static List<Color> equipoRojo = new List<Color>();

        //public static List<Color> sinEquipo = new List<Color>();

        Renderer render;

        void Start() {
            //On Evento
            PosicionJugador.OnValueChanged += OnPositionChange;    //Solo si cambia la posicion de position actualiza el valor
            render = GetComponent<Renderer>();
            colorJugador.OnValueChanged += OnColorChange; 

        }

        public void Colorea(){
            render.material.SetColor("_Color", coloresDisponibles[Random.Range(0,coloresDisponibles.Count)]);
        }

         

        //Solo actualiza cuando hay un cambio de valor y no cada frame cuando estaba en el Update
        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = PosicionJugador.Value;
        }

        public void OnColorChange(Color colorAntiguo, Color nuevoColor){
            render.material.color = colorJugador.Value;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer && IsOwner) {  //Hace que no se repita al inciar el start

                coloresDisponibles.Add(Color.blue); //Equipo 1
                
                coloresDisponibles.Add(Color.red); //Equipo 2

                coloresDisponibles.Add(Color.white); //Sin equipo
            
            //El color blanco no se corresponde con su indice
            
            }
            if (IsOwner)
            {
                SubmitPositionColoreaBlancoRequestServerRpc();
                SubmitPositionCentroRequestServerRpc();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetCentralPositionOnPlane();
                transform.position = randomPosition;
                PosicionJugador.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        public void MoverAzul()
        {
            SubmitPositionLeftRequestServerRpc();
            SubmitPositionColoreaAzulRequestServerRpc();
        }

        public void MoverRojo()
        {
            
            SubmitPositionRightRequestServerRpc();
            SubmitPositionColoreaRojoRequestServerRpc();
        }

        public void MoverCentro()
        {
            
            SubmitPositionCentroRequestServerRpc();
            SubmitPositionColoreaBlancoRequestServerRpc();
        }
        
        [ServerRpc]

         void SubmitPositionColoreaAzulRequestServerRpc(ServerRpcParams rpcParams = default){
            //render.material.SetColor("Azul", Color.blue);
            colorJugador.Value = coloresDisponibles[0];
            Debug.Log("Equipo Azul");
          
        }
        [ServerRpc]
         void SubmitPositionColoreaRojoRequestServerRpc(ServerRpcParams rpcParams = default){ 
            //render.material.SetColor("Rojo", Color.red);
            colorJugador.Value = coloresDisponibles[1];
            Debug.Log("Equipo rojo");

        }
        [ServerRpc]
        void SubmitPositionColoreaBlancoRequestServerRpc(ServerRpcParams rpcParams = default){
            //render.material.SetColor("Blanco", Color.white);
            colorJugador.Value = coloresDisponibles[2];

            Debug.Log("Sin equipo");
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionCentroRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionJugador.Value = GetCentralPositionOnPlane(); //La posicion de aquien llamo el server rpc
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionJugador.Value = GetCentralPositionOnPlane(); //La posicion de aquien llamo el server rpc
        }

        
        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionLeftRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionJugador.Value = GetPositionLeft(); //La posicion de aquien llamo el server rpc
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionRightRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionJugador.Value = GetPositionRight(); //La posicion de aquien llamo el server rpc
        }

        //CAMBIAR LA POSICION AL CAMBIAR DE EQUIPO
        static Vector3 GetCentralPositionOnPlane()
        {
            return new Vector3(Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));
        }

        static Vector3 GetPositionLeft(){
            return new Vector3(Random.Range(-5f, -3f), 1f, Random.Range(-5f, -3f));
        }

        static Vector3 GetPositionRight(){
            return new Vector3(Random.Range(2.5f, 4f), 1f, Random.Range(1.5f, 4f));
        }

        void Update()
        {
            render.material.color = colorJugador.Value;
           // render.material.SetColor("Color", colorJugador.Value);
            transform.position = PosicionJugador.Value;
        }
    }
}