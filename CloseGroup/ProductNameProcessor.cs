using System;
using System.Collections.Generic;
using System.Text;

namespace CloseGroup
{
    public interface IProductNameProcessor
    {
        IList<string> Process(string productName);
    }

    public class ProductNameProcessor : IProductNameProcessor
    {
        private const char SplitChar = ' ';
        private readonly char[] trimChars = {',', '.'};

        public IList<string> Process(string productName)
        {
            var split = productName.ToLower().Split(SplitChar);
            var result = new List<string>();
            foreach (var token in split)
            {
                if (string.IsNullOrWhiteSpace(token))
                    continue;

                // режем запятые и точки, в перспективе катнуть в конфиг и пополнять
                var word = token.Trim(trimChars);

                // короткие слова плохи для категоризации, это либо предлоги, либо слишком смелые сокращения
                if (word.Length < 3)
                    continue;

                // если слово начато с цифр, то это, скорее всего, вес и он не нужен
                if (word[0] > '0' && word[0] < '9')
                    continue;

                if (result.Contains(word))
                    continue;

                result.Add(word);
            }

            return result;
        }
    }
}
