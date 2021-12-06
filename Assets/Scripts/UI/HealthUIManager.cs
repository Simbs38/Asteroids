using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform HealtContainer;
    [SerializeField]
    private HealthUIIcon HealtPrefabUp;
    [SerializeField]
    private HealthUIIcon HealtPrefabDown;
    [SerializeField]
    private UISettings Settings;
    private List<HealthUIIcon> _currentIcons;
    private List<HealthUIIcon> _usedItems;

    public void PopulateHealtUI(int startingHealt)
    {
        Vector3 currentPosition = -Settings.OffSet;
        _currentIcons = new List<HealthUIIcon>();
        _usedItems = new List<HealthUIIcon>();

        for (int i = 0; i < startingHealt; i++)
        {
            HealthUIIcon tmp = Instantiate(i % 2 == 0 ? HealtPrefabUp : HealtPrefabDown, HealtContainer, true);
            tmp.transform.localPosition = currentPosition;
            tmp.transform.localScale = Vector3.one * Settings.HealtIconsSize;
            tmp.transform.localEulerAngles = new Vector3(90, -90, 90);
            currentPosition -= Vector3.right * Settings.SpaceBetweenShips;
            _currentIcons.Add(tmp);
        }

        _currentIcons = _currentIcons.OrderBy(item => item.transform.position.x).ToList();
    }

    public void RemoveHealt()
    {
        HealthUIIcon _currentIcon = _currentIcons.Find(item => !_usedItems.Contains(item));

        if (_currentIcon != null)
        {
            _currentIcon.SetWireframe();
            _usedItems.Add(_currentIcon);
        }
    }
}