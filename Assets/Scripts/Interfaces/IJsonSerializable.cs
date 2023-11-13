using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJsonSerializable
{
    void SaveData();
    void LoadData();
    void DeleteData();
}
