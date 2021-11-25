using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealtManager : MonoBehaviour
{
    public static HealtManager Instance;
    public Transform HealtContainer;
    public HealtUIIcon HealtPrefabUp;
    public HealtUIIcon HealtPrefabDown;
    private List<HealtUIIcon> _currentIcons;
    private List<HealtUIIcon> _usedItems;

    private void Awake() => Instance = this;

    public void PopulateHealtUI(int startingHealt)
    {
        Vector3 currentPosition = -Main.Instance.Settings.OffSet;
        _currentIcons = new List<HealtUIIcon>();
        _usedItems = new List<HealtUIIcon>();

        for (int i = 0; i < startingHealt; i++)
        {
            HealtUIIcon tmp = Instantiate(i % 2 == 0 ? HealtPrefabUp : HealtPrefabDown, HealtContainer, true);
            tmp.transform.localPosition = currentPosition;
            tmp.transform.localScale = Vector3.one * Main.Instance.Settings.HealtIconsSize;
            tmp.transform.localEulerAngles = new Vector3(90, -90, 90);
            currentPosition -= Vector3.right * Main.Instance.Settings.SpaceBetweenShips;
            _currentIcons.Add(tmp);
        }

        _currentIcons = _currentIcons.OrderBy(item => item.transform.position.x).ToList();
    }

    public void RemoveHealt()
    {
        HealtUIIcon _currentIcon = _currentIcons.Find(item => !_usedItems.Contains(item));

        if (_currentIcon != null)
        {
            _currentIcon.SetWireframe();
            _usedItems.Add(_currentIcon);
        }
    }
}