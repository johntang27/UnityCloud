using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public enum TimeMode
{
    TurnBased,
    RealTime
}

public enum RetirementDream
{
    NaturalSolitude = 20000,
    CountrysideComfort = 60000,
    CityLuxury = 100000,
    NonStopTravel = 140000,
    IslandParadise = 180000
}

[Serializable]
public class LevelData
{
    public int levelID;
    public string levelName;
    public TimeMode timeMode;
    public int startingAge;
    public int retirementAge;
    public int baseSalary;
    public int studentLoan;
    public int employerMatch;
    public MatchValue[] tiles;
    public RetirementDream retirementDream;
    public int minimumScore;
    public string levelDescription;
    public bool isTutorial;
    public string tutorialDescription;
    public int previousLevelID;
    public BadgeMastery unlockingMastery = BadgeMastery.None;
}

[XmlRoot("LevelDataCollection")]
public class LevelDataCollection {

    [XmlArray("LevelData"), XmlArrayItem("Level")]
    public LevelData[] data;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(LevelDataCollection));
        using (var stream = new FileStream(path, FileMode.Create))
            serializer.Serialize(stream, this);
    }

    public static LevelDataCollection Load(string path)
    {
        var serializer = new XmlSerializer(typeof(LevelDataCollection));
        using (var stream = new FileStream(path, FileMode.Open))
            return serializer.Deserialize(stream) as LevelDataCollection;
    }

    public static LevelDataCollection LoadFromResources(string filename)
    {
        TextAsset text = Resources.Load(filename) as TextAsset;
        var serializer = new XmlSerializer(typeof(LevelDataCollection));

        using (var reader = new StringReader(text.text))
            return serializer.Deserialize(reader) as LevelDataCollection;
    }
}
