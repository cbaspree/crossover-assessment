using System.Collections.Generic;
using System.Linq;

public class StackFactory
{
    private static List<string> _gradesAsending;
    private static HashSet<string> _keys;
    private static Dictionary<string, List<BlockModel>> _blocksPerGrade;

    public static List<string> Grades { get => _gradesAsending; }

    public static void CategorizeBlocksPerGrade(BlockModel[] blockModels)
    {
        if (blockModels == null || blockModels.Length == 0)
        {
            return;
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

        _gradesAsending = new List<string>(_keys);
        _gradesAsending.Sort();
    }

    public static List<BlockModel> GetBlocksPerGrade(string grade)
    {
        if (_blocksPerGrade.TryGetValue(grade, out List<BlockModel> models))
        {
            models = models.OrderBy(model => model.Domain)
                .ThenBy(model => model.Cluster)
                .ThenBy(model => model.Standardid).ToList<BlockModel>();

            return models;
        }

        return null;
    }
}
