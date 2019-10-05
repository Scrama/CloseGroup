using System.Collections.Generic;
using System.Linq;

namespace CloseGroup
{
    public interface IKeyWordAnalyzer
    {
        void Analyze(bool forced);
    }

    public class KeyWordAnalyzer : IKeyWordAnalyzer
    {
        private readonly IRepo repo;
        private readonly IProductNameProcessor productNameProcessor;
        private readonly double wordSimilarityRatio;

        public KeyWordAnalyzer(IRepo repo, IProductNameProcessor productNameProcessor, ISettings settings)
        {
            this.repo = repo;
            this.productNameProcessor = productNameProcessor;

            wordSimilarityRatio = settings.Get<double>("WordSimilarityRatio");
        }

        public void Analyze(bool forced)
        {
            var analyzed = false;
            foreach (var group in repo.List())
            {
                if (!forced && group.KeyWords != null && group.KeyWords.Any())
                    continue;

                AnalyzeGroup(group);
                analyzed = true;
            }

            if (analyzed)
                repo.WriteDown();
        }

        private void AnalyzeGroup(Group group)
        {
            var map = new Dictionary<string, int>();
            foreach (var product in group.Products)
                AnalyzeProduct(map, product);

            group.KeyWords = map
                .Where(x =>x.Value > 1)
                .Select(x => new KeyWord {Token = x.Key, Weight = x.Value})
                .ToList();
        }

        private void AnalyzeProduct(Dictionary<string, int> map, string product)
        {
            var words = productNameProcessor.Process(product);
            foreach (var word in words)
            {
                if (map.TryGetValue(word, out var cnt))
                {
                    map[word] = cnt + 1;
                    continue;
                }

                var foundSimilar = false;
                foreach (var key in map.Keys.ToArray())
                {
                    if (!WordUtils.IsWordsSimilar(key, word, wordSimilarityRatio))
                        continue;
                    map[key] = map[key] + 1;
                    foundSimilar = true;
                    break;
                }
                if (foundSimilar)
                    continue;

                map[word] = 1;
            }
        }
    }
}