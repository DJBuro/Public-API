using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;


public partial class Checkout : LoyaltyPage
{

    public int availablePoints { get; set; }
    public int redemptionRatio { get; set;}
    public int availableDiscount { get; set; }

    public double currentOrderTotal { get; set;}
    
    protected void Page_Load(object sender, EventArgs e)
    {

        tdPoints.Visible = false;
        tbDiscount.Visible = false;
        
        currentOrderTotal = 0;

        if (Page.IsPostBack)
        {
            ltPoints.Text = "";

        }
        else
        {
            var countries = lib.GetCountries();
            var orderTypes = lib.GetOrderTypes();

            foreach (ListItem country in countries)
            {
                ddlCountry.Items.Add(country);
            }

            foreach (ListItem orderType in orderTypes)
            {
                ddlOrderType.Items.Add(orderType);
            }

        }
        if (tbLoyaltyCard.Text != "")
        {
            //note: make sure the LoyaltyCard is valid
            //todo: check if pin required to redeem?
            availablePoints = lib.AvailablePoints(tbLoyaltyCard.Text);
            redemptionRatio = lib.LoyaltyRatios().RedemptionRatio;

            availableDiscount = availablePoints/redemptionRatio;

            //todo: this is culture dependant, which can be wrong, change to use right currency
            var loyaltyDiscount = string.Format("{0:c}", availableDiscount);

            if (availablePoints > 0)
            {
                ltPoints.Text = "available points: " + availablePoints;

                //note: apply business logic
                //eg. do you want customers to only use amounts above £1?
                if (availableDiscount > 0)
                {
                    ltPoints.Text = ltPoints.Text + " which gives you an available discount of " + loyaltyDiscount;
                    tdPoints.Visible = true;
                    tbDiscount.Visible = true;
                }
            }
            else if (availablePoints == 0) // new card?
                {
                    ltPoints.Text = "sorry - no points associated with this card, but you can start collecting!";
                }
        }
    }

    protected void btnGetAvailablePoints_Click(object sender, EventArgs e)
    {
        
    }


    protected void btnOrder_Click(object sender, EventArgs e)
    {

        //note the serviceOrderAddress is optional
        //if it is a collection/dine-in, pass in a null to the method
        var add = new serviceOrderAddress();
        add.AddressLine1 = tbAddressLine1.Text;
        add.AddressLine2 = tbAddressLine2.Text;
        add.AddressLine3 = tbAddressLine3.Text;
        add.Town = tbTown.Text;
        add.Province = tbProvince.Text;
        add.PostCode = tbPostCode.Text;
        add.Directions = tbDirections.Text;
        add.Country = new Country();
        add.Country.Id = Int32.Parse(ddlCountry.SelectedItem.Value);

            
        var transactionHistory = new serviceTransactionHistory();

        //transactionHistory.Id //note: leave id null, value is populated after insert!

        double monitaryDiscount = 0;
        availablePoints = 0;        

            if(tbDiscount.Text.Length != 0)
            {
                monitaryDiscount = Convert.ToDouble(tbDiscount.Text);
                availablePoints = lib.AvailablePoints(tbLoyaltyCard.Text);
            }

            if (tbLoyaltyCard.Text.Length != 0)
            {
                redemptionRatio = lib.LoyaltyRatios().RedemptionRatio;

                availableDiscount = availablePoints/redemptionRatio;

                var redeemedPoints = monitaryDiscount*redemptionRatio;

                transactionHistory.LoyaltyPointsRedeemed = Convert.ToInt32(redeemedPoints);
                //transactionHistory.LoyaltyPointsValue = not needed here, automatically calculated;

                //As the ratio may change in future, keep the value to now.
                transactionHistory.OrderTypeId = Int32.Parse(ddlOrderType.SelectedItem.Value);

                //Your order id, for your reference, use as a 'hook' for your system integration
                transactionHistory.OrderId = tbOrderReference.Text.Trim(); 

                serviceOrderItemHistory[] itemHistory = new serviceOrderItemHistory[2];

                //itemHistory[i].Id  //note: leave Id null, value is populated after insert!
                itemHistory[0] = new serviceOrderItemHistory();
                itemHistory[0].ItemLoyaltyPoints = Convert.ToInt32(tbItem1Points.Text);
                itemHistory[0].ItemPrice = Convert.ToDecimal(tbItem1Value.Text);
                itemHistory[0].Name = tbItem1Name.Text;

                itemHistory[1] = new serviceOrderItemHistory();
                itemHistory[1].ItemLoyaltyPoints = Convert.ToInt32(tbItem2Points.Text);
                itemHistory[1].ItemPrice = Convert.ToDecimal(tbItem2Value.Text);
                itemHistory[1].Name = tbItem2Name.Text;

                transactionHistory.OrderItemHistory = itemHistory;

                //read the documentation on this property, very important as this property has precedence
                //over the item history points.
                transactionHistory.LoyaltyPointsAdded = itemHistory[0].ItemLoyaltyPoints +
                                                        itemHistory[1].ItemLoyaltyPoints;

                transactionHistory.OrderTotal = itemHistory[0].ItemPrice + itemHistory[1].ItemPrice;

                lib.ApplyLoyaltyCardDiscount(tbLoyaltyCard.Text, transactionHistory, add);
            }

        Response.Redirect("ThankYou.aspx");

    }
}
