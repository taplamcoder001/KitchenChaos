using UnityEngine;

public class BotInteract : MonoBehaviour
{
    private Table tableScript;
    public Table TableScript => tableScript;

    public void SetTable(Table table)
    {
        tableScript = table;
        table.OnHasDinner();
    }
}
