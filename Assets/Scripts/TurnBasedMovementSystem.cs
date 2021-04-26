using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TurnBasedMovementSystem : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private int playerAllowedMoves = 3;
    private int playerActionsLeft;

    [SerializeField]
    private int enemiesAllowedMoves = 2;

    [System.Serializable]
    public class EnemyEntry
    {
        public GridEnemyBase enemy;
        public int actionsLeft;
    }

    private List<EnemyEntry> enemies;


    public class RemoveEnemyEntry
    {
        public GridEnemyBase enemy;
        public bool removeFromWorld;
    }

    private Queue<RemoveEnemyEntry> removeEnemy;

    private float aiTimer = 0;

    public void AddEnemy(GridEnemyBase enemy)
    {
        EnemyEntry entry = new EnemyEntry();

        entry.enemy = enemy;
        entry.actionsLeft = enemiesAllowedMoves;

        enemies.Add(entry);
    }

    public void RemoveEnemy(GridEnemyBase enemy, bool removeObject)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyEntry entry = enemies[i];

            if (entry.enemy == enemy)
            {
                RemoveEnemyEntry rmEntry = new RemoveEnemyEntry();
                rmEntry.enemy = enemy;
                rmEntry.removeFromWorld = removeObject;
                removeEnemy.Enqueue(rmEntry);
                break;
            }
        }
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }

    public void ResetMoves()
    {
        playerActionsLeft = playerAllowedMoves;
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyEntry entry = enemies[i];
            entry.actionsLeft = enemiesAllowedMoves;
        }
    }

    private void Awake()
    {
        enemies = new List<EnemyEntry>();
        removeEnemy = new Queue<RemoveEnemyEntry>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerObject, "A player must exist in the scene!");

        player = playerObject.GetComponent<Player>();
        Assert.IsNotNull(player, "Player GameObject REQUIRES the Player class! Are you using the prefab?");

        ResetMoves();
    }

    void Update()
    {
        aiTimer += Time.deltaTime;

        if (playerActionsLeft > 0 || enemies.Count == 0)
        {
            int actions = player.DoActions(playerActionsLeft);
            
            playerActionsLeft -= actions;

            if (playerActionsLeft <= 0)
                aiTimer = 0;

            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyEntry entry = enemies[i];

            if (entry.actionsLeft <= 0)
            {
                continue;
            }

            if (aiTimer < 1.0f)
                return;

            aiTimer = 0;

            int actions = entry.enemy.DoActions(entry.actionsLeft);

            entry.actionsLeft = entry.actionsLeft - actions;

            return;
        }

        // Remove all dead enemies or enemies that need to be removed

        while (removeEnemy.Count > 0)
        {
            RemoveEnemyEntry enemy = removeEnemy.Dequeue();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].enemy == enemy.enemy)
                {
                    Debug.Log("Remove Enemy");
                    enemies.RemoveAt(i);

                    if (enemy.removeFromWorld)
                        Destroy(enemy.enemy.gameObject);

                    break;
                }
            }
        }

        ResetMoves();
    }
}
