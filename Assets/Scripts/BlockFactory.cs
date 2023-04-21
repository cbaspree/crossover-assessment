public class BlockFactory
{
    public static BlockModel[] Create(string jsonContent)
    {
        BlockModel[] models = JsonDeserializer.FromJsonArray<BlockModel>(jsonContent);
        return models;
    }
}