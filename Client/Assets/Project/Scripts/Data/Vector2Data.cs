namespace Project.Scripts.Data
{
    [System.Serializable]
    public class Vector2Data
    {
        public float X;
        public float Z;

        public Vector2Data(float x, float z)
        {
            this.X = x;
            this.Z = z;
        }
    }
}