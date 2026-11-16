using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private event Action UIChange;

    public void OnUIChange()
    {
        UIChange?.Invoke();
    }
}
