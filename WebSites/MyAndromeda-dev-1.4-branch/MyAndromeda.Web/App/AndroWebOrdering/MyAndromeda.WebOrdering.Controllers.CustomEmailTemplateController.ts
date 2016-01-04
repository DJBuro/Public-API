/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {

    Angular.ControllersInitilizations.push((app) => {
        app.controller("CustomEmailTemplateController",(
            $scope: Scopes.ICustomEmailTemplateScope,
            $timeout: ng.ITimeoutService,
            ContextService: Services.ContextService,
            WebOrderingWebApiService: Services.WebOrderingWebApiService
            ) => {

            var setLiveColours = () => {
                Logger.Notify("Set live colours");
                ContextService.Model.CustomEmailTemplate.LiveHeaderColour =
                ContextService.Model.CustomEmailTemplate.HeaderColour;
                ContextService.Model.CustomEmailTemplate.LiveFooterColour =
                ContextService.Model.CustomEmailTemplate.FooterColour;
            };
            var resetColours = () => {
                Logger.Notify("Resetting email template colors");
                var customEmailTemplate = ContextService.Model.CustomEmailTemplate;
                customEmailTemplate.HeaderColour = customEmailTemplate.LiveHeaderColour;
                customEmailTemplate.FooterColour = customEmailTemplate.LiveFooterColour;
            };
            var saveChanges = () => {
                Logger.Notify("Save");
                setLiveColours();
                WebOrderingWebApiService.Update();
            };

            $scope.SetLiveColors = setLiveColours;
            $scope.ResetColors = resetColours;
            $scope.SaveChanges = saveChanges;

            

            ContextService.ModelSubject
                .where(e=> e !== null)
                .subscribe((websiteSettings) => {

                //set the scope
                var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                var correct = (key: string, value: string) => {
                    if (!customEmailTemplate.get(key)) {
                        customEmailTemplate.set(key, "#EEEEEE");
                    }
                };
                correct("HeaderColour", "HeaderColour");
                correct("FooterColour", "FooterColour");
                setLiveColours();
                $scope.CustomEmailTemplate = websiteSettings.CustomEmailTemplate;
            });
        });
    });
}    