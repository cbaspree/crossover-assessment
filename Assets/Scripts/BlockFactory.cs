public class BlockFactory
{
    public static BlockModel[] Create(string jsonContent)
    {
        BlockModel[] models = JsonArrayDeserializer.FromJson<BlockModel>(jsonContent);
        return models;
    }
}