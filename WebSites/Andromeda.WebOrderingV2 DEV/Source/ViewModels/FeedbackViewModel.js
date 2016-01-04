/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/modernizr/modernizr.d.ts" />
/// <reference path="baseviewmodel.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var AndroWeb;
(function (AndroWeb) {
    var ViewModels;
    (function (ViewModels) {
        var FeedbackCategory = (function () {
            function FeedbackCategory(id, displayText) {
                this.displayText = "";
                this.id = id;
                this.displayText = displayText;
            }
            return FeedbackCategory;
        })();
        ViewModels.FeedbackCategory = FeedbackCategory;
        var Feedback = (function () {
            function Feedback(name, email, feedbackCategory, feedbackCategoryName, feedback, storeName) {
                this.name = "";
                this.email = "";
                this.feedback = "";
                this.storeName = "";
                this.name = name;
                this.email = email;
                this.feedbackCategory = feedbackCategory;
                this.feedbackCategoryName = feedbackCategoryName;
                this.feedback = feedback;
                this.storeName = storeName;
            }
            return Feedback;
        })();
        ViewModels.Feedback = Feedback;
        var FeedbackViewModel = (function (_super) {
            __extends(FeedbackViewModel, _super);
            function FeedbackViewModel() {
                var _this = this;
                _super.call(this);
                // Feedback form (it'd be nice to use the Feedback object but that causes binding problems)
                this.name = "";
                this.email = "";
                this.feedback = "";
                this.submitFeedback = function () {
                    if (_this.feedbackCategory === undefined || _this.feedbackCategory.id === 0) {
                        // No feedback category selected
                        _this.hasError(true);
                        _this.errorMessage(textStrings.feedbackCategoryMissingError);
                    }
                    else if (_this.feedback.length === 0) {
                        // Feedback field is empty
                        _this.hasError(true);
                        _this.errorMessage(textStrings.feedbackMissingError);
                    }
                    else {
                        // No errors ... yet
                        _this.hasError(false);
                        _this.errorMessage("");
                        // The callback loses this
                        var self = _this;
                        // Show please wait
                        guiHelper.showPleaseWait(textStrings.feedbackSubmitting, '', function (pleaseWaitViewModel) {
                            var store = viewModel.selectedSite() === undefined ? { name: "", siteId: "" } : viewModel.selectedSite();
                            // Send the feedback
                            acsapi.putFeedback(store.siteId, new Feedback(self.name, self.email, self.feedbackCategory.id, self.feedbackCategory.displayText, self.feedback, store.name), function (success) {
                                if (success) {
                                    // Tell the user it worked
                                    self.hasError(true);
                                    self.errorMessage(textStrings.feedbackSuccess);
                                    // Clear the feedback form
                                    self.name = "";
                                    self.email = "";
                                    self.feedbackCategory = undefined;
                                    self.feedback = "";
                                }
                                else {
                                    // Tell the user there was a technical problem
                                    self.hasError(true);
                                    self.errorMessage(textStrings.feedbackServerError);
                                }
                                // Return to the Feedback form
                                viewModel.pageManager.showPage("Feedback", true, self);
                            });
                        });
                    }
                };
                this.submitFeedback = this.submitFeedback.bind(this);
                this.feedbackCategories = ko.observableArray([
                    new FeedbackCategory(0, textStrings.selectAFeedbackCategory),
                    new FeedbackCategory(1, textStrings.feedbackCatWebsiteProblem),
                    new FeedbackCategory(2, textStrings.feedbackCatGeneralWebsite),
                    new FeedbackCategory(3, textStrings.feedbackCatStore)
                ]);
                this.feedbackCategory = this.feedbackCategories()[0];
                this.hasError = ko.observable(false);
                this.errorMessage = ko.observable("");
            }
            return FeedbackViewModel;
        })(ViewModels.BaseViewModel);
        ViewModels.FeedbackViewModel = FeedbackViewModel;
    })(ViewModels = AndroWeb.ViewModels || (AndroWeb.ViewModels = {}));
})(AndroWeb || (AndroWeb = {}));
//# sourceMappingURL=FeedbackViewModel.js.map