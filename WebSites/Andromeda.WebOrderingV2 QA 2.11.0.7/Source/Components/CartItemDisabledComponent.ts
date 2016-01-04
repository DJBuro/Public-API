/// <reference path="../../scripts/typings/react/react.d.ts" />
module AndroWeb.Components
{
    import React = __React;

    export interface CartItemDisabledComponentProps
    {
    }

    export class CartItemDisabledComponent extends React.Component<CartItemDisabledComponentProps, {}>
    {
        render()
        {
            return React.createElement("div", null, "This is the cart item disabled");
        }
    }
}