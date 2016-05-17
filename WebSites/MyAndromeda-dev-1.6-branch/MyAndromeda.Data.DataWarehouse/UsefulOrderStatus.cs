namespace MyAndromeda.Data.DataWarehouse
{
    public enum UsefulOrderStatus
    {
        //Id	Description
        OrderCreated = 0,
        OrderHasBeenReceivedByTheStore = 1,
        OrderIsInOven = 2,
        OrderIsReadyForDispatch = 3,
        OrderIsOutForDelivery = 4,
        OrderHasBeenCompleted = 5,
        OrderHasBeenCancelled = 6,
        FutureOrder = 8,
        OrderRefusedStoreOffline = 9,

        HelpDeskHaveIt = 1000,
        PrinterRejection,

        NoIdea 
    }
}