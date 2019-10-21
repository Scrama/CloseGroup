using NUnit.Framework;
using System;

namespace CloseGroup.Tests
{
    [TestFixture]
    public class CloseGroupTests
    {
        private readonly ISettings settings = new TestSettings();

        [TestCaseSource(nameof(InputProductNames))]
        public void ProductNameProcessorTest((string input, string output, int cnt) caseData)
        {
            var pnp = new ProductNameProcessor();
            var list = pnp.Process(caseData.input);
            var expected = caseData.output.Split(" ");
            Assert.AreEqual(caseData.cnt, list.Count);
            for (var i = 0; i < 3; ++i)
            {
                Assert.AreEqual(expected[i], list[i]);
            }
        }

        [TestCaseSource(nameof(SimilarWords))]
        public void SimilarWordTest((string first, string second, bool isSimilar) caseData)
        {
            var result = WordUtils.IsWordsSimilar(caseData.first, caseData.second, 0.3);
            Assert.AreEqual(caseData.isSimilar, result);
        }

        [Test]
        public void KeyWordAnlyzerTest()
        {
            var repo = new TestRepo();
            var pnp = new ProductNameProcessor();
            var kwa = new KeyWordAnalyzer(repo, pnp, settings);

            kwa.Analyze(true);

            Assert.AreEqual(2, repo.List()[0].KeyWords.Count);
            Assert.AreEqual(2, repo.List()[1].KeyWords.Count);
        }

        [Test]
        public void DirectProductTest()
        {
            var repo = new TestRepo();
            var pnp = new ProductNameProcessor();
            var kwa = new KeyWordAnalyzer(repo, pnp, settings);
            var svc = new CloseGroupService(settings, repo, pnp, kwa);

            var result = svc.CloseGroupFor("шоколадный батончик сникерс");
            Assert.AreEqual("Шоколадные батончики", result);
        }

        [Test]
        public void CloseProductTest()
        {
            var repo = new TestRepo();
            var pnp = new ProductNameProcessor();
            var kwa = new KeyWordAnalyzer(repo, pnp, settings);
            var svc = new CloseGroupService(settings, repo, pnp, kwa);

            var result = svc.CloseGroupFor("шоколадный батончик сказка");
            Assert.AreEqual("Шоколадные батончики", result);
        }

        [Test]
        public void CloseProductTestFail()
        {
            var repo = new TestRepo();
            var pnp = new ProductNameProcessor();
            var kwa = new KeyWordAnalyzer(repo, pnp, settings);
            var svc = new CloseGroupService(settings, repo, pnp, kwa);

            var productName = "большая простыня";
            try
            {
                svc.CloseGroupFor(productName);
            }
            catch (Exception e)
            {
                Assert.AreEqual($"Не удалось определить группу для {productName}", e.Message);
            }
        }

        private static readonly (string input, string output, int cnt)[] InputProductNames =
        {
            ("Шоколадный батончик Сникерс", "шоколадный батончик сникерс", 3),
            ("Стиральный порошок Сказка", "стиральный порошок сказка", 3)
        };

        private static readonly (string first, string second, bool isSimilar)[] SimilarWords =
        {
            ("шоколадный", "шоколадная", true ),
            ("порошок", "сникерс", false)
        };
    }
}
