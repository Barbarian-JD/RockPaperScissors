using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsDatabase", menuName = "ScriptableObjects/Prefabs Database")]
public class PrefabsDatabase : SingletonScriptableObject<PrefabsDatabase>
{
    [SerializeField] private MoveOptionView MoveOptionPrefab;


    public MoveOptionView InstantiateMoveOptionView(MoveOptionType moveOptionType, Transform parentTransform = null)
    {
        MoveOptionView prefab = MoveOptionPrefab;

        if (prefab == null)
        {
            Debug.LogError("Couldn't instantiate the MoveOptionView, please attach prefab!");
            return null;
        }
        
        return Instantiate(prefab, parentTransform);
    }
}
