/*
 * Creator
 * 20150925 15:20:56
 * Loccy
 */

public class Creator
{
	public T Create<T>() where T : new()
	{
		return new T();
	}
}
