using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        //Variables para la posicion
        public NetworkVariable<Vector3> PosicionCentroSinEquipo = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> PosicionIzquierdaAzul = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> PosicionDerechaRojo = new NetworkVariable<Vector3>();

        //network variable de color
        public NetworkVariable<Color> colorCambia = new NetworkVariable<Color>();

        /*
        public static List<Color> coloresDisponibles = new List<Color>(); //colores disponibles

        public static List<Color> equipoAzul = new List<Color>();

        public static List<Color> equipoRojo = new List<Color>();

        public static List<Color> sinEquipo = new List<Color>();
        */
        
        //Lista de jugadores
        public List<GameObject> jugadores = new List<GameObject>();

        Renderer render;

        void Start() {
            //On Evento
            PosicionCentroSinEquipo.OnValueChanged += OnPosicionCentroSinEquipoChange;    //Solo si cambia la posicion de position actualiza el valor
            PosicionIzquierdaAzul.OnValueChanged += OnPosicionIzquierdaAzulChange;
            PosicionDerechaRojo.OnValueChanged += OnPosicionDerechaRojoChange;

            /*
            render = GetComponent<Renderer>();
            colorJugador.OnValueChanged += OnColorChange;*/
            

            GetPosicionAleatoriaSinEquipo(); //GetRandomPositionSinEquipo();

        }

        //Solo actualiza cuando hay un cambio de valor y no cada frame cuando estaba en el Update
        public void OnPosicionCentroSinEquipoChange(Vector3 previousValue, Vector3 newValue){
            transform.position = PosicionCentroSinEquipo.Value;
        }

        public void OnPosicionIzquierdaAzulChange(Vector3 previousValue, Vector3 newValue){
            transform.position = PosicionIzquierdaAzul.Value;
        }

        public void OnPosicionDerechaRojoChange(Vector3 previousValue, Vector3 newValue){
            transform.position = PosicionDerechaRojo.Value;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
               // Move();
            }
        }

        //metodo que hace mover al equipo 1
        public void MoverEquipoAzul1()
        {
            //Mueve aleatoriamente dentro de la zona especificada
            SubmitEquipoAzul1PeticionServerRpc();//Tiene que acabar con ServerRpc
            Debug.Log("Me muevo a Equipo Azul");
        }

        
        public void MoverEquipoRojo2()
        {
            //Mueve aleatoriamente dentro de la zona especificada
            SubmitEquipoRojo2PeticionServerRpc();//Tiene que acabar con ServerRpc
            Debug.Log("Me muevo a Equipo Rojo");
        }

            public void MoverSinEquipo()
        {
            //Mueve aleatoriamente dentro de la zona especificada
            SubmitSinEquipoPeticionServerRpc();//Tiene que acabar con ServerRpc
            Debug.Log("Me muevo a Sin Equipo");
        }

             //CAMBIAR LA POSICION AL CAMBIAR DE EQUIPO
        static Vector3 GetPosicionAleatoriaSinEquipo()
        {
            return new Vector3(Random.Range(-1.8f, 1.8f), 1f, Random.Range(-3f, 3f));

        }

        static Vector3 GetPosicionAleatoriaEquipoAzul1(){
            return new Vector3(Random.Range(-5f, -1.8f), 1f, Random.Range(-3f, 3f));
        }

        static Vector3 GetPosicionAleatoriaEquipoRojo2(){
            return new Vector3(Random.Range(1.8f, 5f), 1f, Random.Range(-3f, 3f));
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitSinEquipoPeticionServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionCentroSinEquipo.Value = GetPosicionAleatoriaSinEquipo(); //La posicion de aquien llamo el server rpc
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

         [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitEquipoAzul1PeticionServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionIzquierdaAzul.Value = GetPosicionAleatoriaEquipoAzul1(); //La posicion de aquien llamo el server rpc

            gameObject.GetComponent<Renderer>().material.color = Color.blue;

            //Si la lista de jugadores contiene mas de 2 no se podra añadir otro
            if (jugadores.Count > 2)
            {
                Debug.Log("No se puede añadir a equipo");
            } else
            {
                Debug.Log("Jugador añadido");
            }
        }

        [ServerRpc] //SIEMPRE TIPO VOID por tanto no devuelve nada
        void SubmitEquipoRojo2PeticionServerRpc(ServerRpcParams rpcParams = default)
        {
            PosicionDerechaRojo.Value = GetPosicionAleatoriaEquipoRojo2(); //La posicion de aquien llamo el server rpc
            gameObject.GetComponent<Renderer>().material.color = Color.red;

            /*if (jugadores.Count > 2)
            { else
            {
                    Debug.Log("Max 2 jugadores");
                }
            }*/}
        }
    }

  