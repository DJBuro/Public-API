/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />
/// <reference path="baseviewmodel.ts" />

module AndroWeb
{
    export module ViewModels
    {
        export class FeedbackCategory
        {
            public id: number;
            public displayText: string = "";

            constructor(id: number, displayText: string)
            {
                this.id = id;
                this.displayText = displayText;
            }
        }

        export class Feedback
        {
            public name: string = "";
            public email: string = "";
            public feedbackCategory: number;
            public feedbackCategoryName: string;
            public feedback: string = "";
            public storeName: string = "";

            constructor(name: string, email: string, feedbackCategory: number, feedbackCategoryName: string, feedback: string, storeName: string)
            {
                this.name = name;
                this.email = email;
                this.feedbackCategory = feedbackCategory;
                this.feedbackCategoryName = feedbackCategoryName;
                this.feedback = feedback;
                this.storeName = storeName;
            }
        }

        export class FeedbackViewModel extends BaseViewModel
        {
            public feedbackCategories: KnockoutObservableArray<FeedbackCategory>;   
            public hasError: KnockoutObservable<boolean>;
            public errorMessage: KnockoutObservable<string>;

            // Feedback form (it'd be nice to use the Feedback object but that causes binding problems)
            public name: string = "";
            public email: string = "";
            public feedbackCategory: FeedbackCategory;
            public feedback: string = "";

            constructor()
            {
                super();

                this.submitFeedback = this.submitFeedback.bind(this);

                this.feedbackCategories = ko.observableArray
                (
                    [
                        new FeedbackCategory(0, textStrings.selectAFeedbackCategory),
                        new FeedbackCategory(1, textStrings.feedbackCatWebsiteProblem),
                        new FeedbackCategory(2, textStrings.feedbackCatGeneralWebsite),
                        new FeedbackCategory(3, textStrings.feedbackCatStore)
                    ]
                );
                this.feedbackCategory = this.feedbackCategories()[0];

                this.hasError = ko.observable(false);
                this.errorMessage = ko.observable("");
            }

            public submitFeedback = () =>
            {
                if (this.feedbackCategory === undefined || this.feedbackCategory.id === 0)
                {
                    // No feedback category selected
                    this.hasError(true);
                    this.errorMessage(textStrings.feedbackCategoryMissingError);
                }
                else if (this.feedback.length === 0)
                {
                    // Feedback field is empty
                    this.hasError(true);
                    this.errorMessage(textStrings.feedbackMissingError);
                }
                else
                {
                    // No errors ... yet
                    this.hasError(false);
                    this.errorMessage("");

                    // The callback loses this
                    var self = this;

                    // Show please wait
                    guiHelper.showPleaseWait
                    (
                        textStrings.feedbackSubmitting,
                        '',
                        function (pleaseWaitViewModel)
                        {
                            // Send the feedback
                            acsapi.putFeedback
                            (
                                viewModel.selectedSite() === undefined ? '' : viewModel.selectedSite().siteId,
                                new Feedback
                                (
                                    self.name,
                                    self.email,
                                    self.feedbackCategory.id,
                                    self.feedbackCategory.displayText,
                                    self.feedback,
                                    viewModel.selectedSite().name
                                ),
                                function (success)
                                {
                                    if (success)
                                    {
                                        // Tell the user it worked
                                        self.hasError(true);
                                        self.errorMessage(textStrings.feedbackSuccess);

                                        // Clear the feedback form
                                        self.name = "";
                                        self.email = "";
                                        self.feedbackCategory = undefined;
                                        self.feedback = "";
                                    }
                                    else
                                    {
                                        // Tell the user there was a technical problem
                                        self.hasError(true);
                                        self.errorMessage(textStrings.feedbackServerError);
                                    }

                                    // Return to the Feedback form
                                    viewModel.pageManager.showPage("Feedback", true, self);
                                }
                            );
                        }
                    );
                }
            }
        }
    }
} 