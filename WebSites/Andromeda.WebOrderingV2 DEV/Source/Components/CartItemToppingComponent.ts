/// <reference path="../../scripts/typings/react/react.d.ts" />
/// <reference path="../models/cartitemtopping.ts" />
/// <reference path="../models/cartitem.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartItemToppingComponentProps
    {
        cartItem: AndroWeb.Models.CartItem,
        cartItemTopping: AndroWeb.Models.CartItemTopping 
    }

    export class CartItemToppingComponent extends React.Component<CartItemToppingComponentProps, {}>
    {
        render()
        {
            /* <div class="cartRowItemTopping">
        <span class="cartIndent1" data-bind="html: cartName"></span>
        <span data-bind="'attr': { 'class': $parent.isEnabled() ? 'cartPrice' : 'cartPriceDisabled' }, html: cartPrice"></span>
    </div>
            */
            return React.createElement("div", { className: "cartRowItemTopping" }, "",
                React.createElement("span", { className: "cartIndent1", dangerouslySetInnerHTML: { __html: this.props.cartItemTopping.cartName() } }, null),
                React.createElement("span", { className: this.props.cartItem.isEnabled() ? "cartPrice" : "cartPriceDisabled", dangerouslySetInnerHTML: { __html: this.props.cartItemTopping.cartPrice() } }, null),
                this.props.cartItemTopping.notes.length > 0 ? React.createElement("div", { className: "cartNotes", dangerouslySetInnerHTML: { __html: this.props.cartItemTopping.notes } }, null) : null
                );
        }
    }
}