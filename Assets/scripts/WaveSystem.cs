using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyWave
{
    public int type;
    public float posX;
    public float posY;
    public float time;
    public int repeat;
    public EnemyWave(int type, float posX, float posY, float time, int repeat)
    {
        this.type = type; this.posX = posX; this.posY = posY; this.time = time; this.repeat = repeat;
    }
}

public class WaveSystem : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<EnemyWave> wave;
    private float timer = 0;
    private int repeat = 0;

    // Start is called before the first frame update
    void Start()
    {
        wave.Add(new EnemyWave(0, 3, 0, 1, 0));
        wave.Add(new EnemyWave(0, -1, 0, 8, 3));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (wave.Count > 0) {
            if (timer >= wave[0].time)
            {
                timer = 0;
                Spawn(wave[0]);
                repeat++;
                if (repeat > wave[0].repeat) {
                    wave.Remove(wave[0]);
                    repeat = 0;
                }
            }
        }
    }

    private void Spawn(EnemyWave ew)
    {
        transform.position = new Vector2(ew.posX, ew.posY);
        Instantiate(enemyPrefabs[ew.type]);
    }
}
