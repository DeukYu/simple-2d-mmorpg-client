using Data;
using System.Collections.Generic;
using System.Diagnostics;

public class DataManager
{
    //public static Dictionary<int, StatData> StatDict { get; private set; } = new Dictionary<int, StatData>();
    public Dictionary<int, SkillData> SkillDict { get; private set; } = new Dictionary<int, SkillData>();
    //public static Dictionary<int, ProjectileInfoData> ProjectileInfoDict { get; private set; } = new Dictionary<int, ProjectileInfoData>();
    public Dictionary<int, ItemData> ItemDataDict { get; private set; } = new Dictionary<int, ItemData>();
    public Dictionary<int, MonsterData> MonsterDataDict { get; private set; } = new Dictionary<int, MonsterData>();
    public void Init()
    {
        LoadData();
    }
    public void LoadData()
    {
        //StatDict = DataLoader.Load<StatDataLoader, int, StatData>("StatData");
        SkillDict = DataLoader.Load<SkillDataLoader, int, SkillData>("SkillData");
        //ProjectileInfoDict = DataLoader.Load<ProjectileDataLoader, int, ProjectileInfoData>("ProjectileInfoData");
        ItemDataDict = DataLoader.Load<ItemDataLoader, int, ItemData>("ItemData");
        MonsterDataDict = DataLoader.Load<MonsterDataLoader, int, MonsterData>("MonsterData");

        Debug.WriteLine("Data loaded");
    }
}