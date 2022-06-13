using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        //Varible para la posicion
        public NetworkVariable<Vector3> Posicion = new NetworkVariable<Vector3>();
        //Variable para el color
        public NetworkVariable<Color> ColorJugador = new NetworkVariable<Color>();
        //Variable para el numero del equipo
        public NetworkVariable<int> NumeroEquipo = new NetworkVariable<int>();

        //Lista estatica con los colores blanco, azul, rojo
        public static List<Color> coloresGuardados= new List<Color>();
        //Lista estatica con los equipos
        public static List<int> ListaEquipo = new List<int>();//public static List<int> equipo1 = new List<int>();
        
        //El numero maximo de jugadores por equipo 2
        public static int numeroMax = 2;
        
        //Para acceder al componente
        private Renderer render;

        public void Start(){
            Posicion.OnValueChanged += OnPosicionChange; //La posicion cambiara al cambiar el valor += On...Change
            ColorJugador.OnValueChanged += OnColorJugadorChange; //El color cambiara al cambiar el valor += con la estructura On...Change
            render = GetComponent<Renderer>(); //Para enlazarlo
            
        }

        public void OnPosicionChange(Vector3 previousValue, Vector3 newValue){ //La posicion antigua tendra una nueva posicion
            transform.position = Posicion.Value; //el componente transform y la seccion position sera igual al valor de la network variable Posicion
        }

        public void OnColorJugadorChange(Color previousValue, Color newValue){ //El color antiguo tendra un nuevo color
            render.material.color = newValue;   //El componente rederer se asociara con un material y un color lo que asignara un nuevo valor color
        }

        public override void OnNetworkSpawn()   //Nace 
        {
            if(IsServer && IsOwner){ //Si es Servidor Y es Propietario
                //Se añade a la lista 3 equipos *
                ListaEquipo.Add(0);
                ListaEquipo.Add(0);
                ListaEquipo.Add(0);

                //Se añade a la lista los 3 colores disponibles
                coloresGuardados.Add(Color.white);
                coloresGuardados.Add(Color.blue);
                coloresGuardados.Add(Color.red);
            }
            if (IsOwner) //Si es propietario
            {
                JugadorEquipo(-1); // Jugador equipo tendra asignado -1 en este caso
            }
        }
        
        public void JugadorEquipo(int x){ 
            SubmitJugadorEquipoSolicitudServerRpc(x); //x para poder definir una variable a mayores
        }

        [ServerRpc]
        void SubmitJugadorEquipoSolicitudServerRpc(int NumeroEquipox, ServerRpcParams rpcParams = default)
        {
            Debug.Log("Blanco " + ListaEquipo[0]);
            Debug.Log("Azul " + ListaEquipo[1]);
            Debug.Log("Rojo " + ListaEquipo[2]);

            if(NumeroEquipox == -1){ //Si el equipo es igual a -1 Blanco
                NumeroEquipo.Value = 0; 
                Posicion.Value = GetRandomPosicionEquipo(0);
                Color newColor = coloresGuardados[0];
                ColorJugador.Value = newColor;
                ListaEquipo[0]++; //Se añade 1 mas a la lista
            }
            
            /*
            if(NumeroEquipox == 1){ //Si el equipo es igual a Azul
                NumeroEquipo.Value = 1;
                Posicion.Value = GetRandomPosicionEquipo(1);
                Color newColor = coloresGuardados[1];
                ColorJugador.Value = newColor;
                ListaEquipo[1]++; //Se añade 1 mas a la lista
            }

            if(NumeroEquipox == 2){ //Si el equipo es igual al Rojo
                NumeroEquipo.Value = 2;
                Posicion.Value = GetRandomPosicionEquipo(2);
                Color newColor = coloresGuardados[2];
                ColorJugador.Value = newColor;
                ListaEquipo[2]++; //Se añade 1 mas a la lista
            }*/
            
            //Sino ListaEquipo en [] porque no se puede usar como metodo al ser una variable menor o igual al numeroMax OR que el numeroEquipox sea igual a 0
            else if(ListaEquipo[NumeroEquipox] <= numeroMax || NumeroEquipox == 0){
                
                ListaEquipo[NumeroEquipo.Value] --;
                NumeroEquipo.Value = NumeroEquipox;
                Posicion.Value = GetRandomPosicionEquipo(NumeroEquipox);
                Color newColor = coloresGuardados[NumeroEquipo.Value];
                ColorJugador.Value = newColor;
                ListaEquipo[NumeroEquipo.Value] ++;
                Debug.Log("Jugador Blanco " + ListaEquipo[0]);
                Debug.Log("Jugador Azul " + ListaEquipo[1]);
                Debug.Log("Jugador Rojo " + ListaEquipo[2]);

            }else{
                Debug.Log("Equipo " + NumeroEquipox + " lleno");
            }
        }

        static Vector3 GetRandomPosicionEquipo(int x)   //Metodo para Obtener una posicion aleatoria dependiendo del equipo seleccionado int x para permitir que sea igual 0 o 1 distintos equipos
        {
            if(x == 0){ //Si x es igual a 0 (Sin Equipo)
                return new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-3f, 3f)); //Devuelve un nuevo Vector 3 aleatorio entre dichos parametros posicion Central
            }else if(x == 1){ //Mas si x es igual 1(Equipo Azul)
                return new Vector3(Random.Range(-5f, -2f), 1f, Random.Range(-3f, 3f)); //Devuelve un nuevo Vector 3 aleatorio entre dichos parametros posicion Izquierda
            }else{ //Sino la otra variante
                return new Vector3(Random.Range(2f, 5f), 1f, Random.Range(-3f, 3f)); //Devuelve un nuevo Vector 3 aleatorio entre dichos parametros posicion Derecha
            }
        }
        
        void Update()
        {
            transform.position = Posicion.Value; //Se comprueba constantemente: el componente transform y la seccion position sera igual al valor de la network variable Posicion
        }
    }
}