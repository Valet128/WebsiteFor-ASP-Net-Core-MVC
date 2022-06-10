namespace ShvedovaAV.Services
{
    public class CountService
    {
        public int Count { get; set; } = 0;
        public void Counting()
        {
            Count++;
        }
    }
}
