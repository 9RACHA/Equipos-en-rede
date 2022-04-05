using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> colorJugador = new NetworkVariable<Color>();

        //network variable de color sino de material
        public static List<Color> coloresDisponibles = new List<Color>(); //colores disponibles

        public static List<Color> equipoAzul = new List<Color>();

        public static List<Color> equipoRojo = new List<Color>();

        public static List<Color> sinEquipo = new List<Color>();

        Renderer render;

        void Start() {
            //On Evento
            Position.OnValueChanged += OnPositionChange;    //Solo si cambia la posicion de position actualiza el valor
            render = GetComponent<Renderer>();
            colorJugador.OnValueChanged += OnColorChange;
            

            if (IsServer && IsOwner) {  //Hace que no se repita al inciar el start

            coloresDisponibles.Add(Color.blue); //Equipo 1
            coloresDisponibles.Add(Color.red); //Equipo 2
            coloresDisponibles.Add(Color.white); //Sin equipo
            

            Debug.Log(coloresDisponibles.Count); //3 Colores disponibles
            }

            if (IsOwner)
            {
                SubmitColorRequestServerRpc(true);
            }
            Debug.Log("Start");
        }

        public void Colorea(){
            render.material.SetColor("_Color", coloresDisponibles[Random.Range(0,coloresDisponibles.Count)]);
        }

         public void ColoreaAzul(){
            render.material.SetColor("Azul", Color.blue);
        }

        public void ColoreaRojo(){
            
            render.material.SetColor("Rojo", Color.red);
            
        }

        public void ColoreaBlanco(){
            
            render.material.SetColor("Blanco", Color.white);
            
        }

        //Solo actualiza cuando hay un cambio de valor y no cada frame cuando estaba en el Update
        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Position.Value;
        }

        public void OnColorChange(Color colorAntiguo, Color nuevoColor){
            render.material.color = colorJugador.Value;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetCentralPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        public void MoverAzul()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetPositionLeft();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionLeftRequestServerRpc();
            }
        }

        public void MoverRojo()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetPositionRight();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRightRequestServerRpc();
            }
        }

        public void MoverCentro()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetCentralPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionCentroRequestServerRpc();
            }
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionCentroRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetCentralPositionOnPlane(); //La posicion de aquien llamo el server rpc
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetCentralPositionOnPlane(); //La posicion de aquien llamo el server rpc
        }

        
        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionLeftRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetPositionLeft(); //La posicion de aquien llamo el server rpc
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitPositionRightRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetCentralPositionOnPlane(); //La posicion de aquien llamo el server rpc
        }
        

         [ServerRpc] //SIEMPRE TIPO VOID
        void SubmitColorRequestServerRpc(bool primeravez = false, ServerRpcParams rpcParams = default)
        {
            //CREA DOS VARIABLES el color asignado 
            Color oldColor = colorJugador.Value;    // color antiguo del jugador
            Color newColor = coloresDisponibles[Random.Range(0,coloresDisponibles.Count)];  // asignan un nuevo color generando un aleatorio
            coloresDisponibles.Remove(newColor);  //Borra el nuevo color
            if (!primeravez)    //Si no es el primer color
            {
                coloresDisponibles.Add(oldColor); //Añade el color antiguo para evitar quedarse sin colores
            }
            colorJugador.Value = newColor;
            Debug.Log(coloresDisponibles);
        }

        //CAMBIAR LA POSICION AL CAMBIAR DE EQUIPO
        static Vector3 GetCentralPositionOnPlane()
        {
            return new Vector3(Random.Range(0f, 1f), 1f, Random.Range(0f, 1f));

        }

        static Vector3 GetPositionLeft(){
            return new Vector3(Random.Range(-5f, -2f), 1f, Random.Range(-5f, -2f));
        }

        static Vector3 GetPositionRight(){
            return new Vector3(Random.Range(2f, 3f), 1f, Random.Range(2f, 3f));
        }

        void Update()
        {
            render.material.SetColor("Color", colorJugador.Value);
            //transform.position = Position.Value;
        }
    }
}