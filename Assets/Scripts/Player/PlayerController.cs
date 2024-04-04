using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerController Instance { get; private set;}


    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private NavMeshAgent agent;

    private Vector3 moveDir;
    private Vector3 lastInteractDir;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    
    private void Awake() {
        if(Instance != null && Instance == this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractActionAlternate += GameInput_OnInteractAlternateAction;

        agent.speed = moveSpeed;
    }
    private void GameInput_OnInteractAlternateAction(object sender,System.EventArgs e)
    {
        
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender,System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        HandleMovementByMouse();
        HandleMovement();
        HandleInteractions();
    }

    void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        moveDir = new Vector3(inputVector.x,0,inputVector.y);

        if(moveDir!=Vector3.zero)
        {
            lastInteractDir = transform.forward;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position,lastInteractDir,out RaycastHit hit,interactDistance,countersLayerMask))
        {
            if(hit.transform.TryGetComponent(out BaseCounter baseCounter)) // Check gameobject contain Script BaseCounter
            {
                if(selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
                {
                    SetSelectedCounter(null);
                }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        moveDir = new Vector3(inputVector.x,0,inputVector.y);

        
        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance);

        if(!canMove)
        {
            // Cannot move towards direction

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x,0,0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance);
            if(canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move towards movedir

                // Attempt only z movement
                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance);

                if(canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if(canMove)
        {
            transform.position += moveDir * moveDistance; 
        }

        isWalking = moveDir != Vector3.zero;

        float speedRotation = 8.0f;
        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward,moveDir,speedRotation*Time.deltaTime);
        // transform.forward = Vector3.Slerp(transform.forward,lastInteractDir,speedRotation*Time.deltaTime);
    }

    private void HandleMovementByMouse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

   public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
