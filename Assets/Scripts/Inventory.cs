using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Image[] icons;

    public bool Contains(Sprite item) => IndexOf(item) >= 0;

    public int Count(Sprite item)
    {
        int count = 0;
        foreach (Image icon in icons)
            if (icon.enabled && icon.sprite == item) count++;
        return count;
    }

    public bool TryFind(Sprite item, out Image found)
    {
        int index = IndexOf(item);
        found = index >= 0 ? icons[index] : null;
        return index >= 0;
    }

    public bool TryAdd(Sprite item, out Image added)
    {
        foreach (Image icon in icons)
        {
            if (icon.enabled) continue;
            icon.sprite = item;
            icon.enabled = true;
            added = icon;
            return true;
        }
        added = null;
        return false;
    }

    public bool TryRemove(Sprite item)
    {
        int index = IndexOf(item);
        if (index < 0) return false;

        icons[index].enabled = false;
        return true;
    }

    public void Clear()
    {
        foreach (Image icon in icons) icon.enabled = false;
    }

    int IndexOf(Sprite item)
    {
        for (int i = 0; i < icons.Length; i++)
            if (icons[i].enabled && icons[i].sprite == item) return i;
        return -1;
    }
}
