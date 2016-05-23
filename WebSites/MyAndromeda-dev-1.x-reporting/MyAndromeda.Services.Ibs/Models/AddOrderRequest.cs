using System;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;
using System.Linq;
using System.Collections.Generic;

namespace MyAndromeda.Services.Ibs.Models
{
    public class AddOrderRequest 
    {
        /// <summary>
        /// Gets or sets the commit.
        /// </summary>
        /// <value>The commit.</value>
        public bool Commit { get { return true; } }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public eOrderType OrderType { get; set; }

        /// <summary>
        /// Gets or sets the wanted order day.
        /// </summary>
        /// <value>The wanted order day.</value>
        public int WantedOrderDay { get; set; }

        /// <summary>
        /// Gets or sets the wanted order month.
        /// </summary>
        /// <value>The wanted order month.</value>
        public int WantedOrderMonth { get; set; }

        /// <summary>
        /// Gets or sets the wanted order year.
        /// </summary>
        /// <value>The wanted order year.</value>
        public int WantedOrderYear { get; set; }

        /// <summary>
        /// Gets or sets the time slot from. eg 900
        /// </summary>
        /// <value>The time slot from.</value>
        public int TimeSlotFrom { get; set; }

        /// <summary>
        /// Gets or sets the time slot to. eg 915
        /// </summary>
        /// <value>The time slot to.</value>
        public int TimeSlotTo { get; set; }

        /// <summary>
        /// Gets or sets the customer no.
        /// </summary>
        /// <value>The customer no.</value>
        public long CustomerNo { get; set; }

        /// <summary>
        /// Gets or sets the customer details.
        /// You can specify a different set of delivery collection details from the customer original details NULL = use customer details
        /// </summary>
        /// <value>The customer details.</value>
        public cOrderCustomerDetails CustomerDetails { get; set; }

        /// <summary>
        /// Gets or sets the location time cat.
        /// </summary>
        /// <value>The location time cat.</value>
        public int LocationTimeCat { get; private set; }

        /// <summary>
        /// Gets or sets the delivery instructions.
        /// </summary>
        /// <value>The delivery instructions.</value>
        public string DeliveryInstructions { get; set; }

        /// <summary>
        /// Gets or sets the user reference.
        /// </summary>
        /// <value>The user reference.</value>
        public string UserReference { get; set; }

        /// <summary>
        /// Gets or sets the cost centre.
        /// </summary>
        /// <value>The cost centre.</value>
        public string CostCentre { get; private set; }

        /// <summary>
        /// Gets or sets the covers.
        /// </summary>
        /// <value>The covers.</value>
        public int Covers { get; private set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<cWebTransItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the photo bytes.
        /// </summary>
        /// <value>The photo bytes.</value>
        public byte[] PhotoBytes { get; private set; }

        /// <summary>
        /// Gets or sets the table number.
        /// </summary>
        /// <value>The table number.</value>
        public long TableNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id.</value>
        public string TransactionId { get; private set; }

        /// <summary>
        /// Gets or sets the order placed day.
        /// </summary>
        /// <value>The order placed day.</value>
        public int OrderPlacedDay { get; set; }

        /// <summary>
        /// Gets or sets the order placed month.
        /// </summary>
        /// <value>The order placed month.</value>
        public int OrderPlacedMonth { get; set; }

        /// <summary>
        /// Gets or sets the order placed year.
        /// </summary>
        /// <value>The order placed year.</value>
        public int OrderPlacedYear { get; set; }

        /// <summary>
        /// Gets or sets the order placed hour.
        /// </summary>
        /// <value>The order placed hour.</value>
        public int OrderPlacedHour { get; set; }

        /// <summary>
        /// Gets or sets the order placed min.
        /// </summary>
        /// <value>The order placed min.</value>
        public int OrderPlacedMin { get; set; }

        /// <summary>
        /// Gets or sets the room selection.
        /// </summary>
        /// <value>The room selection.</value>
        public int RoomSelection { get; private set; }

        /// <summary>
        /// Gets or sets the area selection.
        /// </summary>
        /// <value>The area selection.</value>
        public int AreaSelection { get; private set; }

        /// <summary>
        /// Gets or sets the send email.
        /// </summary>
        /// <value>The send email.</value>
        public bool SendEmail { get; private set; }

        /// <summary>
        /// Gets or sets the loyalty card number.
        /// </summary>
        /// <value>The loyalty card number.</value>
        public string LoyaltyCardNumber { get; private set; }

        /// <summary>
        /// Gets or sets the pay on collection or delivery.
        /// </summary>
        /// <value>The pay on collection or delivery.</value>
        public bool PayOnCollectionOrDelivery { get; set; }

        /// <summary>
        /// Gets the print at store.
        /// </summary>
        /// <value>The print at store.</value>
        public ePrintOrderStatus PrintAtStore
        {
            get
            {
                return ePrintOrderStatus.eNotSet;
            }
        }

        /// <summary>
        /// Gets the type of the production.
        /// </summary>
        /// <value>The type of the production.</value>
        public eProductionType ProductionType
        {
            get
            {
                return eProductionType.eNotSet;
            }
        }

        /// <summary>
        /// Gets or sets the training mode.
        /// </summary>
        /// <value>The training mode.</value>
        public bool TrainingMode { get { return false; } }

        /// <summary>
        /// Gets or sets the confirm on pos.
        /// </summary>
        /// <value>The confirm on pos.</value>
        public bool ConfirmOnPos { get; private set; }
    }
}