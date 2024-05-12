using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private PlayerController player;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        animator.SetBool(Constant.IS_WALKING,player.IsWalking());
    }
}
