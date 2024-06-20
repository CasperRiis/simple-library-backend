public class ArrayResponse<T>
{
    public int Count { get; set; }
    public IEnumerable<T>? Results { get; set; }
}