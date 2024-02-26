using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class LevelCreator : MonoBehaviour
{
    [SerializeField] private Transform platformSpawnTransform;
    private Vector3 _nextPlatformSpawnPos;
    [SerializeField] private GameObject platformPartPrefab;
    [SerializeField] private float distanceBetweenPlatforms = 2.08f;
    [SerializeField] private List<GameObject> trapPrefabs;
    [SerializeField] private GameObject rotatorPrefab;
    [SerializeField] private float luckPercentage = 5f;
    
    GameObject platform = null;
    GameObject rotator = null;
    private void Start()
    {
        CreatePlatform(platformSpawnTransform.position);
        CreateRotator(platform.transform.position + new Vector3(0,8.4f,3f), false);
        CreatePlatform(_nextPlatformSpawnPos);
        CreateRotator(platform.transform.position + new Vector3(0,8.4f,3f), true);
        
    }
    private void CreatePlatform(Vector3 spawnPosition)
    {
        int platformCount = Random.Range(8, 15) + (LevelSpawner.level / 15);

        for (float i = 0; i < platformCount; i++)
        {
            Vector3 pos = new Vector3(spawnPosition.x,spawnPosition.y,spawnPosition.z + (i * distanceBetweenPlatforms));
            platform = Instantiate(platformPartPrefab, pos, Quaternion.identity);
            
            float randomNumber = Random.Range(0f, 100f);
            if ((i >= 3) && (i <= (platformCount-2)) && randomNumber <= luckPercentage)
            {
                Vector3 spawnPos = new Vector3(pos.x, pos.y + 2f, pos.z);
                int randomTrap = (int)Random.Range(0, trapPrefabs.Count);
                Instantiate(trapPrefabs[randomTrap], spawnPos, Quaternion.identity);
            }
        }
        
        
    }

    private void CreateRotator(Vector3 spawnPosition, bool isFinishRotator)
    {
        if (isFinishRotator)
            rotatorPrefab.GetComponentInChildren<Rotator>().finishRotator = true;
        else
            rotatorPrefab.GetComponentInChildren<Rotator>().finishRotator = false;
            
        
        rotator = Instantiate(rotatorPrefab, spawnPosition, Quaternion.identity);
        _nextPlatformSpawnPos = new Vector3(rotator.transform.position.x + 1.5f,
            (rotator.GetComponentInChildren<Rotator>().height - 1) * -0.5f, rotator.transform.position.z);
    }
}
