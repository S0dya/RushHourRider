using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("Settings")]
    public float startDistance;
    public float distance;

    public float[] minMaxDistanceBetweenCars;
    public float middleDistanceBetweenCars;
    public float[] minMaxCarSpeed;

    [Header("SerializeFields")]
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject[] cars;
    [SerializeField] GameObject[] objOfLvl;

    [SerializeField] Transform levelsParent;
    [SerializeField] Transform carsParent;


    //local
    float curDistance;
    
    int levelsN;
    int objOfLvlN;
    int carsN;


    protected override void Awake()
    {
        base.Awake();

        levelsN = levels.Length;
        objOfLvlN = objOfLvl.Length;
        carsN = cars.Length;
        curDistance = startDistance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateNewLevel();
        }
    }

    public void CreateNewLevel()
    {
        GameObject levelObj = Instantiate(levels[Random.Range(0, levelsN)], new Vector3(0, 0, curDistance), Quaternion.identity, levelsParent);
        Instantiate(objOfLvl[Random.Range(0, objOfLvlN)], levelObj.transform);

        var randomList = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            int randomPlace = Random.Range(0, 6);
            if (!randomList.Contains(randomPlace)) randomList.Add(randomPlace);
        }

        for (int i = 0; i < randomList.Count; i++)
        {
            float x = 0;
            switch (randomList[i])
            {
                case 0: case 3: x = Random.Range(minMaxDistanceBetweenCars[0], minMaxDistanceBetweenCars[1]); break;
                case 2: case 5: x = Random.Range(-minMaxDistanceBetweenCars[1], -minMaxDistanceBetweenCars[0]); break;
                default: x = Random.Range(-middleDistanceBetweenCars, middleDistanceBetweenCars); break;
            }

            Vector3 carPos = new Vector3(x, 0, curDistance + (randomList[i] >= 3 ? distance : Random.Range(0, distance / 2)));
            GameObject carObj = Instantiate(cars[Random.Range(0, carsN)], carPos, Quaternion.identity, carsParent);

            Enemy enemy = carObj.GetComponent<Enemy>();
            enemy.speed = Random.Range(minMaxCarSpeed[0], minMaxCarSpeed[1]);
        }

        curDistance += distance;
    }
}
