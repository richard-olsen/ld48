using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedMovementSystem : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private int playerAllowedMoves = 3;
    private int playerActionsLeft;

    [SerializeField]
    private int enemiesAllowedMoves = 2;

    [System.Serializable]
    public struct EnemyEntry
    {
        public GridEnemyBase enemy;
        public int actionsLeft;
    }

    [SerializeField]
    private EnemyEntry[] enemies;

    private float aiTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsLeft = playerAllowedMoves;
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].actionsLeft = enemiesAllowedMoves;
    }

    // Update is called once per frame
    void Update()
    {
        aiTimer += Time.deltaTime;

        if (playerActionsLeft > 0)
        {
            if (player.DoActions())
                playerActionsLeft--;

            if (playerActionsLeft == 0)
                aiTimer = 0;

            return;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].actionsLeft == 0)
            {
                aiTimer = 0;
                continue;
            }

            if (aiTimer < 1.0f)
                return;

            aiTimer = 0;

            if (enemies[i].enemy.DoActions())
                enemies[i].actionsLeft--;

            return;
        }

        playerActionsLeft = playerAllowedMoves;
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].actionsLeft = enemiesAllowedMoves;
    }
}
