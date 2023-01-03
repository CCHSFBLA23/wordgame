using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSaveDataButton : MonoBehaviour
{
    public OptionsHandler optionsHandler;

    public void ClearSaveData()
    {
        // Delete Save Data
        SaveSystem.DeleteAllSaveData();
        // Reset Volume
        optionsHandler.SetOptions();
    }
}
