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

    [SerializeField]
    private List<EnemyEntry> enemies;

    private float aiTimer = 0;

    public void AddEnemy(GridEnemyBase enemy)
    {
        if (enemies == null)
            enemies = new List<EnemyEntry>();

        EnemyEntry entry = new EnemyEntry();

        entry.enemy = enemy;
        entry.actionsLeft = enemiesAllowedMoves;

        enemies.Add(entry);
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }

    public void ResetMoves()
    {
        if (enemies == null)
            enemies = new List<EnemyEntry>();

        playerActionsLeft = playerAllowedMoves;
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyEntry entry = enemies[i];
            entry.actionsLeft = enemiesAllowedMoves;
        }

        Debug.Log("What's calling this");
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
            if (player.DoActions())
                playerActionsLeft--;

            if (playerActionsLeft <= 0)
                aiTimer = 0;

            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyEntry entry = enemies[i];

            if (entry.actionsLeft == 0)
            {
                continue;
            }

            if (aiTimer < 1.0f)
                return;

            aiTimer = 0;

            if (entry.enemy.DoActions())
            {
                entry.actionsLeft = entry.actionsLeft - 1;
            }


            return;
        }

        ResetMoves();
    }
}
