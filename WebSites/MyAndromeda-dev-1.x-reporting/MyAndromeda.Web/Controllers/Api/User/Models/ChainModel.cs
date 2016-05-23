namespace MyAndromeda.Web.Controllers.Api.User
{

    public class ChainModel 
    {
        public string Name { get; set; }

        public int StoreCount { get; set; }

        public int? ParentId { get; set; }

        public int Id { get; set; }

        public int ChildChainCount { get; set; }

        public int ChildStoreCount { get; set; }
    }

}