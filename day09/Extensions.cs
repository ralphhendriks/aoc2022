
namespace Day09
{
    public static class Extensions
    {
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source, 
            TAccumulate seed, 
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            var current = seed;
            foreach (var item in source)
            {
                current = accumulator(current, item);
                yield return current;
            }
        }
    }
}