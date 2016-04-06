using System.Collections;
using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Core
{
    public static class XmlSerializer
    {
        public static Driver XmlSerialize(this Driver driver)
        {
            return TranslateDriver(driver);           
        }

        public static IList<Order> XmlSerialize(this IList<Order> orders)
        {
            return TranslateOrders(orders);
        }

        public static Order XmlSerialize(this Order order)
        {
            return TranslateOrder(order);
        }

        public static IList<Tracker> XmlSerialize(this IList<Tracker> trackers)
        {
            return TranslateTrackers(trackers);
        }

        #region GetDriver

        private static IList<Tracker> TranslateTrackers(IList<Tracker> trackers)
        {
            foreach (var tracker in trackers)
            {
                tracker.Id = null;
                tracker.Store = null;
                tracker.Coordinates.Id = null;
                tracker.Apn = null;

                if (tracker.Driver !=null)
                {
                    tracker.Driver.Trackers = null;
                    tracker.Driver.Store = null;
                    tracker.Driver.DriverOrder = null;
                    tracker.Driver.Id = null;
                }
            }

            return trackers;
        }


        private static Driver TranslateDriver(Driver driver)
        {
            var drive = new Driver
                            {
                                Id = null,
                                Name = driver.Name,
                                Vehicle = driver.Vehicle,
                                ExternalDriverId = driver.ExternalDriverId,
                                DriverOrder = TranslateOrdersForDriver(driver.DriverOrder),
                                Trackers = TranslateTrackerForDriver(driver.Trackers),
                                Store = null
                            };

            return drive;
        }

        private static IList TranslateOrdersForDriver(IList driverOrders)
        {
            IList arrayList = new ArrayList();

            foreach (DriverOrder list in driverOrders)
            {

                TranslateOrder(list.Order);

                list.Id = null;
                list.Driver = null;
                list.Order.Store = null;

                foreach (OrderStatus orderStatus in list.Order.OrderStatus)
                {
                    orderStatus.Order = null;
                }

                foreach (Item item in list.Order.Items)
                {
                    item.Id = null;
                    item.Order = null;
                }
                
                arrayList.Add(list);
            }

            return arrayList;
        }

        private static IList TranslateTrackerForDriver(IList tracker)
        {
            IList arrayList = new ArrayList();

            foreach (Tracker sender in tracker)
            {
                sender.Id = null;
                sender.Driver = null;
                sender.Store = null;
                sender.Apn = null;
                arrayList.Add(sender);
            }

            return arrayList;
        }


        #endregion

        #region GetOrders

        private static IList<Order> TranslateOrders(IList<Order> orders)
        {
            var orderList = new List<Order>();

            foreach (Order order in orders)
            {
                orderList.Add(TranslateOrder(order));
            }

            return orderList;
        }

        private static Order TranslateOrder(Order order)
        {
            order.Id = null;
            order.Store = null;
            
            
            foreach (CustomerOrder customerOrder in order.CustomerOrder)
            {
                customerOrder.Id = null;
                customerOrder.Order = null;
                customerOrder.Customer.Id = null;
                customerOrder.Customer.CustomerOrders = null;
                customerOrder.Customer.Coordinates.Id = null;
            }

            foreach (Item item in order.Items)
            {
                item.Id = null;
               item.Order = null;
            }

            foreach (OrderStatus orderStatus in order.OrderStatus)
            {
                orderStatus.Id = null;
                orderStatus.Order = null;
            }

            return order;
        }
        
        #endregion


    }
}
