using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

public class FetchDataTest
{
    [Test]
    public async Task CreateStacks()
    {
        // request the data from the api endpoint
        const string url = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        RequestResult result = await HttpRequester.Get(url);

        Assert.IsNotNull(result);
        Assert.IsNull(result.Error);
        Assert.IsNotNull(result.Content);
        Assert.IsNotEmpty(result.Content);

        // create the blocks with all the information
        string content = result.Content;
        BlockModel[] blockModels = BlockFactory.Create(content);

        Assert.IsNotNull(blockModels);
        Assert.AreNotEqual(0, blockModels.Length);

        // create the stacks per grade
        List<string> grades = StackFactory.CategorizeBlocksPerGrade(blockModels);

        Assert.IsNotNull(grades);
        Assert.AreNotEqual(0, grades.Count);
        Assert.AreEqual(4, grades.Count);
    }
}
