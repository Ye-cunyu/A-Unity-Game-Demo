using UnityEngine;
public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Object_Checkpoint[] allCheckpoints;
    private Animator anim;
    private float saveCooldown = 10f;
    private float saveCooldownTimer;
    void OnValidate()
    {
        saveCooldownTimer = saveCooldown;
    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        allCheckpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
        
    }
    void Update()
    {
        saveCooldownTimer -= Time.deltaTime;
    }

    public void ActivateCheckpoint(bool activate)
    {
        anim.SetBool("isActive", activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (saveCooldownTimer > 0)
            return;
        if (!collision.CompareTag("Player"))
            return;
        if (SaveManager.instance == null || SaveManager.instance.GetGameData() == null)
        {
            return;
        }
        saveCooldownTimer = saveCooldown;
        foreach (var point in allCheckpoints)
            point.ActivateCheckpoint(false);

        SaveManager.instance.GetGameData().savedCheckpoint = transform.position;
        ActivateCheckpoint(true);
        SaveManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckpoint == transform.position;
        ActivateCheckpoint(active);

        if (active)
        {
            Backtocp();
        }
            
    }

    public void SaveData(ref GameData data)
    {
        data.savedCheckpoint = transform.position;
    }
    public void Backtocp()
    {
        Player.instance.TeleportPlayer(transform.position);
    }
    void OnEnable()
    {
        Player.instance.PlayerIsDead += Backtocp;
    }
    void OnDisable()
    {
        Player.instance.PlayerIsDead -= Backtocp;
    }
}