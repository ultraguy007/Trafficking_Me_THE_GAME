﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindingQuest : Quest
{


    private Dictionary<int, List<ItemQuest>> objectiveLocation; //map that contain this quest
    // item list before random 
    private List<ItemQuest> items;
    private List<int> map;
    private int finalDestination;
    public FindingQuest(int questID, string questName, string questDescription, string questStatus, int score, int finalDestination)
        : base(questID, questName, questDescription, questStatus, score)
    {
        objectiveLocation = new Dictionary<int, List<ItemQuest>>();
        items = new List<ItemQuest>();
        map = new List<int>();

        this.finalDestination = finalDestination;
    }

    public int FinalDestination
    {
        set { this.finalDestination = FinalDestination; }
        get { return this.finalDestination; }
    }

    // 1= true;
    //2 = false;
    public void addItemQuest(ItemQuest item)
    {
        items.Add(item);
    }
    public void addMap(int[] mapID)
    {
        for (int i = 0; i < mapID.Length; i++)
        {
            map.Add(mapID[i]);
        }
    }

    public bool HaveItem(int mapID, Spawn position, out string item)
    {
        bool result = false;
        item = "";
        if (objectiveLocation.ContainsKey(mapID))
        {
            for (int i = 0; i < objectiveLocation[mapID].Count; i++)
            {
                if (objectiveLocation[mapID][i].getPositions() == position)
                {
                    result = true;
                    item = objectiveLocation[mapID][i].getPRefab();
                    break;
                }
            }

        }
        return result;
    }
    public bool IsCollect(int mapID, Spawn position)
    {
        bool result = false;
        if (objectiveLocation.ContainsKey(mapID))
        {
            for (int i = 0; i < objectiveLocation[mapID].Count; i++)
            {
                if (objectiveLocation[mapID][i].getPositions() == position)
                {
                    result = objectiveLocation[mapID][i].getIsCollected();
                    break;
                }
            }
        }
        return result;
    }



    public void randomItems()
    {
        while (items.Count > 0 && map.Count > 0)
        {
            int randomMap = Random.Range(0, map.Count);

            //Debug.Log("randomMap" + randomMap);
            if (objectiveLocation.ContainsKey(map[randomMap]))
            {

                if (objectiveLocation[map[randomMap]].Count < 2)
                {

                    int randomItem = Random.Range(0, items.Count);
                    ItemQuest temp = items[randomItem];
                    for (int i = 0; i < objectiveLocation[map[randomMap]].Count; i++)
                    {

                        if (objectiveLocation[map[randomMap]][i].getPositions() == Spawn.one)
                        {
                            temp.setPositions(Spawn.two);
                            break;
                        }
                    }
                    objectiveLocation[map[randomMap]].Add(temp);
                    map.Remove(randomMap);
                    items.Remove(items[randomItem]);
                }
            }
            else
            {

                int randomItem = Random.Range(0, items.Count);
                objectiveLocation.Add(map[randomMap], new List<ItemQuest>());
                ItemQuest temp = items[randomItem];
                objectiveLocation[map[randomMap]].Add(temp);
                items.Remove(items[randomItem]);

            }
        }
        /*foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {

            for (int i = 0; i < pair.Value.Count; i++)
            {
                Debug.Log(pair.Key + " " + pair.Value[i].getItemID() + " " + pair.Value[i].getPositions());
            }
        }*/
    }
    public void setIsCollect(int mapID, Spawn position)
    {
        if (objectiveLocation.ContainsKey(mapID))
        {
            for (int i = 0; i < objectiveLocation[mapID].Count; i++)
            {
                if (objectiveLocation[mapID][i].getPositions() == position)
                {
                    Debug.Log("successful");
                    objectiveLocation[mapID][i].setIsCollected(true);
                }

            }
        }

    }

    public bool allItemCollected()
    {
        bool result = true;

        foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {
            for (int i = 0; i < pair.Value.Count; i++)
            {
                if (pair.Value[i].getIsCollected() == false)
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }
    public void getItemQuest(out string[] items, out int[] sum, out int[] total)
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        Dictionary<string, int> dataMax = new Dictionary<string, int>();
        foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {
            List<ItemQuest> temp = pair.Value;
            for (int i = 0; i < temp.Count; i++)
            {
                if (data.ContainsKey(temp[i].getPRefab()))
                {
                    if (temp[i].getIsCollected())
                    {
                        data[temp[i].getPRefab()] += 1;
                    }
                    dataMax[temp[i].getPRefab()] += 1;
                }
                else
                {
                    if (temp[i].getIsCollected())
                    {
                        data.Add(temp[i].getPRefab(), 1);
                    }
                    else
                    {
                        data.Add(temp[i].getPRefab(), 0);
                    }
                    dataMax.Add(temp[i].getPRefab(), 1);

                }
            }
        }

        List<string> temp2 = new List<string>(data.Keys);
        items = temp2.ToArray();
        List<int> temp3 = new List<int>(data.Values);
        sum = temp3.ToArray();
        List<int> temp4 = new List<int>(dataMax.Values);
        total = temp4.ToArray();
    }
    public bool checkCollectwithPrefab(string name)
    {
        bool result = false;
        foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {
            List<ItemQuest> temp = pair.Value;
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].getPRefab() == name && temp[i].getIsCollected())
                {
                    result = true;
                }
                else if (temp[i].getPRefab() == name && !temp[i].getIsCollected())
                {
                    result = false;
                }
            }
        }
        return result;
    }
    public int[] getALLItemList()
    {
        List<int> result = new List<int>();
        foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {
            List<ItemQuest> key = pair.Value;
            for (int i = 0; i < key.Count; i++)
            {
                if (key[i].getIsCollected() == true)
                {
                    result.Add(key[i].getItemID());
                }

            }
        }
        return result.ToArray();
    }
    public void setAllCurrentItemisCollect(int itemID)
    {
        foreach (KeyValuePair<int, List<ItemQuest>> pair in objectiveLocation)
        {
            List<ItemQuest> key = pair.Value;
            for (int i = 0; i < key.Count; i++)
            {
                if (key[i].getItemID() == itemID)
                {
                    objectiveLocation[pair.Key][i].setIsCollected(true);
                }

            }
        }
    }
}
