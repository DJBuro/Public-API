namespace MyAndromeda.Services.WebHooks.Models
{
    public interface ISpecificHook 
    {
        //string ExternalAcsApplicationId { get; set; }
        int? AcsApplicationId { get; set; }
    }
}