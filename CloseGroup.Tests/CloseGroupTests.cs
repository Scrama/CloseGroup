using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CloseGroup.Tests
{
    public class TestData
    {
        public (string input, string output, int cnt)[] InputProductNames =
        {
            ("Шоколадный батончик Сникерс", "шоколадный батончик сникерс", 3),
            ("Стиральный порошок Сказка", "стиральный порошок сказка", 3)
        };
    }
    [TestFixture]
    public class CloseGroupTests
    {
        [TestCaseSource(nameof(ProductNamesCases))]
        public void ProductNameProcessorTest((string input, string output, int cnt) casePair)
        {
            var pnp = new ProductNameProcessor();
            var list = pnp.Process(casePair.input);
            Assert.AreEqual(list.Count, casePair.cnt);
        }

        private static IEnumerable<(string input, string output, int cnt)> ProductNamesCases()
        {
            foreach (var tuple in InputProductNames)
            {
                yield return tuple;
            }
        }

        private static readonly (string input, string output, int cnt)[] InputProductNames =
        {
            ("Шоколадный батончик Сникерс", "шоколадный батончик сникерс", 3),
            ("Стиральный порошок Сказка", "стиральный порошок сказка", 3)
        };
    }
}
