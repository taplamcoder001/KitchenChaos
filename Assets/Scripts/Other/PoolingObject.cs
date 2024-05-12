using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : Singleton<PoolingObject>
{
    [SerializeField] private GameObject[] botPrefabs;
    [SerializeField] private int quantityBot;

    private Queue<GameObject> queueBotObject; 

    protected override void Awake()
    {
        base.Awake();
        queueBotObject = new Queue<GameObject>();
        InitializedBot();
    }

    private void InitializedBot()
    {
        for (int i = 0; i < quantityBot; i++)
        {
            int random = Random.Range(0, botPrefabs.Length);
            GameObject bot = Instantiate(botPrefabs[random],transform);
            bot.SetActive(false);
            queueBotObject.Enqueue(bot);
        }
    }

    public GameObject GetBot()
    {
        if(queueBotObject.Count > 0)
        {
            GameObject botObject = queueBotObject.Dequeue();
            botObject.SetActive(true);
            return botObject;
        }
        return null;
    }

    public void InactiveBot(GameObject botObject)
    {
        botObject.SetActive(false);
        queueBotObject.Enqueue(botObject);
    }
}
