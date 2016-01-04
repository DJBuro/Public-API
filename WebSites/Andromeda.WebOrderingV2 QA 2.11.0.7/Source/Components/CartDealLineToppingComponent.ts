/// <reference path="../../scripts/typings/react/react.d.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartDealLineToppingComponentProps
    {
    }

    export class CartDealLineToppingComponent extends React.Component<CartDealLineToppingComponentProps, {}>
    {
        render()
        {
            return React.createElement("div", { className: "cartRowItemTopping" }, "This is a cart deal item topping",
                React.createElement("span", { className: "cartIndent1" }, "A Pizza"),
                React.createElement("span", { className: "cartPrice" }, "£12.50")
                );

  //          <div class="cartRowItemTopping" >
   //         <span class="cartIndent1" data- bind="html: cartName" > </span>
   //         < span data- bind="'attr': { 'class': $parent.isEnabled() ? 'cartPrice' : 'cartPriceDisabled' }, html: cartPrice" > </span>
   //         < /div>
        }
    }
}