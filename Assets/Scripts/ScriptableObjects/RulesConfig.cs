
public class RulesConfig : Singleton<RulesConfig>
{
	private readonly bool[][] Rules = { // Mapping of rules from MoveOptionType -- MoveOptionType
		new []{false, false, true, true, false},
		new []{true, false, false, false, true},
		new []{false, true, false, true, false},
		new []{false, true, false, false, true},
		new []{true, false, true, false, false},
    };

	public bool DoesSourceBeatTarget(MoveOptionType source, MoveOptionType target)
	{
		if ((int)source < 0 || (int)source > Rules.Length || (int)target < 0 || (int)target > Rules[0].Length)
		{
			UnityEngine.Debug.LogErrorFormat("Rule doesn't exist for the provided source={0}, target={1}", source, target);
			return false;
		}

		return Rules[(int)source][(int)target];
	}
}
