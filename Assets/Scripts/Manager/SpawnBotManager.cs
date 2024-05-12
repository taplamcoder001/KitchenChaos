using System.Collections;
using UnityEngine;

public class SpawnBotManager : Singleton<SpawnBotManager>
{
    [SerializeField] private Transform pointSpawn;
    [SerializeField] private int limitAmountBot;
    [SerializeField] private float timeSpawn = 10f;
    [SerializeField] private bool isSpawn;

    private float currentTime = 0f;

    private void Update()
    {
        SpawnBot();
    }

    private void SpawnBot()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeSpawn) // And condition have slot
        {
            Table table = TableManager.Instance.GetTable();
            if(table == null)   // When table off
            {
                currentTime = 0;
                return;
            }

            TableManager.Instance.RemoveTable(table);
            for (int i = 0; i < RandomQuantityBot(); i++)
            {
                GameObject bot = PoolingObject.Instance.GetBot();
                bot.transform.position = pointSpawn.position;
                CharacterMotion motion = bot.GetComponent<CharacterMotion>();
                Chair chair = table.ListChair[i];
                BotInteract botInteract = bot.GetComponent<BotInteract>();
                botInteract.SetTable(table);
                motion.SetUpStats(chair.TopPointTranform.position);
            }
            currentTime = 0;
        }
    }

    private int RandomQuantityBot()
    {
        return Random.Range(1, limitAmountBot);
    }
}
