using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level")]
    public LevelData[] levelDatas;
    public bool enableTestLevel;
    public LevelData testLevel;

    public bool IsInitialized { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    public Level CurrentLevelInstance { get; private set; }
    public int CurrentLevelUniqueID
    {
        get { return CurrentLevelData?.ID ?? -1; }
    }

    public event Action<int> AddingAmount;
    public void Initialize()
    {

        IsInitialized = true;
    }

    public void CreateLevel(int linearLevelIndex)
    {
        UnloadLevel();

        CurrentLevelData = GetCurrentLevelData(linearLevelIndex);

        var levelObject = GameObject.Instantiate(CurrentLevelData.LevelPrefab.gameObject, this.transform);
        levelObject.transform.localPosition = Vector3.zero;
        levelObject.transform.localRotation = Quaternion.identity;
        levelObject.transform.localScale = Vector3.one;

        var level = levelObject.GetComponent<Level>();
        level.Initialize(CurrentLevelData.StackCount);
        level.OnAddingAmount += Level_AddingAmount;
        CurrentLevelInstance = level;
    }

    private void Level_AddingAmount(int addingAmount)
    {
        AddingAmount?.Invoke(addingAmount);
    }

    public void UnloadLevel()
    {
        if (CurrentLevelInstance == null)
            return;

        GameObject.Destroy(CurrentLevelInstance.gameObject);

        CurrentLevelData = null;
        CurrentLevelInstance = null;
    }

    private LevelData GetCurrentLevelData(int linearLevelIndex)
    {
        if (enableTestLevel)
        {
            return testLevel;
        }

        int levelIndex = linearLevelIndex;

        if (levelIndex >= levelDatas.Length)
        {
            int discardedCount = Mathf.Min(levelDatas.Length - 1, GameConfigs.Instance.LevelSkipCountAtRepeat);
            levelIndex = (linearLevelIndex - discardedCount) % (levelDatas.Length - discardedCount) + discardedCount;
        }

        return levelDatas[levelIndex];
    }
}
