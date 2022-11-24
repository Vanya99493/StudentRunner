using System.Collections.Generic;
using UnityEngine;

public class RecordsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private RecordItemView _recordItemPrefab;
    
    private List<RecordItemView> _recordsItems;

    public void UpdateRecordsView(string _path)
    {
        if(_recordsItems == null)
        {
            _recordsItems = new List<RecordItemView>();
        }

        RecordItemsContainer recordsContainer = JsonUtility.FromJson<RecordItemsContainer>(_path);
        foreach (RecordItemView item in _recordsItems)
        {
            Destroy(item.gameObject);
        }
        _recordsItems.Clear();
        for (int i = 0; i < recordsContainer.Records.Length; i++)
        {
            RecordItemView newItemView = Instantiate(_recordItemPrefab, _container.transform);
            newItemView.SetValues(recordsContainer.Records[i].PlayerName, recordsContainer.Records[i].PlayerScore);
            _recordsItems.Add(newItemView);
        }
    }
}