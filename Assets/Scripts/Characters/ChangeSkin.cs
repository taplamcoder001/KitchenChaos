using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    [SerializeField] private List<Material> materials = new List<Material>();

    [SerializeField] private List<GameObject> partBody = new List<GameObject>();

    public void ChangeMarterial()
    {
        int index = RandomSkin();
        foreach(GameObject body in partBody)
        {
            MeshRenderer meshRenderer = body.GetComponent<MeshRenderer>();
            meshRenderer.material = materials[index];
        }
    }

    private int RandomSkin()
    {
        int index = Random.Range(0, materials.Count - 1);
        return index;
    }
}
