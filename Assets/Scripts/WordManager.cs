using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class WordManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;
    public Transform player;
    public Button[] optionButtons;

    private List<WordData> wordPool;
    private List<Zombie> zombies = new List<Zombie>();
    private Zombie activeZombie;

    void Start()
    {
        LoadWordData();
        InvokeRepeating(nameof(SpawnZombie), 1f, 2f); // spawn every 2 sec
    }

    void LoadWordData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("wordData");
        WordData[] data = JsonHelper.FromJson<WordData>(FixJson(jsonText.text));
        wordPool = new List<WordData>(data);
    }

    void SpawnZombie()
    {
        if (wordPool.Count == 0) return;

        WordData word = wordPool[Random.Range(0, wordPool.Count)];
        GameObject zObj = Instantiate(zombiePrefab, GetRandomSpawnPosition(), Quaternion.identity);
        Zombie zombie = zObj.GetComponent<Zombie>();
        zombie.Setup(word, player);

        zombies.Add(zombie);

        UpdateActiveZombie();
    }

    void UpdateActiveZombie()
    {
        // Remove any destroyed zombies from the list
        zombies.RemoveAll(z => z == null);
        
        if (zombies.Count == 0) return;

        Zombie closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Zombie z in zombies)
        {
            float dist = Vector3.Distance(z.transform.position, player.position);
            if (dist < closestDist)
            {
                closest = z;
                closestDist = dist;
            }
        }

        if (closest != activeZombie)
        {
            if (activeZombie != null) activeZombie.Deactivate();

            activeZombie = closest;
            activeZombie.Activate();

            SetOptions(activeZombie.myWordData);
        }
    }


    void SetOptions(WordData data)
    {
        List<string> options = new List<string> {
            data.option1, data.option2, data.option3, data.option4
        };

        options = Shuffle(options);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            string choice = options[i];
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choice;
            optionButtons[i].onClick.RemoveAllListeners();

            optionButtons[i].onClick.AddListener(() =>
            {
                if (activeZombie != null && choice == activeZombie.myWordData.correctAnswer)
                {
                    zombies.Remove(activeZombie);
                    activeZombie.DestroyZombie();
                    activeZombie = null;

                    UpdateActiveZombie(); // select next closest
                }
            });
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        return spawnPoint.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(1f, 3f));
    }

    List<string> Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
        return list;
    }

    string FixJson(string value)
    {
        return "{\"items\":" + value + "}";
    }
}
