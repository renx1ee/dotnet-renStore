using System.Text;
using System.Text.RegularExpressions;
using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Service;

internal sealed class VariantUrlService : IVariantUrlService
{
    private static readonly Dictionary<char, string> TranslitMap = new ()
    {
        {'а',"a"}, {'б',"b"}, {'в',"v"}, {'г',"g"}, {'д',"d"}, {'е',"e"}, {'ё',"yo"},
        {'ж',"zh"}, {'з',"z"}, {'и',"i"}, {'й',"y"}, {'к',"k"}, {'л',"l"}, {'м',"m"},
        {'н',"n"}, {'о',"o"}, {'п',"p"}, {'р',"r"}, {'с',"s"}, {'т',"t"}, {'у',"u"},
        {'ф',"f"}, {'х',"h"}, {'ц',"c"}, {'ч',"ch"}, {'ш',"sh"}, {'щ',"sch"},
        {'ъ',""}, {'ы',"y"}, {'ь',""}, {'э',"e"}, {'ю',"yu"}, {'я',"ya"},
    }; 
    
    public string GenerateUrl(
        string name,
        long article)
    {
        if (string.IsNullOrWhiteSpace(name))
            return article.ToString();
        
        var slug = name.ToLowerInvariant();

        slug = TranslitToLatin(slug);
        
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        const int separateLength = 1;
        int articleLength = article.ToString().Length;
        
        int allowedSlugLength = 
            CatalogConstants.ProductVariant.MaxUrlLenght 
            - separateLength
            - articleLength;
        
        if (slug.Length >= allowedSlugLength)
            slug = slug.Substring(0, allowedSlugLength).TrimEnd('-');
        
        if(string.IsNullOrEmpty(slug))
            return article.ToString();
        
        return $"{slug}-{article}";
    }

    private static string TranslitToLatin(string text)
    {
        var result = new StringBuilder(capacity: text.Length * 2);
        
        foreach (char c in text)
        {
            result.Append(TranslitMap.TryGetValue(char.ToLowerInvariant(c), out var val) 
                ? val 
                : c.ToString());
        }

        return result.ToString();
    }
}