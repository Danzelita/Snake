namespace Project.Scripts.Data
{
    [System.Serializable]
    public class DetailPositionsData
    {
        public string Id;
        public Vector2Data[] Ds;

        public DetailPositionsData(int size) => 
            Ds = new Vector2Data[size];
    }
}