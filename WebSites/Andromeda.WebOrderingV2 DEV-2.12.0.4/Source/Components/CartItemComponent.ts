/// <reference path="cartitemtoppingcomponent.ts" />
/// <reference path="../../scripts/typings/react/react.d.ts" />
/// <reference path="../models/cartitem.ts" />
/// <reference path="cartitemtoppingcomponent.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartItemComponentProps
    {
        cartItem: AndroWeb.Models.CartItem
    }

    export class CartItemComponent extends React.Component<CartItemComponentProps, {}>
    {
        render()
        {
            return React.createElement("div", { className: this.props.cartItem.isEnabled() ? "cartItem" : "cartItemDisabled" }, null,
                React.createElement("div", { className: "cartRow" }, null,
                    //React.createElement("div", { className: "cartRemovedTimeSlot", title: this.props.cartItem.availabilityText() }, null),
                    React.createElement("a", { className: "cartName" }, this.props.cartItem.displayName()),
                    React.createElement("a", { className: "cartRemove" }, ""),
                    React.createElement("span", { className: "cartQuantity" }, this.props.cartItem.quantity()),
                    React.createElement("span", { className: this.props.cartItem.isEnabled() ? "cartPrice" : "cartPriceDisabled", dangerouslySetInnerHTML: {__html: this.props.cartItem.displayPrice() }}, null)
                    ),
                this.props.cartItem.notes().length > 0 ? React.createElement("div", { className: "cartNotes", dangerouslySetInnerHTML: { __html: this.props.cartItem.notes() } }, null) : null,
                this.getToppingElements());

            /*<div class="cartRemovedTimeSlot" data-bind="visible: removedTimeSlot(), attr: { title: availabilityText }"></div>*/

            /*
            <div data- bind="'attr': { 'class': isEnabled() ? 'cartItem' : 'cartItemDisabled' }" >
            <div class="cartRow" >
            <a class="cartName" data- bind="html: displayName, click: AndroWeb.Helpers.CartHelper.editCartItem" href= "#" > </a>
            < a href= "#" class="cartRemove" data- bind="visible: isEnabled(), click: AndroWeb.Helpers.CartHelper.removeCartItem" > </a>
            < span class="cartQuantity" data- bind="html: quantity" > </span>
            < span data- bind="'attr': { 'class': isEnabled() ? 'cartPrice' : 'cartPriceDisabled' }, html: displayPrice" > </span>
            < /div>

            < !--ko if: $root.orderType() == 'delivery' && (menuItem.DelPrice == undefined && menuItem.DeliveryPrice == undefined)-- >
                <div class="notAvailable" data- bind="html: textStrings.ciCollectOnly" > </div>
                < !-- /ko -->
                < !--ko if: $root.orderType() == 'collection' && (menuItem.ColPrice == undefined && menuItem.CollectionPrice == undefined)-- >
                    <div class="notAvailable" data- bind="html: textStrings.ciDeliverOnly" > </div>
                    < !-- /ko -->
                    < !--ko foreach: displayToppings-- >
                    <!--ko template: { name: 'cartTopping-template' } -->
                    <!-- /ko -->
                    < !-- /ko -->
                    < /div>
            */
        }

        getToppingElements(): React.ReactElement<any>[]
        {
            var toppingComponents: __React.ReactElement<any>[] = [];

            if (this.props.cartItem.toppings() === undefined || this.props.cartItem.toppings().length === 0) return toppingComponents;

            for (var index = 0; index < this.props.cartItem.toppings().length; index++)
            {
                var cartItemTopping = this.props.cartItem.toppings()[index];

                if ((cartItemTopping.type === "removable" && !cartItemTopping.selectedSingle() && !cartItemTopping.selectedDouble()) ||
                    (cartItemTopping.type !== "removable" && cartItemTopping.selectedSingle()) ||
                    (cartItemTopping.type !== "removable" && cartItemTopping.selectedDouble()))
                {
                    var cartoppingComponentProps: AndroWeb.Components.CartItemToppingComponentProps =
                        {
                            cartItem: this.props.cartItem,
                            cartItemTopping: cartItemTopping
                        };

                    var toppingComponent = React.createElement(CartItemToppingComponent, cartoppingComponentProps, {});
                    toppingComponents.push(toppingComponent);
                }
            }

            return toppingComponents;
        }
    }
}