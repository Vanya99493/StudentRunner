using System;
using UnityEngine;

public class RecordsConverter
{
    private RecordItemsContainer _recordsContainer;
    private string _json;

    public string PathToJson { get { return _json; } }

    public RecordsConverter()
    {
        _json = PlayerPrefs.GetString("RecordsData");
        if(_json == "")
        {
            _recordsContainer = new RecordItemsContainer();
            _recordsContainer.Records = new RecordItem[0];
            SaveData();
        }
        else
        {
            _recordsContainer = JsonUtility.FromJson<RecordItemsContainer>(_json);
        }
    }

    public void AddRecordItem(string playerName, int playerScore)
    {
        bool isInRecords = false;

        foreach (RecordItem item in _recordsContainer.Records)
        {
            if(item.PlayerName == playerName)
            {
                if(item.PlayerScore < playerScore)
                {
                    item.PlayerScore = playerScore;

                }
                isInRecords = true;
                break;
            }
        }

        if (!isInRecords)
        {
            RecordItem[] oldRecords = _recordsContainer.Records;
            _recordsContainer.Records = new RecordItem[oldRecords.Length + 1];
            for (int i = 0; i < oldRecords.Length; i++)
            {
                _recordsContainer.Records[i] = oldRecords[i];
            }
            _recordsContainer.Records[_recordsContainer.Records.Length - 1] = new RecordItem(playerName, playerScore);
        }
        Array.Sort(_recordsContainer.Records);
        SaveData();
    }

    private void SaveData()
    {
        _json = JsonUtility.ToJson(_recordsContainer);
        PlayerPrefs.SetString("RecordsData", _json);
    }
}