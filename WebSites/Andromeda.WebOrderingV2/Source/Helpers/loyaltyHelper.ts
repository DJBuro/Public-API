module AndroWeb.Services {
    export class LoyaltyService {
        private unavailableReasons: string[];
        
        //helpers
        private viewModel: AndroWeb.ViewModels.IViewModel;
        private checkoutHelper: Helpers.ICheckoutHelper;
        private cartHelper: Helpers.ICartHelper;

        //customer loyalty details.
        //private customerLoyalty: Models.ICustomerLoyalty;
        private customer: Models.ICustomer;


        private loyaltyOptions: Models.IAndromedaStoreLoyalty;
        public loyaltySession: Models.ICustomerLoyaltySession;

        constructor() {

            this.viewModel = viewModel;
            this.cartHelper = cartHelper;
            this.checkoutHelper = checkoutHelper;

            this.unavailableReasons = [];
            this.loyaltySession = {
                //based on the rules ... the ui can or cannot redeem. 
                canReedeemPoints: ko.observable<boolean>(),

                /* potential points to gain / spend 
                 * may be equal to or lower than the customers' total depending on rules.
                 */
                canGainThesePoints: ko.observable<number>(),
                canGainThesePointsValue: ko.observable<number>(),
                canGainThesePointsValueLabel: ko.observable<string>(),
                canSpendThesePoints: ko.observable<number>(),
                canSpendThesePointsValue: ko.observable<number>(),
                canSpendThesePointsValueLabel: ko.observable<string>(),

                /* customer's loyalty points */
                customersAvailableLoyaltyPoints: ko.observable<number>(),
                customersAvailableLoyaltyPointsValue: ko.observable<number>(),
                customersAvailableLoyaltyPointsValueLabel: ko.observable<string>(),
                
                
                /* checkout values */
                isSpendingThePoints: ko.observable<boolean>(false),

                gainedPoints: ko.observable<number>(0),
                gainedPointsValue: ko.observable<number>(0),
                gainedPointsValueLabel: ko.observable<string>(),

                spentPoints: ko.observable<number>(0),
                spentPointsValue: ko.observable<number>(0),
                spentPointsValueLabel: ko.observable<string>(),

                /* checkout display observables */
                displayPointsLeft: ko.observable(0),
                displayPointsValue: ko.observable(0),
                displayPointsValueLabel: ko.observable("")
            };

            this.AutomateSomeObservables();
            this.WatchForSiteDetails();

            var cart = cartHelper.cart();
            
            /* work out gained points  */
            cart.subTotalPrice.subscribe((total) => {
                var decimalValue = total / 100;

                this.WorkoutAwardedPoints(decimalValue);
                this.WorkoutPotentialRedeemablePoints(decimalValue);
            });


            accountHelper.isLoggedIn.subscribe((loggedIn) => {
                AndroWeb.Logger.Notify("check login: " + loggedIn);

                if (!loggedIn) {
                    this.customer = undefined;

                    this.loyaltySession.customersAvailableLoyaltyPoints(0);

                    return;
                }

                var customerDetails = accountHelper.customerDetails;
                this.customer = customerDetails;

                if (!this.customer.loyalties) {
                    AndroWeb.Logger.Notify("no loyalty - undefined");
                    return;
                }
                if (this.customer.loyalties.length === 0) {
                    AndroWeb.Logger.Notify("no loyalty - 0");
                    return;
                }

                var allCustomerLoyalties = this.customer.loyalties;
                var andromedaLoyalty = allCustomerLoyalties.filter((loyalty) => {
                    return loyalty.ProviderName.toLowerCase() === "andromeda";
                });

                if (andromedaLoyalty.length === 0) {
                    console.log("no loyalty - andromeda");
                    return;
                }

                var worth = this.ConvertPointsToValue(andromedaLoyalty[0].Points);

                this.loyaltySession.customersAvailableLoyaltyPoints(andromedaLoyalty[0].Points);
                this.WorkoutPotentialRedeemablePoints(cart.subTotalPrice() / 100);

                Logger.Notify("Finished with the customer for loyalty");
            });
        }

        public WatchForSiteDetails(): void {
            this.viewModel.siteDetails.subscribe((siteDetails) => {
                Logger.Notify("Loyalty - Changing site details");
                Logger.Notify(siteDetails);

                if (!siteDetails) {
                    Logger.Error("Loyalty - No site details. Changing stores possibly");
                    return;
                }
                if (!siteDetails.siteLoyalties || siteDetails.siteLoyalties.length === 0) {
                    Logger.Notify("There is no loyalty available to apply.");
                    return;
                }
                var andromedaLoyalty = siteDetails.siteLoyalties.filter(function (provider) {
                    var enabled = provider.Enabled, andromeda = provider.ProviderName.toLowerCase() === "andromeda";
                    return enabled && andromeda;
                });
                if (andromedaLoyalty.length === 0) {
                    Logger.Notify("No valid loyalty configuration");
                    return;
                }
                this.SetStoreOptions(andromedaLoyalty[0].Configuration);
            });
        }

        private AutomateSomeObservables(): void {
            this.loyaltySession.canGainThesePoints.subscribe(value => {
                Logger.Notify("Earned points changed: " + value);
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.canGainThesePointsValue(worth);
                this.loyaltySession.canGainThesePointsValueLabel(helper.formatPrice(worth));

                //gain points
                var custommerHasAddedToCheckout = this.loyaltySession.isSpendingThePoints();
                if (custommerHasAddedToCheckout) {
                    this.loyaltySession.gainedPoints(0);
                }
                else {
                    this.loyaltySession.gainedPoints(value);
                }
            });

            this.loyaltySession.canSpendThesePoints.subscribe(value => {
                Logger.Notify("Potentially redeemable points changed: " + value);
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.canSpendThesePointsValue(worth);
                this.loyaltySession.canSpendThesePointsValueLabel(helper.formatPrice(worth));

                var updateSpendingValue = this.loyaltySession.isSpendingThePoints();

                Logger.Notify("Is the user spending the points yet? " + updateSpendingValue);
                if (updateSpendingValue) {
                    this.loyaltySession.spentPoints(value);
                }
            });

            this.loyaltySession.customersAvailableLoyaltyPoints.subscribe(value => {
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.customersAvailableLoyaltyPointsValue(worth);
                this.loyaltySession.customersAvailableLoyaltyPointsValueLabel(helper.formatPrice(worth));
            });

            this.loyaltySession.spentPoints.subscribe(value => {
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.spentPointsValue(worth);
                this.loyaltySession.spentPointsValueLabel(helper.formatPrice(worth));
            });

            this.loyaltySession.spentPointsValueLabel.subscribe(value => {
                Logger.Notify("=== Im spending this much : " + value + " ===");
            });

            this.loyaltySession.gainedPoints.subscribe(value => {
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.gainedPointsValue(worth);
                this.loyaltySession.gainedPointsValueLabel(helper.formatPrice(worth));
            });

            this.loyaltySession.isSpendingThePoints.subscribe(isTrue => {
                Logger.Notify("User is adding or removing loyalty: adding - " + isTrue);

                if (isTrue) {
                    Logger.Notify("recalculate points for cart.");
                    var subTotal = this.cartHelper.cart().subTotalPrice();

                    this.WorkoutPotentialRedeemablePoints(subTotal / 100);
                }
                if (!isTrue) {
                    Logger.Notify("Removing points for cart.");
                    this.loyaltySession.spentPoints(0);

                    var subTotal = this.cartHelper.cart().subTotalPrice();

                    this.WorkoutAwardedPoints(subTotal / 100);
                }

                cartHelper.refreshCart();
            });

            /* loyalty accepted / rejected */
            var updatePointsLeft = (value) => {
                var x1 = this.loyaltySession.customersAvailableLoyaltyPoints();
                var x2 = this.loyaltySession.spentPoints();
                var left = Math.round(x1 - x2);
                this.loyaltySession.displayPointsLeft(left);
            };
            this.loyaltySession.customersAvailableLoyaltyPoints.subscribe((value) => {
                return updatePointsLeft(value);
            });
            this.loyaltySession.spentPoints.subscribe((value) => {
                return updatePointsLeft(value);
            });
            this.loyaltySession.displayPointsLeft.subscribe((value) => {
                var worth = this.ConvertPointsToValue(value);
                this.loyaltySession.displayPointsValue(worth);
                this.loyaltySession.displayPointsValueLabel(helper.formatPrice(worth));
            });
        }

        //Im a big mess. Run away. And calculate the highest possible amount given the rules and customer points
        //to how much the customer can spend.
        private WorkoutPotentialRedeemablePoints(subTotal: number): void {
            if (!this.customer) {
                Logger.Notify("Cant spend any points: No customer loaded")

                this.loyaltySession.canSpendThesePoints(0);
                return;
            }

            if (subTotal === 0 || customerPoints === 0) {
                Logger.Notify("Cant spend any points: SubTotal is 0")

                this.loyaltySession.canSpendThesePoints(0);
                return;
            }
            // ie is still unavailable due to restriction ... 
            // ***  BUG - this.loyaltyOptions is undefined
            //if (subTotal < this.loyaltyOptions.MinimumPointsBeforeAvailable) {
            //    Logger.Notify("Cant spend any points: under the minimum points available limit");

            //    this.loyaltySession.canSpendThesePoints(0);
            //    return;
            //}


            // ***  BUG - this.loyaltyOptions is undefined
            //if (subTotal < this.loyaltyOptions.MinimumOrderTotalAfterLoyalty) {
            //    Logger.Notify("Need to spend more - min: " + this.loyaltyOptions.MinimumOrderTotalAfterLoyalty);

            //    this.loyaltySession.canSpendThesePoints(0);
            //    return;
            //}

            var minimumValueLeft = 0,
                maxByPercent = subTotal,
                maxByValue = subTotal;

            var customerPoints = this.loyaltySession.customersAvailableLoyaltyPoints(),
                customerPointsValue = this.loyaltySession.customersAvailableLoyaltyPointsValue();

            Logger.Notify("Customer's point value: " + helper.formatPrice(customerPointsValue));

            // ***  BUG - this.loyaltyOptions is undefined
            //if (this.loyaltyOptions.MinimumOrderTotalAfterLoyalty) {
            //    minimumValueLeft = this.loyaltyOptions.MinimumOrderTotalAfterLoyalty;
            //}

            //if (this.loyaltyOptions.MaximumPercentThatCanBeClaimed) {
            //    maxByPercent = subTotal * this.loyaltyOptions.MaximumPercentThatCanBeClaimed;

            //    if (subTotal - maxByPercent < minimumValueLeft) {
            //        maxByPercent -= minimumValueLeft - (subTotal - maxByPercent);
            //    }
            //}
            //if (this.loyaltyOptions.MaximumValueThatCanBeClaimed) {
            //    maxByValue = this.loyaltyOptions.MaximumValueThatCanBeClaimed;

            //    if (subTotal - maxByValue < minimumValueLeft) {
            //        maxByValue -= minimumValueLeft - (subTotal - maxByValue);
            //    }
            //}

            maxByPercent *= 100;
            maxByValue *= 100;

            Logger.Notify("Max that can be spent by value: " + maxByValue);
            Logger.Notify("Max that can be spent by percentage: " + maxByPercent);
            
            //pick min of percentage or value. (the maximum points that can be used) 
            var reduce = maxByPercent < maxByValue ? maxByPercent : maxByValue;
            Logger.Notify("From the value or percentage this is the max that can be spent by loyalty: " + reduce);

            
            //take the minimum based on points available as to which to spend (the maximum the customer can use). 
            Logger.Notify("Customer point value: " + customerPointsValue);
            Logger.Notify("Max loyalty points that could be spent: " + reduce);
            var upperLimitOnSpending = reduce < customerPointsValue ? reduce : customerPointsValue;

            //eventually one limit will be hit. The max value the customer has to pay. Or the upper limit that the order can be paid for. 
            var availableFromCustomer = customerPointsValue < upperLimitOnSpending
                ? customerPointsValue
                : upperLimitOnSpending;

            var pointsValue = this.ConvertValueToPoints(availableFromCustomer);

            AndroWeb.Logger.Notify("Can Spend in this session: " + pointsValue);
            //only notifies when unique ... mmmkay 
            if (this.loyaltySession.canSpendThesePoints() === pointsValue) {
                this.loyaltySession.canSpendThesePoints.notifySubscribers(pointsValue);
            }
            else {
                this.loyaltySession.canSpendThesePoints(pointsValue);
            }
        }

        private WorkoutAwardedPoints(total: number): void
        {
            // ***  BUG - this.loyaltyOptions is undefined
            //if (total === 0 || !this.IsEnabled()) {
            //    this.loyaltySession.canGainThesePoints(0);
            //    return;
            //}

            //var addPoints = total * (this.loyaltyOptions === undefined ? 1 : this.loyaltyOptions.AwardPointsPerPoundSpent);
            //var rounded = (this.loyaltyOptions === undefined ? addPoints : (this.loyaltyOptions.RoundUp ? Math.ceil(addPoints) : Math.floor(addPoints)));

            //AndroWeb.Logger.Notify("Award points: " + rounded + " (" + addPoints + ")");

            //var worth = this.ConvertPointsToValue(rounded);

            this.loyaltySession.canGainThesePoints(0); //(rounded);
        }

        public ConvertPointsToValue(totalPoints: number): number
        {
            // ***  BUG - this.loyaltyOptions is undefined
            //if (!this.loyaltyOptions) {
            //    return 0;
            //}
            ////£1 = pointValue
            //var pointValue = this.loyaltyOptions.PointValue;
            //var worth = (1.00 / pointValue) * totalPoints;

            return 0; // worth * 100;
        }

        public ConvertValueToPoints(value: number): number
        {
            // ***  BUG - this.loyaltyOptions is undefined
            //if (!this.loyaltyOptions) {
            //    return 0;
            //}

            //var worth = value / 100;
            //var pointValue = this.loyaltyOptions.PointValue;

            //var totalPoints = worth / (1.00 / pointValue);

            //return totalPoints;
            return 0;
        }

        public IsEnabled(): boolean {
            if (!this.loyaltyOptions) {
                AndroWeb.Logger.Notify("no loyalty options");
                return false;
            }

            if (!this.loyaltyOptions.Enabled) {
                AndroWeb.Logger.Notify("loyalty is turned off");
                return false;
            }

            return true;
        }

        //public IsAvailable(): boolean {
        //    if (!this.IsEnabled) { return false; }

        //    if (this.cartHelper.cart().totalPrice() < this.loyaltyOptions.MinimumOrderTotalAfterLoyalty) {
        //        this.unavailableReasons.push("The order must be at least " + this.loyaltyOptions.MinimumOrderTotalAfterLoyalty);
        //    }
        //}

        public SetStoreOptions(options: Models.IAndromedaStoreLoyalty) {
            AndroWeb.Logger.Notify("Setting up loyalty");
            this.loyaltyOptions = options;
        }
        

        //public RedeemLoyaltyInCart() : void
        //{
        //    var cart = this.cartHelper.cart();
            
        //    var checkoutDetails = this.checkoutHelper.checkoutDetails();
        //    var loyaltyPoints = checkoutDetails.loyaltyPoints();

        //    //don't gain points while spending
        //    loyaltyPoints.gainedPoints(0);

        //    //turn on spending points
        //    loyaltyPoints.redeemedPoints(this.loyaltySession.canSpendThesePoints());
        //    loyaltyPoints.redeemedPointsValue(this.loyaltySession.canSpendThesePointsValue());
        //}

        public AddLoyaltyToCheckout(): void {
            this.loyaltySession.isSpendingThePoints(true);
        }

        public RemoveLoyaltyFromCart(): void {
            this.loyaltySession.isSpendingThePoints(false);
        }

        public ProduceJsonForCheckout(): Models.ICheckoutLoyalty[] {
            if (!this.IsEnabled()) { return []; }

            var results: Models.ICheckoutLoyalty[] = [
                {
                    providerName: "andromeda",
                    values: {
                        awardedPoints: this.loyaltySession.gainedPoints(),
                        awardedPointsValue: this.loyaltySession.gainedPointsValue(),
                        redeemedPoints: this.loyaltySession.spentPoints(),
                        redeemedPointsValue: this.loyaltySession.spentPointsValue()
                    }
                }
            ];
            //var result: Models.ICheckoutLoyalty= 

            return results;
        }

        public refreshCartTotal(cartDetails : Models.ICartDetails): void
        {
            AndroWeb.Logger.Notify("Adjust cartDetails.totalPrice");
            var saving = this.loyaltySession.spentPointsValue();
            cartDetails.totalPrice -= saving ? saving : 0;
        }
    }
}

var loyaltyHelper = new AndroWeb.Services.LoyaltyService();
var loyaltyBindingHelper = {
    redeem: () => {
        var action = $.proxy(loyaltyHelper.AddLoyaltyToCheckout, loyaltyHelper);
        AndroWeb.Logger.Notify("redeem points");
        action();
    },
    remove: () => {
        var action = $.proxy(loyaltyHelper.RemoveLoyaltyFromCart, loyaltyHelper);
        AndroWeb.Logger.Notify("remove points");
        action();
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
