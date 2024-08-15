namespace Subtegral.SceneGraphSystem.DataContainers
{
    [System.Serializable]
    public class SceneExposedProperty
    {
        public static SceneExposedProperty CreateInstance()
        {
            return new SceneExposedProperty();
        }

        public string PropertyName = "New String";
        public string PropertyValue = "New Value";
    }
}