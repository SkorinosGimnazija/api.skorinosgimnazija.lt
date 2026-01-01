namespace API.Extensions;

using System.Collections.Frozen;

public static class StringExtensions
{
    public static readonly IReadOnlyDictionary<char, char> LithuanianToEnglishMap =
        new Dictionary<char, char>
        {
            { 'Ą', 'A' }, { 'ą', 'a' },
            { 'Č', 'C' }, { 'č', 'c' },
            { 'Ę', 'E' }, { 'ę', 'e' },
            { 'Ė', 'E' }, { 'ė', 'e' },
            { 'Į', 'I' }, { 'į', 'i' },
            { 'Š', 'S' }, { 'š', 's' },
            { 'Ų', 'U' }, { 'ų', 'u' },
            { 'Ū', 'U' }, { 'ū', 'u' },
            { 'Ž', 'Z' }, { 'ž', 'z' }
        }.ToFrozenDictionary();

    extension(string str)
    {
        public string NormalizeLithuanian()
        {
            return string.Create(str.Length, str, (span, source) =>
            {
                for (var i = 0; i < source.Length; i++)
                {
                    var current = source[i];
                    var mapped = LithuanianToEnglishMap.GetValueOrDefault(current, current);
                    span[i] = char.ToUpperInvariant(mapped);
                }
            });
        }

        public string TrimEnd(string suffix)
        {
            if (!str.EndsWith(suffix, StringComparison.Ordinal))
            {
                return str;
            }

            return str[..^suffix.Length];
        }
    }
}