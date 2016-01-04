
using System.Collections;
using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface ISiteDao : IGenericDao<Site, int>
    {
        Site FindByIp(string ipAddress);
        IList<Site> FindAllActiveSitesByHeadOffice(HeadOffice headOffice);
        IList FindAllActiveSitesByRegion(Region region);
        IList<Site> FindAllActiveSitesByRegionIList(Region region);
        IList<Site> FindAllSitesByHeadOffice(HeadOffice headOffice);
        IList<Site> FindAllRegion(Region region);
        Site FindBySiteKey(int siteKey);
        Site FindByRamesesId(int? RamesesId);

    }
}
