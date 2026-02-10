using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
    public Transform parent; // GameObject 대신 Transform이 위치 제어에 더 편합니다.
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        InitPool();
    }

    private void InitPool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                objectPool.Enqueue(CreateNewObject(pool));
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // 오브젝트 생성 로직을 분리하여 재사용성 높임
    private GameObject CreateNewObject(Pool pool)
    {
        GameObject obj = Instantiate(pool.prefab, pool.parent);

        // 중요: ReturnToPool 컴포넌트에 미리 캐싱 (GetComponent 최소화)
        var returnScript = obj.GetComponent<ReturnToPool>();
        if (returnScript != null) returnScript.pool = pool;
        
        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        // 1. 큐에서 하나 꺼냄
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // 2. 만약 꺼낸 게 이미 사용 중이라면? (풀이 부족함) -> 새로 생성
        if (objectToSpawn.activeSelf)
        {
            // 꺼냈던 건 다시 넣고
            poolDictionary[tag].Enqueue(objectToSpawn);

            // 아까 찾은 Pool 데이터를 가져오기 위해 LINQ나 캐싱된 리스트 사용
            Pool pool = pools.Find(p => p.tag == tag);
            objectToSpawn = CreateNewObject(pool);
        }

        // 3. 셋팅 후 리턴
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        // 4. 다시 큐의 맨 뒤로 이동
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}