namespace Wuther.Util.PropertyMapping
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<TSource>(string fields);
    }
}