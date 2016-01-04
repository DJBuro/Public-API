/// <reference path="../../scripts/typings/react/react.d.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartDealComponentProps
    {
        cartDeal: AndroWeb.Models.CartDeal
    }

    export class CartDealComponent extends React.Component<CartDealComponentProps, {}>
    {
        render()
        {
            //return React.createElement("div", null, "This is a cart deal");

            return React.createElement("div", { className: this.props.cartDeal.isEnabled() ? "cartItem" : "cartItemDisabled" }, null,
                React.createElement("div", { className: "cartRow" }, null,
                    React.createElement("span", { className: "cartName", id: "cartLineItems" }, this.props.cartDeal.name()),
                    this.props.cartDeal.isEnabled() ? React.createElement("a", { click: AndroWeb.Helpers.CartHelper.removeDealItem }, null) : null,
                    React.createElement("span", { id: "cartLineItems", className: this.props.cartDeal.isEnabled() ? 'cartPrice' : 'cartPriceDisabled', dangerouslySetInnerHTML: { __html: this.props.cartDeal.displayPrice() } }, null),
                    this.getCartDeallines()
                    ),
                this.props.cartDeal.notes().length > 0 ? React.createElement("div", { className: "cartNotes", dangerouslySetInnerHTML: { __html: this.props.cartDeal.notes() } }, null) : null);


                //React.createElement("div", { id: "cartLineItems" }, null,
                //    React.createElement("div", { id: "cartLineItemsHeader" }, null,
                //        React.createElement("h2", { id: "cartLineItemsItemHeader" }, textStrings.sItem),
                //        React.createElement("h2", { id: "cartLineItemsPriceHeader" }, textStrings.sPrice),
                //        React.createElement("h2", { id: "cartLineItemsQuantityHeader" }, textStrings.sQuantity)
                //        ),
                //    this.getCartDeals(),
                //    this.getCartItems()
                //    ),
                //this.getSubTotal(),
                //this.getCharges(),
                //this.getGrandTotal()
                //);

            /*
             <div data-bind="'attr': { 'class': isEnabled() ? 'cartItem' : 'cartItemDisabled' }">
        <a data-bind="click: AndroWeb.Helpers.CartHelper.editCartDeal" href="#">
            
            
            <div class="cartRow">
                <span class="cartName" data-bind="html: name"></span>
                <a href="#" class="cartRemove" data-bind="visible: isEnabled(), click: AndroWeb.Helpers.CartHelper.removeDealItem"></a>
                <span data-bind="'attr': { 'class': isEnabled() ? 'cartPrice' : 'cartPriceDisabled' }, html: displayPrice()"></span>
            </div>
            <!-- ko foreach: cartDealLines -->
            <!-- ko template: { name: 'cartDealLine-template' } -->
            <!-- /ko -->
            <!-- /ko -->
            <!-- ko if: minimumOrderValueNotMet() -->
            <div class="cartRow cartError">
                <span data-bind="html: textStrings.cdMinSpend"></span>
                <span data-bind="html: minimumOrderValue"></span>
            </div>
            <!-- /ko -->
            <!-- ko if: $root.orderType() == 'delivery' && !dealWrapper.deal.ForDelivery -->
            <div class="notAvailable">This deal cannot be delivered to door</div>
            <!-- /ko -->
            <!-- ko if: $root.orderType() == 'collection' && !dealWrapper.deal.ForCollection -->
            <div class="notAvailable">This deal cannot be collected from store</div>
            <!-- /ko -->
        </a>
    </div>
             */
        }

        public getCartDeallines(): React.ReactElement<any>[]
        {
            var dealLineComponents: __React.ReactElement<any>[] = [];

            if (this.props.cartDeal.cartDealLines() == undefined || this.props.cartDeal.cartDealLines().length === 0) return dealLineComponents;

            for (var index = 0; index < this.props.cartDeal.cartDealLines().length; index++)
            {
                var cartDealLine = this.props.cartDeal.cartDealLines()[index];

                var cartDealLineComponentProps: AndroWeb.Components.CartDealLineComponentProps =
                    {
                        cartDealLine: cartDealLine
                    };

                var cartDealLineComponent = React.createElement(CartDealLineComponent, cartDealLineComponentProps, {});
                dealLineComponents.push(cartDealLineComponent);
            }

            return dealLineComponents;
        }
    }
}