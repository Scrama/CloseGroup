using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CloseGroup
{
    public interface IRepo
    {
        string GetGroupByExactProductName(string productName);
    }

    public class Repo : IRepo
    {
        private readonly List<Group> groups;

        public Repo(ISettings settings)
        {
            var path = settings.Get<string>("RepoPath");
            using (var file = File.OpenText(path))
            using (var reader = new JsonTextReader(file))
            {
                var serializer = JsonSerializer.Create();
                groups = serializer.Deserialize<List<Group>>(reader);
            }
        }

        public string GetGroupByExactProductName(string productName)
        {
            // поиск по точному соответствию имени лучше возлагать на SQL - при наличии индекса это будет очень быстро
            // сигнатура метода будет той же, внутри обращение в БД
            foreach (var @group in groups)
            {
                foreach (var product in @group.Products)
                {
                    if (product == productName)
                    {
                        return @group.Name;
                    }
                }
            }

            return null;
        }

        public string Trail()
        {
            return " repotrail";
        }
    }
}
