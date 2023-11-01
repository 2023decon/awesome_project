using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyWave
{
    public int type;
    public int pos;
    public float time;
    public EnemyWave(int type, int pos, float time)
    {
        this.type = type; this.pos = pos; this.time = time;
    }
}

public class WaveSystem : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<EnemyWave> wave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
