using UnityEngine;

public class BotAnimation : MonoBehaviour
{
    [SerializeField] private GameObject body;

    public void GetPosForBody(Vector3 pos)
    {
        body.transform.position = pos;
    }
    
    public void DisActive()
    {
        gameObject.SetActive(false);
    }
}
