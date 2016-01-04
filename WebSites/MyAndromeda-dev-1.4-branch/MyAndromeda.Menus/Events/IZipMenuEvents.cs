using MyAndromeda.Core;

namespace MyAndromeda.Menus.Events
{
    public interface IZipMenuEvents : IDependency 
    {
        void MenuExtracting(DatabaseUpdatedEventContext context);

        void MenuExtractionFailed(DatabaseUpdatedEventContext context);

        void MenuExtracted(DatabaseUpdatedEventContext context);

        void MenuZipped(DatabaseUpdatedEventContext context);

        void MenuZipping(DatabaseUpdatedEventContext context);
    }
}