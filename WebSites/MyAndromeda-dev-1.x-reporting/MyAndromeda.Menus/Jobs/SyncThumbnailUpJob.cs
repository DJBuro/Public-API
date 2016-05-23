using System;
using System.Threading.Tasks;
using WebBackgrounder;

namespace MyAndromeda.Menus.Jobs
{
    public class SyncThumbnailUpJob : Job 
    {
        public SyncThumbnailUpJob(TimeSpan interval, TimeSpan timeout)
            : base(name: "FTP Menu Sync job", interval: interval, timeout: timeout)
        {
        }

        public override Task Execute()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }

}