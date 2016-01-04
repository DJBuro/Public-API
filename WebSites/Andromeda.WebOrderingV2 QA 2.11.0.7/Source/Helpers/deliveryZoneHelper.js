var deliveryZoneHelper =
{
    deliveryZones: ko.observable(undefined),
    isInDeliveryZone: function (targetDeliveryZone)
    {
        if (deliveryZoneHelper.deliveryZones() != undefined)
        {
            for (var index = 0; index < deliveryZoneHelper.deliveryZones().length; index++)
            {
                var possibleDeliveryZone = deliveryZoneHelper.deliveryZones()[index];
                possibleDeliveryZone = possibleDeliveryZone.toUpperCase(); //possibleDeliveryZone.replace(/\s+/g, '').toUpperCase();

                // Does the customers delivery zone start with the possible delivery zone?
                if (targetDeliveryZone.toUpperCase().slice(0, possibleDeliveryZone.length) == possibleDeliveryZone)
                {
                    return true;
                }
            }

            return false;
        }
    }
}