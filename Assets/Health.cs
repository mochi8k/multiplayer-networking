using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {
    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public void TakeDamage(int amount) {
        if (!isServer) {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0) {
            currentHealth = maxHealth;

            // サーバー上で呼び出され、クライアントで実行される
            RpcRespawn();
        }
    }

    void OnChangeHealth(int health) {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    // [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // ゼロ地点に戻る
            transform.position = Vector3.zero;
        }
    }
}
