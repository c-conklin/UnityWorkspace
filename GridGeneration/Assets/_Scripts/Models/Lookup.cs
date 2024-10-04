namespace _Scripts.Models
{
    public interface ILookup<T>
    {
        public T value { get; set; }
        public string key { get; set; }
    }
    
    public class Lookup<T> : ILookup<T>
    {
        public T value { get; set; }
        public string key { get; set; }

        public Lookup(string key, T value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
}
