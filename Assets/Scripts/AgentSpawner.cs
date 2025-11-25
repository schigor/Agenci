using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject agentPrefab; // Prefab twojego Enemy/Agenta
    [SerializeField] private Transform spawnArea; // Parent object dla spawnu
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(50f, 1f, 50f); // Rozmiar obszaru spawnu
    [SerializeField] private int defaultAgentCount = 5;
    
    // UI elementy
    [SerializeField] private TMP_InputField agentCountInput;
    [SerializeField] private Button spawnButton;
    [SerializeField] private TextMeshProUGUI statusText;
    
    private int spawnedAgents = 0;

    private void Start()
    {
        if (spawnButton != null)
            spawnButton.onClick.AddListener(SpawnAgents);
        
        if (agentCountInput != null)
            agentCountInput.text = defaultAgentCount.ToString();
    }

    public void SpawnAgents()
    {
        // Usuń poprzednio spawnutych agentów
        DestroyPreviousAgents();
        
        // Odczytaj ilość agentów z InputField
        if (!int.TryParse(agentCountInput.text, out int agentCount))
        {
            Debug.LogError("Nieprawidłowa liczba agentów!");
            UpdateStatus("Błąd: Wpisz liczbę", Color.red);
            return;
        }

        if (agentCount <= 0)
        {
            Debug.LogError("Liczba agentów musi być większa niż 0!");
            UpdateStatus("Błąd: Liczba musi być > 0", Color.red);
            return;
        }

        spawnedAgents = 0;

        for (int i = 0; i < agentCount; i++)
        {
            Vector3 randomPos = GetRandomSpawnPosition();
            
            // Sprawdź czy pozycja jest na NavMeshu
            if (IsPositionOnNavMesh(randomPos))
            {
                GameObject newAgent = Instantiate(agentPrefab, randomPos, Quaternion.identity, spawnArea);
                newAgent.name = "Agent_" + (i + 1);
                spawnedAgents++;
            }
            else
            {
                i--; // Spróbuj ponownie jeśli pozycja nie jest na NavMeshu
            }
        }

        Debug.Log($"Spawned {spawnedAgents} agents");
        UpdateStatus($"Spawned {spawnedAgents} agents", Color.green);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);
        
        Vector3 randomPos = new Vector3(randomX, spawnAreaSize.y, randomZ);
        
        if (spawnArea != null)
            randomPos += spawnArea.position;
        
        return randomPos;
    }

    private bool IsPositionOnNavMesh(Vector3 position)
    {
        UnityEngine.AI.NavMeshHit hit;
        return UnityEngine.AI.NavMesh.SamplePosition(position, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas);
    }

    private void DestroyPreviousAgents()
    {
        if (spawnArea == null) return;
        
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
}