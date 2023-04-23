using UnityEngine;

public class MasteryToMaterialConverter
{
    private static Material _glassMaterial;
    private static Material _woodMaterial;
    private static Material _stoneMaterial;

    public static void AddMaterials(Material glassMaterial,
        Material woodMaterial,
        Material stoneMaterial)
    {
        _glassMaterial = glassMaterial;
        _woodMaterial = woodMaterial;
        _stoneMaterial = stoneMaterial;
    }

    public static Material Convert(int mastery)
    {
        switch (mastery)
        {
            case 0:
                return _glassMaterial;
            case 1:
                return _woodMaterial;
            case 2:
                return _stoneMaterial;
            default:
                return null;
        }
    }
}