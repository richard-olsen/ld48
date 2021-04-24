using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenZone : MonoBehaviour
{
    public int oxygenGiven = 1;

    public const float TIME_TO_GIVE_OXYGEN = 0.2f;

    private float timer;

    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player collidePlayer = collision.GetComponent<Player>();

        // Reset timer when player enters
        if (collidePlayer != null)
        {
            timer = 0;
            player = collidePlayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
            player = null;
    }

    private void Update()
    {
        if (player == null)
            return;

        timer += Time.deltaTime;

        if (timer >= TIME_TO_GIVE_OXYGEN)
        {
            timer = 0;
            player.GiveOxygen(oxygenGiven);
        }
    }
}
