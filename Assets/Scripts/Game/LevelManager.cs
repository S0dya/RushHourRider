using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("Settings")]
    public float startDistance;
    public float distance;
    public float distanceBetweenCars;

    public float minCarSpeed;
    public float maxCarSpeed;

    [Header("SerializeFields")]
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject[] cars;

    [SerializeField] Transform levelsParent;
    [SerializeField] Transform carsParent;


    //local
    float curDistance;

    int levelsN;
    int carsN;


    protected override void Awake()
    {
        base.Awake();

        levelsN = levels.Length;
        carsN = cars.Length;
        curDistance = startDistance;
    }

    public void CreateNewLevel()
    {
        Instantiate(levels[Random.Range(0, levelsN)], new Vector3(0, 0, curDistance), Quaternion.identity, levelsParent);

        var randomList = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomPlace = Random.Range(0, 3);
            if (!randomList.Contains(randomPlace)) randomList.Add(randomPlace);
        }

        for (int i = 0; i < randomList.Count; i++)
        {
            float x = 0;
            switch (randomList[i])
            {
                case 0: x = -distanceBetweenCars; break;
                case 2: x = distanceBetweenCars; break;
                default: x = 0; break;
            }

            GameObject carObj = Instantiate(cars[Random.Range(0, carsN)], new Vector3(x, 0, curDistance + distance), Quaternion.identity, carsParent);
            Enemy enemy = carObj.GetComponent<Enemy>();
            enemy.speed = Random.Range(minCarSpeed, maxCarSpeed);
        }

        curDistance += distance;
    }
}
