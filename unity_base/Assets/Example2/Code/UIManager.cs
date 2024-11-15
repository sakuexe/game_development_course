using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // the UIManager is a singleton, since we don't want multiple of them
    public static UIManager Instance { get; private set; }
    private UIDocument document;
    private VisualElement characterIcon;

    [SerializeField]
    private float minScale = 0.5f;
    [SerializeField]
    private float maxScale = 1.2f;
    [SerializeField]
    private float animationSpeed = 1f;

    private int enemiesChasing = 0;

    void Awake()
    {
        // making the singleton work, this way we can call UIManager.Instance.Function from anywhere
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        // fetch the ui elements
        document = GetComponent<UIDocument>();
        characterIcon = document.rootVisualElement.Q("CharacterIcon") as VisualElement;
    }

    void Update()
    {
        if (enemiesChasing <= 0) return;

        // scale the icon to large and small while getting chased
        float pingPongValue = Mathf.PingPong(Time.time * animationSpeed, 1f);
        float currentScale = Mathf.Lerp(minScale, maxScale, pingPongValue);
        characterIcon.style.scale = new StyleScale(new Scale(new Vector2(currentScale, currentScale)));
    }

    public void AddEnemyListeners(EnemyLogic enemy)
    {
        enemy.onPlayerLost += OnPlayerLost;
        enemy.onPlayerDetected += OnPlayerDetected;
        // to dereference, you would do this:
        /*enemy.onPlayerLost -= OnPlayerLost;*/
        /*enemy.onPlayerDetected -= OnPlayerDetected;*/
    }

    private void OnPlayerLost()
    {
        Debug.Log("UI: player was lost");
        enemiesChasing -= 1;
        if (enemiesChasing > 0) return;

        characterIcon.RemoveFromClassList("in-danger");
    }

    private void OnPlayerDetected()
    {
        Debug.Log("UI: player detected");
        characterIcon.AddToClassList("in-danger");
        enemiesChasing += 1;
    }
}
