using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES

    #region Movement
    [Header("Movement")]
    [SerializeField, Tooltip("Efecto de part�cula al hacer clic")] ParticleSystem clickEffect;
    [SerializeField, Tooltip("Efecto de part�cula al moverse la ambulancia")] ParticleSystem ambulanceSmoke;
    [SerializeField, Tooltip("Capa de objetos clickeables")] LayerMask clickableLayer;

    [SerializeField, Tooltip("Distancia m�nima angular para que se considere rotaci�n")] float minAngularDistance = 1.0f;

    public float lookRotationSpeed;                              // Velocidad de rotaci�n del jugador
    Vector3 desiredForward;                                      // Direcci�n deseada del jugador
    Vector3 desiredUp;                                          // Direcci�n deseada hacia arriba del jugador

    bool isMoving = false;                                       // Indica si el jugador est� en movimiento

    #endregion


    #region Singleton
    [Header("Singleton")]
    static PlayerController playerController;                    // Instancia est�tica del controlador
    public static PlayerController instance
    {
        get
        {
            return RequestPlayerController();
        }
    }

    static PlayerController RequestPlayerController()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        return playerController;
    }
    #endregion

    #endregion

    #region COMPONENTS
    CustomActions input;                                          // Acciones personalizadas para el input
    NavMeshAgent agent;                                           // Agente de navegaci�n para el movimiento del jugador
    #endregion

    #region UnityMethods
    private void Awake()
    {
        // Inicializaci�n de componentes y variables
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;                             // No actualizamos la rotaci�n del agente
        agent.updateUpAxis = false;                               // No actualizamos el eje vertical del agente
        desiredForward = Vector3.forward;                          // Direcci�n inicial hacia adelante

        input = new CustomActions();                               // Inicializaci�n de las acciones personalizadas
        EnableInput();                                             // Habilitar el input

        ambulanceSmoke.Stop();
    }
    private void OnEnable()
    {
        // Habilitar el input y agregar listeners para pausar y reanudar
        input.Enable();
    }

    private void OnDisable()
    {
        // Deshabilitar el input y remover listeners al desactivar el objeto
        if (agent != null)
            input.Disable();
    }

    #endregion

    /// <summary>
    /// M�todo llamado cuando se hace clic para mover al jugador.
    /// </summary>
    void ClickToMove(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            // Establecer la nueva posici�n del agente
            agent.destination = hit.point;
            if (clickEffect)
            {
                ambulanceSmoke.Play();
                // Instanciar el efecto de clic en la posici�n del raycast
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), Quaternion.LookRotation(-hit.normal));
            }
            isMoving = true;  // Marcar que el jugador est� en movimiento
        }
    }

    private void Update()
    {
        // Si el jugador est� en movimiento, revisar su distancia al destino
        if (isMoving)
        {
            FixOrientationAlignment();

            // Si la distancia al destino es menor o igual a 1, el movimiento ha terminado
            float distanceToDestination = Vector3.Distance(transform.position, agent.destination);

            if (distanceToDestination <= 1f)
            {
                // Detener el movimiento de la ambulancia  
                isMoving = false;
                ambulanceSmoke.Stop();
            }
        }
    }

    /// <summary>
    /// M�todo que arregla la alineaci�n de la orientaci�n del jugador.
    /// </summary>
    private void FixOrientationAlignment()
    {
        // Si la velocidad deseada es mayor que un valor m�nimo, actualizamos la direcci�n
        if (agent.desiredVelocity.sqrMagnitude > 0.1f)
        {
            desiredForward = agent.desiredVelocity.normalized;  // Actualizar direcci�n hacia adelante
        }

        desiredUp = GetGroundNormal();  // Obtener la normal del suelo
        Quaternion desiredForwardRotation = Quaternion.identity;
        Quaternion desiredUpRotation = Quaternion.identity;

        // Calcular el �ngulo de rotaci�n hacia adelante
        float forwardAngle = Vector3.Angle(transform.forward, desiredForward);
        if (forwardAngle > minAngularDistance)
        {
            desiredForwardRotation = Quaternion.AngleAxis(forwardAngle, Vector3.Cross(transform.forward, desiredForward));
        }

        // Calcular el �ngulo de rotaci�n hacia arriba
        float upAngle = Vector3.Angle(transform.up, desiredUp);
        if (upAngle > minAngularDistance)
        {
            desiredUpRotation = Quaternion.AngleAxis(upAngle, Vector3.Cross(transform.up, desiredUp));
        }

        // Calcular la rotaci�n final deseada y aplicarla suavemente
        Quaternion desiredRotation = desiredForwardRotation * desiredUpRotation * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * lookRotationSpeed);
    }

    /// <summary>
    /// Obtener la normal del suelo utilizando un raycast hacia abajo.
    /// </summary>
    private Vector3 GetGroundNormal()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, clickableLayer))
        {
            return info.normal;  // devuelve la normal del suelo
        }
        return Vector3.up;  // Si no se encuentra, devuelve el vector hacia arriba
    }

    #region InputSection

    /// <summary>
    /// Detener el movimiento del jugador.
    /// </summary>
    public void StopPlayerMovement()
    {
        agent.destination = transform.position;  // Detener el movimiento del agente

        DisableInput();
    }
    bool firstStop = true;
    public void ResumeFirstMovement()
    {
        if (firstStop)
        {
            EnableInput();
            firstStop = false;
        }
    }

    /// <summary>
    /// Reanudar el movimiento del jugador.
    /// </summary>
    public void ResumePlayerMovement()
    {
        EnableInput();
    }

    /// <summary>
    /// Deshabilitar el input del jugador.
    /// </summary>
    void DisableInput()
    {
        input._3dMap.Move.performed -= ClickToMove;
    }

    /// <summary>
    /// Habilitar el input del jugador.
    /// </summary>
    void EnableInput()
    {
        input._3dMap.Move.performed += ClickToMove;
    }
    #endregion
}