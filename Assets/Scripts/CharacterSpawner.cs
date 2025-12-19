using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject warriorPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapGenerator.OnMapGenerated += SpawnInitialUnits;
    }

    void SpawnInitialUnits()
    {
        Vector3 pos = mapGenerator.GetRandomGrassWorldPosition();
        GameObject warrior = Instantiate(
            warriorPrefab,
            pos,
            Quaternion.identity
        );

        // THIS is who calls Init
        ClickToRun clickToRun = warrior.GetComponent<ClickToRun>();
        clickToRun.Init(mapGenerator.grassTilemap);
    }
}
