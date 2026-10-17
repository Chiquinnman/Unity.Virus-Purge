using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public float maxHealth = 100;
    public float restartDelay = 3;

    float health;
    public bool IsDead = false;


    void Awake()
    {
        Instance = this;
        health = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            IsDead = true;

            Controller.Instance.LockControl = true;
            foreach (Weapon w in GetComponentsInChildren<Weapon>())
                w.triggerDown = false;

            Invoke("Restart", restartDelay);
        }
    }


    void Restart()
    {
#if UNITY_EDITOR
        EditorSceneManager.LoadSceneAsyncInPlayMode(SceneManager.GetActiveScene().path,
            new LoadSceneParameters(LoadSceneMode.Single));
#else
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#endif
    }


    void OnGUI()
    {
        GUI.skin.label.fontSize = 24;
        GUI.Label(new Rect(20, Screen.height - 50, 300, 40), "HP : " + health);
    }
}