/// <reference path="cartdealcomponent.ts" />
/// <reference path="../../scripts/typings/react/react.d.ts" />
/// <reference path="../models/cart.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartProps
    {
        cart: AndroWeb.Models.Cart
    }

    export class CartComponent extends React.Component<CartProps, {}>
    {
        render()
        {
         //   <div id="cartWrapper" >

       /*     <div id="cartLineItems">
                        <div id="cartLineItemsHeader">
                            <h2 id="cartLineItemsItemHeader">Item</h2>
                            <h2 id="cartLineItemsPriceHeader">Price</h2>
                            <h2 id="cartLineItemsQuantityHeader">Quantity</h2>
                        </div>*/

            // <div id="cartDealItemsWrapper">

            // <div id="cartMenuItemsWrapper">

            return React.createElement("div", { id: "cartWrapper" }, null,
                React.createElement("div", { id: "cartLineItems" }, null,
                    React.createElement("div", { id: "cartLineItemsHeader" }, null,
                        React.createElement("h2", { id: "cartLineItemsItemHeader" }, textStrings.sItem),
                        React.createElement("h2", { id: "cartLineItemsPriceHeader" }, textStrings.sPrice),
                        React.createElement("h2", { id: "cartLineItemsQuantityHeader" }, textStrings.sQuantity)
                    ),
                    this.getCartDeals(),
                    this.getCartItems()
                    ),
                this.getSubTotal(),
                this.getCharges(),
                this.getGrandTotal()
            );
        }

        getCartDeals(): React.ReactElement<any>[]
        {
            var cartDealsComponents: React.ReactElement<any>[] = [];

            // Are there actually any deals?
            if (this.props.cart.deals() === undefined || this.props.cart.deals().length == 0) return cartDealsComponents;

            var cartDealsElements: React.ReactElement<any>[] = [];

            // Render a cart deal component for each deal
            for (var index = 0; index < this.props.cart.deals().length; index++)
            {
                var dealCartItem = this.props.cart.deals()[index];

                var cartDealComponentProps: CartDealComponentProps =
                    {
                        cartDeal: dealCartItem
                    };

                var cartDealComponent = React.createElement(CartDealComponent, cartDealComponentProps, {});
                cartDealsElements.push(cartDealComponent);
            }

            // Wrapper div
            var wrapper: React.ReactElement<any> = React.createElement("div", { id: "cartDealItemsWrapper" }, cartDealsElements);

            cartDealsComponents.push(wrapper, null);

            return cartDealsComponents;
        }

        getCartItems(): React.ReactElement<any>[]
        {
            var cartItemComponents: React.ReactElement<any>[] = [];

            // Are there actually any menu items?
            if (this.props.cart.cartItems() === undefined || this.props.cart.cartItems().length == 0) return cartItemComponents;
 
            var cartItemElements: React.ReactElement<any>[] = [];

            // Render a cart item component for each menu item
            for (var index = 0; index < this.props.cart.cartItems().length; index++)
            {
                var cartItem = this.props.cart.cartItems()[index];

                var cartItemComponentProps: CartItemComponentProps =
                    {
                        cartItem: cartItem
                    };

                var cartItemComponent = React.createElement(CartItemComponent, cartItemComponentProps, {});
                cartItemElements.push(cartItemComponent);
            }

            // Wrapper div
            var wrapper: React.ReactElement<any> = React.createElement("div", { id: "cartMenuItemsWrapper" }, cartItemElements);

            cartItemComponents.push(wrapper, null);

            return cartItemComponents;
        }

        getSubTotal(): React.ReactElement<any>[]
        {
            var subTotalComponents: React.ReactElement<any>[] = [];

            //<div id="cartSubTotalWrapper" >
            //<span class="cartTotal" data- bind="html: textStrings.sSubTotal" > </span>
            //< span class="cartTotalPrice" data- bind="html: AndroWeb.Helpers.CartHelper.cart().displaySubTotalPrice" > </span>
            //< /div>

            if (this.props.cart.displayDeliveryCharge != undefined && this.props.cart.displayDeliveryCharge().length > 0)
            {
                subTotalComponents.push(React.createElement("div", { id: "cartSubTotalWrapper" },
                    React.createElement("span", { className: "cartTotal" }, textStrings.sSubTotal),
                    React.createElement("span", { className: "cartTotalPrice", dangerouslySetInnerHTML: { __html: this.props.cart.displaySubTotalPrice() } }, null)
                    ));
            }

            return subTotalComponents;
        }

        getCharges(): React.ReactElement<any>[]
        {
            var chargesComponents: React.ReactElement<any>[] = [];

            if (this.props.cart.displayDeliveryCharge != undefined && this.props.cart.displayDeliveryCharge().length > 0)
            {
                chargesComponents.push(
                    React.createElement("div", { id: "cartDiscountWrapper" }, null,
                        React.createElement("span", { className: "cartTotal" }, textStrings.sDeliveryCharge),
                        React.createElement("span", { className: "cartTotalPrice", dangerouslySetInnerHTML: { __html: this.props.cart.displayDeliveryCharge() } }, null)));

                //<div id="cartDiscountWrapper" >
                //<span class="cartTotal" data- bind="html: 'Delivery charge'" > </span>
                //< span class="cartTotalPrice" data- bind="html: AndroWeb.Helpers.CartHelper.cart().displayDeliveryCharge" > </span>
                //< /div>
            }
                        
            return chargesComponents;
        }

        getGrandTotal(): React.ReactElement<any>[]
        {
            var chargesComponents: React.ReactElement<any>[] = [];

            chargesComponents.push(
                React.createElement("div", { id: "cartTotalWrapper" }, null,
                    React.createElement("span", { className: "cartTotal" }, textStrings.sOrderTotal),
                    React.createElement("span", { className: "cartTotalPrice", dangerouslySetInnerHTML: { __html: this.props.cart.displayTotalPrice() } }, null)));

            //<div id="cartTotalWrapper" >
            //<span class="cartTotal" data- bind="html: textStrings.sOrderTotal" > </span>
            //< span class="cartTotalPrice" data- bind="html: AndroWeb.Helpers.CartHelper.cart().displayTotalPrice" > </span>
            //< /div>

            return chargesComponents;
        }
    }
}