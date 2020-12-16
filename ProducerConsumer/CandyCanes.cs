namespace ProducerConsumer
{
    public class CandyCanes
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public CandyCanes(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}