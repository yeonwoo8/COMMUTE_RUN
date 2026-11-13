using UnityEngine.Pool;

public static class PoolPrewarm
{
    // Creating pooled objects up front keeps Instantiate out of the commute itself.
    public static void Prewarm<T>(this ObjectPool<T> pool, int count) where T : class
    {
        var items = new T[count];
        for (int i = 0; i < count; i++) items[i] = pool.Get();
        foreach (T item in items) pool.Release(item);
    }
}
