module MyAndromeda.Hr.KendoThings {

    var extend = $.extend,
        Logger = MyAndromeda.Logger,
        k: any = kendo,
        ui: any = kendo.ui,
        kData: any = kendo.data,
        kDate: any = k.date,
        kAttr: any = k.attr,
        kGetter: any = k.getter,
        SchedulerTimelineView = ui.TimelineView,
        SchedulerMonthView = ui.MonthView,
        MS_PER_DAY = kDate.MS_PER_DAY,
        MS_PER_MINUTE = kDate.MS_PER_MINUTE,
        SchedulerView = ui.SchedulerView,
        NS = ".kendoTimelineWeekView";

    function customCreateLayoutConfiguration(name, resources, inner, something) {
        var resource = resources[0];
        if (resource) {
            var configuration = [];

            var data = resource.dataSource.view();

            for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                var defaultText = kendo.htmlEncode(k.getter(resource.dataTextField)(data[dataIndex]));
                var dataItem = data[dataIndex];

                MyAndromeda.Logger.Notify(dataItem);

                var templateText = `
                    <div>{0}</div>
                    <employee-pic id=\"'{0}'\"></employee-pic>`;

                templateText = kendo.format(templateText, dataItem.Id);

                var template = kendo.template("<a href='javascript: void(0)'>#=data.Email#</a>");
                var template2 = kendo.template("<a href='javascript: void(0)'>{{dataItem.Email}}</a>");
                var obj = {
                    text: template2(dataItem),// t(dataItem),
                    className: "k-slot-cell",
                    data: dataItem,
                    dataItem: dataItem
                };

                //var element = $(template2(data));
                //var scope = something.$angular_scope; //scope from _createColumnsLayout
                //scope('
                //this.angular is not defined :( 
                //this.angular('compile', function () {
                //    return {
                //        elements: element,
                //        data: [{ dataItem: data }]
                //    };
                //});
                //obj[name] = customCreateLayoutConfiguration(name, resources.slice(1), inner);

                //text version
                configuration.push(obj);
                //configuration.push(element);
            }
            return configuration;
        }
        return inner;
    }

    function shiftArray(array, idx) {
        return array.slice(idx).concat(array.slice(0, idx));
    }

    function createLayoutConfiguration(name, resources, inner, template) {
        var resource = resources[0];
        if (resource) {
            var configuration = [];
            var data = resource.dataSource.view();

            data = data.sort((a, b) => {
                let aValue = a.Department ? a.Department : "NA",
                    bValue = b.Department ? b.Department : "NA";

                return aValue.length - bValue.length;
            });

            for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                var dataItem: Models.IEmployeeTask = data[dataIndex];
                let things = Models.departments;
                let searchDepartments = things.filter(e=> e.text === dataItem.Department);
                var department = { text: 'None', majorColour: "#000", minorColour: "#000" };
                if (searchDepartments.length > 0)
                {
                    department = searchDepartments[0];
                }

                var obj = {
                    text: template({
                        text: kendo.htmlEncode(kGetter(resource.dataTextField)(data[dataIndex])),

                        majorColor: department.majorColour,
                        minorColor: department.minorColour,
                        employee: dataItem,
                        field: resource.field,
                        title: resource.title,
                        name: resource.name,
                        value: kGetter(resource.dataValueField)(data[dataIndex])
                    }),
                    className: 'k-slot-cell k-thingy'
                };
                obj[name] = createLayoutConfiguration(name, resources.slice(1), inner, template);
                configuration.push(obj);
            }
            return configuration;
        }
        return inner;
    }

    /**
     * ***********************************************************
     * Timeline view
     * ***********************************************************
     */

    let timeLineOptions: any = {
        nextDate: function () {
            return kDate.nextDay(this.startDate());
        },
        calculateDateRange: function () {
            var selectedDate = this.options.date,
                start = selectedDate,
                end = kDate,
                dates = [];
                
            for (let index = 0, length = 7; index < length; index++)
            {
                dates.push(start);
                start = kDate.nextDay(start);
            }

            this._render(dates);
        },
        
        _createEventElement: function (event) {
            var options = this.options;
            var editable = options.editable;
            var isMobile = this._isMobile();
            event.showDelete = editable && editable.destroy !== false && !isMobile;
            event.resizable = false; //editable && editable.resize !== false && !isMobile;
            event.ns = kendo.ns;
            event.resources = this.eventResources(event);
            event.inverseColor = event.resources && event.resources[0] ? this._shouldInverseResourceColor(event.resources[0]) : false;
            var element = $(this.eventTemplate(event));
            this.angular('compile', function () {
                return {
                    elements: element,
                    data: [{ dataItem: event }]
                };
            });
            return element;
        },
        _render: function (dates) {
            var that = this;
            dates = dates || [];
            that._dates = dates;
            that._startDate = dates[0];
            that._endDate = dates[dates.length - 1 || 0];
            that._calculateSlotRanges();
            that.createLayout(that._layout(dates));
            that._content(dates);
            that._footer();
            that._setContentWidth();
            that.refreshLayout();
            that.datesHeader.on('click' + NS, '.k-nav-day', function (e) {
                var th = $(e.currentTarget).closest('th');
                var slot = that._slotByPosition(th.offset().left, that.content.offset().top);
                that.trigger('navigate', {
                    view: 'timeline',
                    date: slot.startDate()
                });
            });
            that.timesHeader.find('table tr:last').hide();
            that.datesHeader.find('table tr:last').hide();
        },
        _positionEvent: function (eventObject) {
            var slotsCollection = eventObject.slotRange.collection;
            //var eventWidth = 100;
            var eventHeight = this.options.eventHeight + 2;
            var rect = eventObject.slotRange.innerRect(eventObject.start, eventObject.end, false);
            var left = this._adjustLeftPosition(rect.left);
            var width = rect.right - rect.left - 2;
            if (width < 0) {
                width = 0;
            }

            if (width < this.options.eventMinWidth) {
                var lastSlot = slotsCollection._slots[slotsCollection._slots.length - 1];
                var offsetRight = lastSlot.offsetLeft + lastSlot.offsetWidth;
                width = this.options.eventMinWidth;
                if (offsetRight < left + width) {
                    width = offsetRight - rect.left - 2;
                }
            }

            eventObject.element.css({
                top: eventObject.slotRange.start.offsetTop + eventObject.rowIndex * (eventHeight + 2) + 'px',
                left: left,
                height: 50,
                width: width//slot.clientWidth - 2
            });


            $(eventObject.element).hover(
                function () {
                    $(this).animate({ width: 100 });
                },
                function () {
                    $(this).animate({ width: width });
                }
            );

        },
        _renderEvents: function (events, groupIndex, eventGroup) {
            var event;
            var idx;
            var length;
            for (idx = 0, length = events.length; idx < length; idx++) {
                event = events[idx];
                if (this._isInDateSlot(event)) {
                    var isMultiDayEvent = event.isAllDay || event.end.getTime() - event.start.getTime() >= MS_PER_DAY;
                    var container = this.content;
                    if (isMultiDayEvent || this._isInTimeSlot(event)) {
                        var adjustedEvent = this._adjustEvent(event);
                        var group = this.groups[groupIndex];
                        if (!group._continuousEvents) {
                            group._continuousEvents = [];
                        }
                        var ranges = group.slotRanges(adjustedEvent.occurrence, false);
                        var range = ranges[0];
                        var element;
                        if (this._isInTimeSlot(adjustedEvent.occurrence)) {
                            element = this._createEventElement(adjustedEvent.occurrence, event, range.head || adjustedEvent.head, range.tail || adjustedEvent.tail);
                            element.appendTo(container).css({
                                top: 0,
                                height: this.options.eventHeight
                            });
                            var eventObject = {
                                start: adjustedEvent.occurrence._startTime || adjustedEvent.occurrence.start,
                                end: adjustedEvent.occurrence._endTime || adjustedEvent.occurrence.end,
                                element: element,
                                uid: event.uid,
                                slotRange: range,
                                rowIndex: 0,
                                offsetTop: 0
                            };
                            eventGroup.events[event.uid] = eventObject;
                            this.addContinuousEvent(group, range, element, event.isAllDay);
                            this._arrangeRows(eventObject, range, eventGroup);
                        }
                    }
                }
            }
        },
        //_groupRowHtml: function (group, colspan, level, groupHeaderBuilder, templates, skipColspan) {
        //    var that = this, html = '', idx, length, field = group.field, column = $.grep(leafColumns(that.columns), function (column) {
        //        return column.field == field;
        //    })[0] || {}, template = column.groupHeaderTemplate, text = (column.title || field) + ': ' + formatGroupValue(group.value, column.format, column.values, column.encoded), footerDefaults = that._groupAggregatesDefaultObject || {}, aggregates = extend({}, footerDefaults, group.aggregates), data = extend({}, {
        //        field: group.field,
        //        value: group.value,
        //        aggregates: aggregates
        //    }, group.aggregates[group.field]), groupFooterTemplate = templates.groupFooterTemplate, groupItems = group.items;
        //    if (template) {
        //        text = typeof template === FUNCTION ? template(data) : kendo.template(template)(data);
        //    }
        //    html += groupHeaderBuilder(colspan, level, text);
        //    if (group.hasSubgroups) {
        //        for (idx = 0, length = groupItems.length; idx < length; idx++) {
        //            html += that._groupRowHtml(groupItems[idx], skipColspan ? colspan : colspan - 1, level + 1, groupHeaderBuilder, templates, skipColspan);
        //        }
        //    } else {
        //        html += that._rowsHtml(groupItems, templates);
        //    }
        //    if (groupFooterTemplate) {
        //        html += groupFooterTemplate(aggregates);
        //    }
        //    return html;
        //}
        //_createColumnsLayout: function (resources, inner) {
        //    var that = this;
        //    return customCreateLayoutConfiguration("columns", resources, inner, that);
        //},
        //_createRowsLayout: function (resources, inner) {
        //    var that = this;
        //    return customCreateLayoutConfiguration("rows", resources, inner, that);
        //},
    };
    
    var SchedulerTimelineWeekView = SchedulerTimelineView.extend(timeLineOptions);

    extend(true, ui, {
        SchedulerTimelineWeekView: SchedulerTimelineWeekView
    });
     
}