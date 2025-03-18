using System.Collections.Generic;
using UnityEngine;

public class Stand : StandBase
{
    [SerializeField] private List<ItemBase> _items = new();

    private void Awake()
    {
        if(_items.Count > 0)
        {
            CalculateOrder();
        }
    }

    public override void AddItem(ItemBase item)
    {
        _items.Add(item);
        item.transform.parent = transform;
        CalculateOrder();
    }

    public override void RemoveItem(ItemBase item)
    {
        if (_items.Contains(item))
        {
            item.transform.parent = null;
            _items.Remove(item);
        }
    }

    private void CalculateOrder()
    {
        _items.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetLayer(2 + i);
        }
    }
}
