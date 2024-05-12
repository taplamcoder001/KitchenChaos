using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMotion))]
public class PlayerController : Singleton<PlayerController>, IKitchenObjectParent
{
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private CharacterMotion motion;
    private Vector3 moveDir;
    private Vector3 lastInteractDir;
    private float moveSpeed;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private Transform transformPlayer;

    // Value for interact by mouse
    private BaseCounter baseCounter;
    private bool hasInteract;

    protected override void Awake()
    {
        base.Awake();
        transformPlayer = transform;
        SetUpStats();
    }

    private void SetUpStats()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractActionAlternate += GameInput_OnInteractAlternateAction;

        motion = GetComponent<CharacterMotion>();
        moveSpeed = motion.Speed;
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {

        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();

        HandleMovementByMouse();
        HandleInteractByMouse();
    }

    void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = transform.forward;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter)) // Check gameobject contain Script BaseCounter
            {
                if (selectedCounter != baseCounter)
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

        moveDir = new Vector3(inputVector.x, 0, inputVector.y);


        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards direction

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move towards movedir

                // Attempt only z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        if (isWalking)
        {
            motion.AgentClear();
        }

        float speedRotation = 8.0f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, speedRotation * Time.deltaTime);
        // transform.forward = Vector3.Slerp(transform.forward,lastInteractDir,speedRotation*Time.deltaTime);
    }

    private void HandleMovementByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, countersLayerMask))
            {
                if(hit.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    motion.Movement(baseCounter.TransformBaseCounter.position);
                    this.baseCounter = baseCounter;
                    hasInteract = true;
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                motion.Movement(hit.point);
                Debug.Log(hit.transform.name);
            }

        }
    }

    private void HandleInteractByMouse()
    {
        float interactDistance = 2f;

        if(baseCounter == null)
        {
            return;
        }
        float distance = Vector3.Distance(transformPlayer.position, baseCounter.transform.position);
        if (distance <= interactDistance)
        {
            SetSelectedCounter(baseCounter);
            if(!hasInteract)
            {
                return;
            }
            baseCounter.Interact(this);
            hasInteract = false;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
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

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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
