/// <reference path="MyAndromeda.WebOrdering.App.ts" />
module MyAndromeda.WebOrdering.Controllers {

    Angular.ControllersInitilizations.push((app) => {
        app.controller("CustomEmailTemplateController",(
            $scope: Scopes.ICustomEmailTemplateScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService
            ) => {

            var setLiveColours = () => {
                Logger.Notify("Set live colours");
                contextService.Model.CustomEmailTemplate.LiveHeaderColour =
                contextService.Model.CustomEmailTemplate.HeaderColour;
                contextService.Model.CustomEmailTemplate.LiveFooterColour =
                contextService.Model.CustomEmailTemplate.FooterColour;
            };
            var resetColours = () => {
                Logger.Notify("Resetting email template colors");
                var customEmailTemplate = contextService.Model.CustomEmailTemplate;
                customEmailTemplate.HeaderColour = customEmailTemplate.LiveHeaderColour;
                customEmailTemplate.FooterColour = customEmailTemplate.LiveFooterColour;
            };
            var saveChanges = () => {
                Logger.Notify("Save");
                setLiveColours();
                webOrderingWebApiService.Update();
            };

            $scope.SetLiveColors = setLiveColours;
            $scope.ResetColors = resetColours;
            $scope.SaveChanges = saveChanges;

            

            contextService.ModelSubject
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