namespace API.Extensions;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class NpgsqlExtensions
{
    private static readonly string LtTranslateFrom;
    private static readonly string LtTranslateTo;

    static NpgsqlExtensions()
    {
        var upperPairs = StringExtensions.LithuanianToEnglishMap
            .Where(x => char.IsUpper(x.Key))
            .ToList();

        LtTranslateFrom = new(upperPairs.Select(p => p.Key).ToArray());
        LtTranslateTo = new(upperPairs.Select(p => p.Value).ToArray());
    }

    extension<TEntity>(IndexBuilder<TEntity> builder)
    {
        public void GinTrigram()
        {
            builder.HasMethod("GIN").HasOperators("gin_trgm_ops");
        }
    }

    extension<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        public void GenerateNormalized(
            Expression<Func<TEntity, object>> sourceProperty,
            Expression<Func<TEntity, object>> targetProperty)
        {
            var sourceColumnName = ((MemberExpression) sourceProperty.Body).Member.Name;

            builder.Property(targetProperty)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql(
                    $"translate(upper(\"{sourceColumnName}\"), '{LtTranslateFrom}', '{LtTranslateTo}')",
                    true)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}