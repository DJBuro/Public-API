/// <reference path="../../scripts/typings/knockout/knockout.d.ts" />
var AndroWeb;
(function (AndroWeb) {
    var Services;
    (function (Services) {
        var LoyaltyService = (function () {
            function LoyaltyService() {
            }
            LoyaltyService.prototype.initialise = function (viewmodel) {
                loyaltyHelper.viewModel = viewmodel;
                loyaltyHelper.cartHelper = cartHelper;
                loyaltyHelper.checkoutHelper = checkoutHelper;
                loyaltyHelper.unavailableReasons = [];
                loyaltyHelper.loyaltySession = {
                    // based on the rules ... the ui can or cannot redeem. 
                    canReedeemPoints: ko.observable(),
                    /* potential points to gain / spend
                     * may be equal to or lower than the customers' total depending on rules.
                     */
                    canGainThesePoints: ko.observable(),
                    canGainThesePointsValue: ko.observable(),
                    canGainThesePointsValueLabel: ko.observable(),
                    canSpendThesePoints: ko.observable(),
                    canSpendThesePointsValue: ko.observable(),
                    canSpendThesePointsValueLabel: ko.observable(),
                    /* customer's loyalty points */
                    customersAvailableLoyaltyPoints: ko.observable(),
                    customersAvailableLoyaltyPointsValue: ko.observable(),
                    customersAvailableLoyaltyPointsValueLabel: ko.observable(),
                    /* checkout values */
                    isSpendingThePoints: ko.observable(false),
                    gainedPoints: ko.observable(0),
                    gainedPointsValue: ko.observable(0),
                    gainedPointsValueLabel: ko.observable(),
                    spentPoints: ko.observable(0),
                    spentPointsValue: ko.observable(0),
                    spentPointsValueLabel: ko.observable(),
                    /* checkout display observables */
                    displayPointsLeft: ko.observable(0),
                    displayPointsValue: ko.observable(0),
                    displayPointsValueLabel: ko.observable(""),
                    minimumPointsMessage: ko.observable("")
                };
                loyaltyHelper.resetCustomerSession();
                loyaltyHelper.watchForSiteDetails();
            };
            LoyaltyService.prototype.resetCustomerSession = function () {
                loyaltyHelper.loyaltySession.canGainThesePoints(0);
                loyaltyHelper.loyaltySession.canGainThesePointsValue(0);
                loyaltyHelper.loyaltySession.canGainThesePointsValueLabel('');
                loyaltyHelper.loyaltySession.canSpendThesePoints(0);
                loyaltyHelper.loyaltySession.canSpendThesePointsValue(0);
                loyaltyHelper.loyaltySession.canSpendThesePointsValueLabel('');
                /* customer's loyalty points */
                loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints(0);
                loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValue(0);
                loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValueLabel('');
                /* checkout values */
                loyaltyHelper.loyaltySession.isSpendingThePoints(false);
                loyaltyHelper.loyaltySession.gainedPoints(0);
                loyaltyHelper.loyaltySession.gainedPointsValue(0);
                loyaltyHelper.loyaltySession.gainedPointsValueLabel('');
                loyaltyHelper.loyaltySession.spentPoints(0);
                loyaltyHelper.loyaltySession.spentPointsValue(0);
                loyaltyHelper.loyaltySession.spentPointsValueLabel('');
                /* checkout display observables */
                loyaltyHelper.loyaltySession.displayPointsLeft(0);
                loyaltyHelper.loyaltySession.displayPointsValue(0);
                loyaltyHelper.loyaltySession.displayPointsValueLabel('');
                if (loyaltyHelper.loyaltyOptions != undefined) {
                    // recalculate points
                    loyaltyHelper.refreshLoyalty(undefined);
                }
            };
            LoyaltyService.prototype.watchForSiteDetails = function () {
                loyaltyHelper.viewModel.siteDetails.subscribe(function (siteDetails) {
                    AndroWeb.Logger.Notify("Loyalty - Changing site details");
                    AndroWeb.Logger.Notify(siteDetails);
                    if (!siteDetails) {
                        AndroWeb.Logger.Error("Loyalty - No site details. Changing stores possibly");
                        return;
                    }
                    if (!siteDetails.siteLoyalties || siteDetails.siteLoyalties.length === 0) {
                        AndroWeb.Logger.Notify("There is no loyalty available to apply.");
                        return;
                    }
                    var andromedaLoyalty = siteDetails.siteLoyalties.filter(function (provider) {
                        var enabled = provider.Enabled, andromeda = provider.ProviderName.toLowerCase() === "andromeda";
                        return enabled && andromeda;
                    });
                    if (andromedaLoyalty.length === 0) {
                        AndroWeb.Logger.Notify("No valid loyalty configuration");
                        return;
                    }
                    loyaltyHelper.SetStoreOptions(andromedaLoyalty[0].Configuration);
                    loyaltyHelper.automateSomeObservables();
                });
            };
            LoyaltyService.prototype.automateSomeObservables = function () {
                if (!loyaltyHelper.IsEnabled())
                    return;
                var cart = cartHelper.cart();
                accountHelper.isLoggedIn.subscribe(function (loggedIn) {
                    // clear out the session
                    loyaltyHelper.resetCustomerSession();
                    if (!accountHelper.customerDetails.loyalties) {
                        return;
                    }
                    if (accountHelper.customerDetails.loyalties.length === 0) {
                        return;
                    }
                    // extract the customers loyalty details
                    var allCustomerLoyalties = accountHelper.customerDetails.loyalties;
                    var andromedaLoyalty = allCustomerLoyalties.filter(function (loyalty) {
                        return loyalty.ProviderName.toLowerCase() === "andromeda";
                    });
                    if (andromedaLoyalty.length === 0) {
                        return;
                    }
                    // the loyalty points that the customer currently has on the server
                    var worth = loyaltyHelper.convertPointsToValue(andromedaLoyalty[0].Points);
                    loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints(andromedaLoyalty[0].Points);
                    // perform loyalty calculations based on the customers loyalty info
                    loyaltyHelper.refreshLoyalty(undefined);
                });
            };
            LoyaltyService.prototype.convertPointsToValue = function (totalPoints) {
                if (!loyaltyHelper.loyaltyOptions) {
                    return 0;
                }
                //£1 = pointValue
                var pointValue = loyaltyHelper.loyaltyOptions.PointValue;
                var worth = (1.00 / pointValue) * totalPoints;
                return parseFloat((worth * 100).toFixed(2));
            };
            LoyaltyService.prototype.convertValueToPoints = function (value) {
                if (!loyaltyHelper.loyaltyOptions) {
                    return 0;
                }
                var worth = value / 100;
                var pointValue = loyaltyHelper.loyaltyOptions.PointValue;
                var totalPoints = worth / (1.00 / pointValue);
                return parseFloat(totalPoints.toFixed());
            };
            LoyaltyService.prototype.IsEnabled = function () {
                var isEnabled = loyaltyHelper.loyaltyOptions != undefined && loyaltyHelper.loyaltyOptions.Enabled != undefined && loyaltyHelper.loyaltyOptions.Enabled === true;
                return isEnabled;
            };
            LoyaltyService.prototype.SetStoreOptions = function (options) {
                AndroWeb.Logger.Notify("Setting up loyalty");
                loyaltyHelper.loyaltyOptions = options;
            };
            LoyaltyService.prototype.ProduceJsonForCheckout = function () {
                if (!loyaltyHelper.IsEnabled()) {
                    return [];
                }
                var results = [
                    {
                        providerName: "andromeda",
                        awardedPoints: loyaltyHelper.loyaltySession.gainedPoints(),
                        awardedPointsValue: loyaltyHelper.loyaltySession.gainedPointsValue(),
                        redeemedPoints: loyaltyHelper.loyaltySession.spentPoints(),
                        redeemedPointsValue: loyaltyHelper.loyaltySession.spentPointsValue()
                    }
                ];
                return results;
            };
            LoyaltyService.prototype.refreshLoyalty = function (cartDetails) {
                if (!loyaltyHelper.IsEnabled())
                    return;
                // calculate the total money value of the customers points
                loyaltyHelper.calculateTotalAvailablePointsMoneyValue();
                // calculate how many points the customer could potentially redeem
                loyaltyHelper.calculatePotentialRedeemablePoints();
                // calculate the number of points that could potentially be gained for this order
                loyaltyHelper.calculatePotentiallyGainedPoints();
                // redeem points as needed
                loyaltyHelper.calculateRedeemedPoints();
                // calculate the number of points that the customer will actually gain from this order
                loyaltyHelper.calculateActualGainedPoints();
                // calculate the number of points left after redemption
                loyaltyHelper.calculateRemainingPointsAfterRedeem();
                // The message that is shown when the customer does not have enough loyalty points to redeem
                if (loyaltyHelper.loyaltyOptions != undefined && loyaltyHelper.loyaltyOptions.MinimumPointsBeforeAvailable != undefined && loyaltyHelper.loyaltyOptions.MinimumPointsBeforeAvailable != null) {
                    var isMinimumPointsMet = loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints() > loyaltyHelper.loyaltyOptions.MinimumPointsBeforeAvailable;
                    var minimumPointsMessage = '';
                    if (!isMinimumPointsMet) {
                        minimumPointsMessage = textStrings.checkSMinimumLoyaltyPoints.replace("{minPoints}", loyaltyHelper.loyaltyOptions.MinimumPointsBeforeAvailable);
                    }
                    loyaltyHelper.loyaltySession.minimumPointsMessage(minimumPointsMessage);
                }
                // update the cart accordingly
                loyaltyHelper.refreshCartTotal(cartDetails);
            };
            LoyaltyService.prototype.calculateTotalAvailablePointsMoneyValue = function () {
                // work out how much the customers loyalty points are worth in real money
                // note that this is not neccesarily the amount that can be redeemed
                var worth = loyaltyHelper.convertPointsToValue(loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints());
                loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValue(worth);
                loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.calculatePotentialRedeemablePoints = function () {
                // calculate how many points can be redeemed on the current order
                // the total points that the customer has available
                var customerPoints = loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints();
                var customerPointsValue = loyaltyHelper.loyaltySession.customersAvailableLoyaltyPointsValue();
                // the part of the order that loyalty can be redeemed against
                var subTotal = loyaltyHelper.cartHelper.cart().preLoyaltySubTotalPrice() / 100;
                if (subTotal === 0 || customerPoints === 0) {
                    // order empty - no redeeming possible
                    loyaltyHelper.setPotentialRedeemablePoints(0);
                    return;
                }
                if (subTotal < loyaltyHelper.loyaltyOptions.MinimumOrderTotalAfterLoyalty) {
                    // order minimum threshold not met - no redeeming possible
                    loyaltyHelper.setPotentialRedeemablePoints(0);
                    return;
                }
                var minimumValueLeft = 0, maxByPercent = subTotal, maxByValue = subTotal;
                if (loyaltyHelper.loyaltyOptions.MinimumOrderTotalAfterLoyalty) {
                    minimumValueLeft = loyaltyHelper.loyaltyOptions.MinimumOrderTotalAfterLoyalty;
                }
                if (loyaltyHelper.loyaltyOptions.MaximumPercentThatCanBeClaimed) {
                    maxByPercent = subTotal * loyaltyHelper.loyaltyOptions.MaximumPercentThatCanBeClaimed;
                    if (subTotal - maxByPercent < minimumValueLeft) {
                        maxByPercent -= minimumValueLeft - (subTotal - maxByPercent);
                    }
                }
                if (loyaltyHelper.loyaltyOptions.MaximumValueThatCanBeClaimed) {
                    maxByValue = loyaltyHelper.loyaltyOptions.MaximumValueThatCanBeClaimed;
                    if (subTotal - maxByValue < minimumValueLeft) {
                        maxByValue -= minimumValueLeft - (subTotal - maxByValue);
                    }
                }
                maxByPercent *= 100;
                maxByValue *= 100;
                // pick max of percentage or value. (the maximum points that can be used) 
                var reduce = maxByPercent > maxByValue ? maxByPercent : maxByValue;
                // take the minimum based on points available as to which to spend (the maximum the customer can use). 
                var upperLimitThatCanBeSpent = reduce < customerPointsValue ? reduce : customerPointsValue;
                // eventually one limit will be hit. The max value the customer has to pay. Or the upper limit that the order can be paid for. 
                var availableFromCustomer = customerPointsValue < upperLimitThatCanBeSpent ? customerPointsValue : upperLimitThatCanBeSpent;
                // figure out how much the spendable points are actually worth
                loyaltyHelper.setPotentialRedeemablePoints(loyaltyHelper.convertValueToPoints(availableFromCustomer));
            };
            LoyaltyService.prototype.setPotentialRedeemablePoints = function (points) {
                loyaltyHelper.loyaltySession.canSpendThesePoints(points);
                // work out how much real money the customer can actually redeem
                var worth = loyaltyHelper.convertPointsToValue(loyaltyHelper.loyaltySession.canSpendThesePoints());
                loyaltyHelper.loyaltySession.canSpendThesePointsValue(worth);
                loyaltyHelper.loyaltySession.canSpendThesePointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.calculatePotentiallyGainedPoints = function () {
                // calculates the number of points that will be awarded for this order
                var subTotal = loyaltyHelper.cartHelper.cart().preLoyaltySubTotalPrice() / 100;
                if (subTotal === 0 || !loyaltyHelper.IsEnabled()) {
                    loyaltyHelper.loyaltySession.canGainThesePoints(0);
                    return;
                }
                var options = loyaltyHelper.loyaltyOptions;
                var addPoints = subTotal * options.AwardPointsPerPoundSpent;
                var awardMaximumPoints = 0;
                var pointsRounded = loyaltyHelper.loyaltyOptions.RoundUp ? Math.ceil(addPoints) : Math.floor(addPoints);
                // check the highest limit the customer can earn.  
                if (options.MaximumObtainablePoints > 0) {
                    //check that the limit is not reached and then limit the earned points then... 
                    var session = this.loyaltySession;
                    var pointsAfterRedeeming = pointsRounded + session.customersAvailableLoyaltyPoints() - session.spentPoints();
                    if (pointsAfterRedeeming > options.MaximumObtainablePoints) {
                        //max hit. Give the different between current and max.
                        pointsRounded = options.MaximumObtainablePoints - session.customersAvailableLoyaltyPoints() - session.spentPoints();
                    }
                }
                loyaltyHelper.setCanGainThesePoints(pointsRounded);
            };
            LoyaltyService.prototype.setCanGainThesePoints = function (canGainPoints) {
                loyaltyHelper.loyaltySession.canGainThesePoints(canGainPoints);
                var worth = loyaltyHelper.convertPointsToValue(loyaltyHelper.loyaltySession.canGainThesePoints());
                loyaltyHelper.loyaltySession.canGainThesePointsValue(worth);
                loyaltyHelper.loyaltySession.canGainThesePointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.calculateRedeemedPoints = function () {
                if (loyaltyHelper.loyaltySession.isSpendingThePoints()) {
                    loyaltyHelper.setRedeemedPoints(loyaltyHelper.loyaltySession.canSpendThesePoints());
                }
                else {
                    loyaltyHelper.setRedeemedPoints(0);
                }
            };
            LoyaltyService.prototype.setRedeemedPoints = function (redeemedPoints) {
                loyaltyHelper.loyaltySession.spentPoints(redeemedPoints);
                // calculate the cash value of points actually redeemed
                var worth = loyaltyHelper.convertPointsToValue(redeemedPoints);
                loyaltyHelper.loyaltySession.spentPointsValue(worth);
                loyaltyHelper.loyaltySession.spentPointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.calculateActualGainedPoints = function () {
                // gain points
                if (loyaltyHelper.loyaltySession.isSpendingThePoints()) {
                    loyaltyHelper.setGainedPoints(0);
                }
                else {
                    loyaltyHelper.setGainedPoints(loyaltyHelper.loyaltySession.canGainThesePoints());
                }
            };
            LoyaltyService.prototype.setGainedPoints = function (gainedPoints) {
                loyaltyHelper.loyaltySession.gainedPoints(gainedPoints);
                // calculate how much the gained points are actually worth in real money
                var worth = loyaltyHelper.convertPointsToValue(loyaltyHelper.loyaltySession.gainedPoints());
                loyaltyHelper.loyaltySession.gainedPointsValue(worth);
                loyaltyHelper.loyaltySession.gainedPointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.calculateRemainingPointsAfterRedeem = function () {
                // calculates the number of points left after redemption
                var x1 = loyaltyHelper.loyaltySession.customersAvailableLoyaltyPoints();
                var x2 = loyaltyHelper.loyaltySession.spentPoints();
                var left = Math.round(x1 - x2);
                loyaltyHelper.loyaltySession.displayPointsLeft(left);
                var worth = loyaltyHelper.convertPointsToValue(left);
                loyaltyHelper.loyaltySession.displayPointsValue(worth);
                loyaltyHelper.loyaltySession.displayPointsValueLabel(helper.formatPrice(worth));
            };
            LoyaltyService.prototype.refreshCartTotal = function (cartDetails) {
                if (cartDetails != undefined && loyaltyHelper.IsEnabled()) {
                    var loyaltyDiscount = loyaltyHelper.loyaltySession.spentPointsValue();
                    cartDetails.totalPrice -= loyaltyDiscount ? loyaltyDiscount : 0;
                }
            };
            return LoyaltyService;
        })();
        Services.LoyaltyService = LoyaltyService;
    })(Services = AndroWeb.Services || (AndroWeb.Services = {}));
})(AndroWeb || (AndroWeb = {}));
var loyaltyHelper = new AndroWeb.Services.LoyaltyService();
var loyaltyBindingHelper = {
    redeem: function () {
        // customer wants to redeem loyalty points
        loyaltyHelper.loyaltySession.isSpendingThePoints(true);
        cartHelper.refreshCart();
    },
    remove: function () {
        // customer doesn't want to redeem loyalty points
        loyaltyHelper.loyaltySession.isSpendingThePoints(false);
        cartHelper.refreshCart();
    }
};
//loyaltyHelper.SetStoreOptions({
//    Enabled: true,
//    AutoSpendPointsOverThisPeak: 100000,
//    AwardOnRegiration: 500,
//    AwardPointsPerPoundSpent: 10,
//    MaximumPercentThatCanBeClaimed: 1,
//    MaximumValueThatCanBeClaimed: 10,
//    MinimumOrderTotalAfterLoyalty: 5,
//    MinimumPointsBeforeAvailable: 0,
//    PointValue: 100,
//    RoundUp: true
//});
//# sourceMappingURL=loyaltyHelper.js.map