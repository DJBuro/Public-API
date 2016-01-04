/// <reference path="../../scripts/typings/react/react.d.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartDealLineComponentProps
    {
        cartDealLine: AndroWeb.Models.CartDealLine
    }

    export class CartDealLineComponent extends React.Component<CartDealLineComponentProps, {}>
    {
        render()
        {
            return React.createElement("div", {}, null,
                React.createElement("div", { className: "cartRowDealLine" }, null,
                    React.createElement("span", { className: "cartIndent1", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.displayName() } }, null),
                    React.createElement("span", { className: "cartPrice", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.displayPrice() } }, null)
                    ),
                    this.getCategoryPremium(),
                    this.getItemPremium(),
                    this.getToppings());

            //<div class="cartRowDealLine" >
            //    <span class="cartIndent1" data-bind="html: displayName" > </span>
            //    <span data-bind="'attr': { 'class': $parent.isEnabled() ? 'cartPrice' : 'cartPriceDisabled' }, html: displayPrice()" > -</span>
            //</div>
        }

        public getCategoryPremium(): React.ReactElement<any>
        {
            var categoryPremiumElements: __React.ReactElement<any>;

            if (this.props.cartDealLine.categoryPremium() != undefined)
            {
                categoryPremiumElements = React.createElement("div", { className: "cartRowDealLineTopping" }, null,
                    React.createElement("span", { className: "cartIndent1", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.categoryPremiumName() } },  null),
                    React.createElement("span", { className: "cartPrice cartDealLinePrice", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.categoryPremium() } }, null));
            }

            //< !--ko if: categoryPremium() != undefined-- >
            //<div class="cartRowDealLineTopping" >
            //    <span class="cartIndent1" data-bind="html: categoryPremiumName" style= "margin-left:10px;" > </span>
            //    <span data-bind="'attr': { 'class': $parent.isEnabled() ? 'cartPrice cartDealLinePrice' : 'cartPriceDisabled cartDealLinePrice' }, html: categoryPremium" > </span>
            //< /div>
            //< !-- /ko -->

            return categoryPremiumElements;
        }

        public getItemPremium(): React.ReactElement<any>
        {
            var itemPremiumElements: __React.ReactElement<any>;

            if (this.props.cartDealLine.itemPremium() != undefined)
            {
                itemPremiumElements = React.createElement("div", { className: "cartRowDealLineTopping" }, null,
                    React.createElement("span", { className: "cartIndent1", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.itemPremiumName() } }, null),
                    React.createElement("span", { className: "cartPrice cartDealLinePrice", dangerouslySetInnerHTML: { __html: this.props.cartDealLine.itemPremium() } }, null));
            }

            //< !--ko if: itemPremium() != undefined-- >
            //<div class="cartRowDealLineTopping" >
            //    <span class="cartIndent1" data-bind="html: itemPremiumName" style= "margin-left:10px;" > </span>
            //    <span data-bind="'attr': { 'class': $parent.isEnabled() ? 'cartPrice cartDealLinePrice' : 'cartPriceDisabled cartDealLinePrice' }, html: itemPremium" > </span>
            //< /div>           

            return itemPremiumElements;
        }

        public getToppings(): React.ReactElement<any>[]
        {
            var toppingElements: __React.ReactElement<any>[] = [];

            for (var toppingsIndex: number = 0; toppingsIndex < this.props.cartDealLine.cartItem().displayToppings().length; toppingsIndex++)
            {
                var topping = this.props.cartDealLine.cartItem().displayToppings()[toppingsIndex];

                toppingElements.push(React.createElement("div", { className: "cartRowDealLineTopping" }, null,
                    React.createElement("span", { className: "cartIndent2", dangerouslySetInnerHTML: { __html: topping.cartName() } }, null),
                    React.createElement("span", { className: "cartPrice cartDealLinePrice", dangerouslySetInnerHTML: { __html: topping.cartPrice() } }, null)));
            }

            //<!--ko foreach: cartItem().displayToppings-- >
            //<!--ko template: { name: 'cartDealLineTopping-template' } -->
            //<!-- /ko -->
            //< !-- /ko -->

            //<div class="cartRowDealLineTopping" >
            //<span class="cartIndent2" data- bind="html: cartName" style= "margin-left:10px;" > </span>
            //< span data- bind="'attr': { 'class': $parents[1].isEnabled() ? 'cartPrice cartDealLinePrice' : 'cartPriceDisabled cartDealLinePrice' }, html: cartPrice" > </span>
            //< /div>

            return toppingElements;
        }
    }
}