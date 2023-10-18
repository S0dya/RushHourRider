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
    [SerializeField] GameObject[] boosts;

    [SerializeField] Transform levelsParent;
    [SerializeField] Transform carsParent;


    //local
    float curDistance;
    float halfOfLvlDistance;
    
    int levelsN;
    int objOfLvlN;
    int boostsN;
    int carsN;


    protected override void Awake()
    {
        base.Awake();

        
        
    }

    void Start()
    {
        levelsN = levels.Length;
        objOfLvlN = objOfLvl.Length;
        boostsN = boosts.Length;
        carsN = cars.Length;
        curDistance = startDistance;
        halfOfLvlDistance = distance / 2;
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
        //create level 
        GameObject levelObj = Instantiate(levels[Random.Range(0, levelsN)], new Vector3(0, 0, curDistance), Quaternion.identity, levelsParent);
        //get it's transform
        var levelTransform = levelObj.transform;

        //create objects 
        Instantiate(objOfLvl[Random.Range(0, objOfLvlN)], levelTransform);

        //create boosts
        if (Random.Range(0, 15) == 1)
        {
            Vector3 boostPos = new Vector3(Random.Range(-minMaxDistanceBetweenCars[0], minMaxDistanceBetweenCars[1]), 1.5f, Random.Range(-halfOfLvlDistance, halfOfLvlDistance) + curDistance);
            Instantiate(boosts[Random.Range(0, boostsN)], boostPos, Quaternion.identity, levelTransform);
        }

        //check how many cars will be on a level
        var randomList = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            int randomPlace = Random.Range(0, 6);
            if (!randomList.Contains(randomPlace)) randomList.Add(randomPlace);
        }

        //create cars 
        for (int i = 0; i < randomList.Count; i++)
        {
            //set random x
            float x = 0;
            switch (randomList[i])
            {
                case 0: case 3: x = Random.Range(minMaxDistanceBetweenCars[0], minMaxDistanceBetweenCars[1]); break;
                case 2: case 5: x = Random.Range(-minMaxDistanceBetweenCars[1], -minMaxDistanceBetweenCars[0]); break;
                default: x = Random.Range(-middleDistanceBetweenCars, middleDistanceBetweenCars); break;
            }

            //create car
            Vector3 carPos = new Vector3(x, 0, curDistance + (randomList[i] >= 3 ? halfOfLvlDistance : Random.Range(-halfOfLvlDistance, 0)));
            GameObject carObj = Instantiate(cars[Random.Range(0, carsN)], carPos, Quaternion.identity, carsParent);

            //set random speed
            Enemy enemy = carObj.GetComponent<Enemy>();
            enemy.speed = Random.Range(minMaxCarSpeed[0], minMaxCarSpeed[1]);
        }

        //update distance
        curDistance += distance;
    }
}
