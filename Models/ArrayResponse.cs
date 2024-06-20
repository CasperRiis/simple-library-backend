public class ArrayResponse<T>
{
    public int Count { get; set; }
    public int Next { get; set; }
    public IEnumerable<T>? Results { get; set; }
}