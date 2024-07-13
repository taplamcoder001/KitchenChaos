using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private List<Table> tables = new List<Table>();
    public List<Table> Tables => tables;
    private void Update()
    {
        /// Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetTable();
        }
    }

    public Table GetTable()
    {
        if(tables.Count == 0)
        {
            Debug.LogError("Don't have element");
            return null;
        }

        Table table = tables[RandomIndexInList()];
        return table;
    }

    private int RandomIndexInList()
    {
        return Random.Range(0, tables.Count - 1);
    }

    public void AddTable(Table table)
    {
        tables.Add(table);
    }
}
