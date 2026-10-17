using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 3;
    public float damage = 10;
    public float attackDelay = 1.5f;

    float timer = 0;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded -= Setup;
        SceneManager.sceneLoaded += Setup;
    }


    static void Setup(Scene scene, LoadSceneMode mode)
    {
        if (Controller.Instance == null) return;

        if (Controller.Instance.GetComponent<PlayerHealth>() == null)
            Controller.Instance.gameObject.AddComponent<PlayerHealth>();

        int count = 0;
        Target[] targets = FindObjectsByType<Target>(FindObjectsInactive.Include);

        foreach (Target t in targets)
        {
            if (t.pointValue <= 0) continue;

            if (t.GetComponent<EnemyAttack>() == null)
                t.gameObject.AddComponent<EnemyAttack>();

            count++;
        }

        Debug.Log("EnemyAttack ready: " + count);
    }


    void Update()
    {
        PlayerHealth player = PlayerHealth.Instance;

        if (player == null) return;
        if (player.IsDead) return;

        timer -= Time.deltaTime;

        Vector3 germPos = transform.position;
        Vector3 playerPos = player.transform.position;
        germPos.y = 0;
        playerPos.y = 0;

        float distance = Vector3.Distance(germPos, playerPos);

        if (distance <= attackRange && timer <= 0)
        {
            player.TakeDamage(damage);
            timer = attackDelay;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}