using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.Model.MyAndromeda
{
    public class MyAndromedaDbWorkContexAccessor : IMyAndromedaDbWorkContextAccessor 
    {
        private MyAndromedaDbContext dbContext; 

        public MyAndromedaDbContext DbContext
        {
            get 
            {
                return this.dbContext ?? (this.dbContext = new MyAndromedaDbContext());
            }
            set 
            {
                this.dbContext = value;
            }
        }
    }
}