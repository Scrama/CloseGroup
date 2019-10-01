using System;

namespace CloseGroup
{
    public interface ICloseGroupService
    {
        string CloseGroupFor(string productName);
    }

    public class CloseGroupService : ICloseGroupService
    {
        private readonly IRepo repo;

        public CloseGroupService(IRepo repo)
        {
            this.repo = repo;
        }

        public string CloseGroupFor(string productName)
        {
            var group = repo.GetGroupByExactProductName(productName);
            if (group != null)
                return group;

            return productName;
        }
    }
}
