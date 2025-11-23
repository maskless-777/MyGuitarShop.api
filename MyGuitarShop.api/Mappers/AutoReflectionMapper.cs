namespace MyGuitarShop.api.Mappers
{
    public class AutoReflectionMapper
    {
        public static TTarget? Map<TSource, TTarget>(TSource? source)
            where TTarget : new()
        {
            if (source == null) return default;
            var target = new TTarget();
            var sourceProps = typeof(TSource).GetProperties();
            var targetProps = typeof(TTarget).GetProperties();

            foreach (var sourceProp in sourceProps)
            {
                var targetProp = targetProps
                    .FirstOrDefault(x => x.Name == sourceProp.Name &&
                    x.PropertyType == sourceProp.PropertyType &&
                    x.CanWrite);
                if (targetProp == null) continue;

                var value = sourceProp.GetValue(source);
                targetProp.SetValue(target, value);
            }
            return target;
        }
    }
}
