using System.Collections.Generic;

public class StackFactory
{
    private static HashSet<string> _keys;
    private static Dictionary<string, List<BlockModel>> _blocksPerGrade;

    public static List<string> CategorizeBlocksPerGrade(BlockModel[] blockModels)
    {
        if (blockModels == null || blockModels.Length == 0)
        {
            return null;
        }

        _keys = new HashSet<string>();
        _blocksPerGrade = new Dictionary<string, List<BlockModel>>();

        for (int i = 0; i < blockModels.Length; ++i)
        {
            string grade = blockModels[i].Grade;
            if (!_keys.Contains(grade))
            {
                _keys.Add(grade);
                _blocksPerGrade.Add(grade, new List<BlockModel>());
            }

            _blocksPerGrade[grade].Add(blockModels[i]);
        }

        List<string> gradesAsending = new List<string>(_keys);
        gradesAsending.Sort();
        return gradesAsending;
    }
}
