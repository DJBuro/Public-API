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

    /**
     * ***********************************************************
     * Month view
     * ***********************************************************
     */
    
    const NUMBER_OF_COLUMNS = 7,
        NUMBER_OF_ROWS = 1,
        MORE_BUTTON_TEMPLATE = kendo.template(`<div style="width:#=width#px;left:#=left#px;top:#=top#px;height:20px" class="k-more-events k-button">
    <span style="height:20px; margin-top:0" class="label label-success">#=count#...</span>
</div>`);

    let monthViewOptions: any = {
        //calculateDateRange: function () {
        //    var selectedDate = this.options.date,
        //        start = selectedDate,
        //        dates = [];

        //    for (let index = 0, length = 7; index < length; index++) {
        //        dates.push(start);
        //        start = kDate.nextDay(start);
        //    }

        //    this._render(dates);
        //},
        _createRow: function (startDate, startIdx, cellsPerRow, groupIndex) {
            var that = this;
            var min = that._firstDayOfMonth;
            var max = that._lastDayOfMonth;
            var content = that.dayTemplate;
            var classes = '';
            var html = '';
            var resources = function () {
                return that._resourceBySlot({ groupIndex: groupIndex });
            };
            for (var cellIdx = 0; cellIdx < cellsPerRow; cellIdx++) {
                classes = '';
                if (kDate.isToday(startDate)) {
                    classes += 'k-today';
                }
                if (!kDate.isInDateRange(startDate, min, max)) {
                    classes += ' k-other-month';
                }
                html += '<td ';
                if (classes !== '') {
                    html += 'class="' + classes + '"';
                }
                html += '>';
                html += content({
                    date: startDate,
                    resources: resources
                });
                html += '</td>';
                that._slotIndices[kDate.getDate(startDate).getTime()] = startIdx + cellIdx;
                startDate = kDate.nextDay(startDate);
            }
            return html;
        },
        _createCalendar: function (verticalGroupIndex) {
            var start = this.startDate();
            var cellCount = NUMBER_OF_COLUMNS * NUMBER_OF_ROWS;
            var cellsPerRow = NUMBER_OF_COLUMNS;
            var weekStartDates = [start];
            var html = '';
            var horizontalGroupCount = 1;
            var isVerticallyGrouped = this._isVerticallyGrouped();
            var resources = this.groupedResources;
            if (resources.length) {
                if (!isVerticallyGrouped) {
                    horizontalGroupCount = this._columnCountForLevel(resources.length - 1);
                }
            }

            this._slotIndices = {};

            for (var rowIdx = 0, length = 1; rowIdx < length; rowIdx++) {
                html += '<tr>';
                weekStartDates.push(start);
                var startIdx = rowIdx * cellsPerRow;
                for (var groupIdx = 0; groupIdx < horizontalGroupCount; groupIdx++) {
                    
                    html += this._createRow(start, startIdx, cellsPerRow, isVerticallyGrouped ? verticalGroupIndex : groupIdx);
                }
                start = kDate.addDays(start, cellsPerRow);
                html += '</tr>';
            }
            this._weekStartDates = weekStartDates;
            this._endDate = kDate.previousDay(start);

            return html;
        },
        _layout: function () {
            var calendarInfo = this.calendarInfo();
            var weekDayNames = this._isMobile() ? calendarInfo.days.namesShort : calendarInfo.days.names;

            var names = shiftArray(weekDayNames, calendarInfo.firstDay);
            //var columns = $.map(names, function (value) {
            //    return { text: value };
            //});
            let columns = [];
            let columnDate = this.startDate();
            for (let i = 0; i < 7; i++){
                let m = kendo.toString(columnDate, 'm'),
                    d = kendo.toString(columnDate, 'ddd');
                    
                columns.push({
                    text: d + ", " + m
                });

                columnDate = kDate.nextDay(columnDate);
            }

            var resources = this.groupedResources;
            Logger.Notify("Grouped resources:");
            Logger.Notify(resources);

            var rows;
            if (resources.length) {
                if (this._isVerticallyGrouped()) {
                    var inner = [];
                    for (var idx = 0; idx < 1; idx++) {
                        inner.push({
                            text: '<div>&nbsp;</div>',
                            className: 'k-hidden k-slot-cell'
                        });
                    }
                    rows = this._createRowsLayout(resources, inner, this.groupHeaderTemplate);
                    console.log("rows: ");
                    console.log(rows);
                } else {
                    columns = this._createColumnsLayout(resources, columns, this.groupHeaderTemplate);
                }
            }
            return {
                columns: columns,
                rows: rows
            };
        },
        _createRowsLayout: function (resources, inner, template) {
            return createLayoutConfiguration('rows', resources, inner, template);
        },
        _positionEvent: function (slotRange, element, group) {
            var eventHeight = this.options.eventHeight;
            var startSlot = slotRange.start;
            if (slotRange.start.offsetLeft > slotRange.end.offsetLeft) {
                startSlot = slotRange.end;
            }
            var startIndex = slotRange.start.index;
            var endIndex = slotRange.end.index;
            var eventCount = startSlot.eventCount;
            var events = SchedulerView.collidingEvents(slotRange.events(), startIndex, endIndex);
            var rightOffset = startIndex !== endIndex ? 5 : 4;
            events.push({
                element: element,
                start: startIndex,
                end: endIndex
            });
            var rows = SchedulerView.createRows(events);
            for (var idx = 0, length = Math.min(rows.length, eventCount); idx < length; idx++) {
                var rowEvents = rows[idx].events;
                var eventTop = startSlot.offsetTop + startSlot.firstChildHeight + idx * eventHeight + 3 * idx + 'px';
                for (var j = 0, eventLength = rowEvents.length; j < eventLength; j++) {
                    rowEvents[j].element[0].style.top = eventTop;
                }
            }
            if (rows.length > eventCount) {
                for (var slotIndex = startIndex; slotIndex <= endIndex; slotIndex++) {
                    var collection = slotRange.collection;
                    var slot = collection.at(slotIndex);
                    if (slot.more) {
                        return;
                    }

                    slot.more = $(MORE_BUTTON_TEMPLATE({
                        ns: kendo.ns,
                        start: slotIndex,
                        end: slotIndex,
                        width: slot.clientWidth - 2,
                        left: slot.offsetLeft + 2,
                        top: slot.offsetTop + slot.firstChildHeight + eventCount * eventHeight + 3 * eventCount,
                        count: rows.length
                    }));

                    this.content[0].appendChild(slot.more[0]);
                }
            } else {
                slotRange.addEvent({
                    element: element,
                    start: startIndex,
                    end: endIndex,
                    groupIndex: startSlot.groupIndex
                });
                element[0].style.width = slotRange.innerWidth() - rightOffset + 'px';
                element[0].style.left = startSlot.offsetLeft + 2 + 'px';
                element[0].style.height = eventHeight + 'px';
                group._continuousEvents.push({
                    element: element,
                    uid: element.attr(kAttr('uid')),
                    start: slotRange.start,
                    end: slotRange.end
                });
                element.appendTo(this.content);
            }
        },
        _renderLayout: function (date) {
            var that = this;
            this._firstDayOfMonth = date;
            this._lastDayOfMonth = kDate.addDays(date, 7);
            this._startDate = date;
            this._endDate = kDate.addDays(date, 7);
            
            this.createLayout(this._layout());
            this._content();
            this.refreshLayout();
            this.content.on('click' + NS, '.k-nav-day,.k-more-events', function (e) {
                var offset = $(e.currentTarget).offset();
                var slot = that._slotByPosition(offset.left, offset.top);
                e.preventDefault();
                //alert("popup :)");

                //console.log(slot);
                //let slotElement = slot.element;
                //var popover = $(slotElement).popover({
                //    title: "Tasks",
                //    placement: "auto",
                //    html: true,
                //    content: "please wait"
                //}).on("show.bs.popover", function () {
                //    console.log("show popover");
                //    let html = $(slotElement).contents().html();
                //    popover.attr('data-content', html);
                //    var current = $(this);
                //    setTimeout(() => { current.popover('hide'); }, 5000);
                //});
                //popover.popover("show");

                that.trigger('navigate', {
                    view: 'day',
                    date: slot.startDate()
                });
            });
        },
        _renderEvents: function (events, groupIndex) {
            var event;
            var idx;
            var length;
            var isMobilePhoneView = this._isMobilePhoneView();

            //console.log("_renderEvents: per row - event.length" +  events.length);
            for (idx = 0, length = events.length; idx < length; idx++) {

                event = events[idx];
                let dataInSlot = this._isInDateSlot(event);

                if (dataInSlot) {
                    var group = this.groups[groupIndex];
                    if (!group._continuousEvents) {
                        group._continuousEvents = [];
                    }
                    var ranges = group.slotRanges(event, true);
                    var rangeCount = ranges.length;
                    

                    for (var rangeIndex = 0; rangeIndex < rangeCount; rangeIndex++) {
                        var range = ranges[rangeIndex];
                        var start = event.start;
                        var end = event.end;

                        //find max ? 
                        if (rangeCount > 1) {
                            if (rangeIndex === 0) {
                                end = range.end.endDate();
                            } else if (rangeIndex == rangeCount - 1) {
                                start = range.start.startDate();
                            } else {
                                start = range.start.startDate();
                                end = range.end.endDate();
                            }
                        }


                        var occurrence = event.clone({
                            start: start,
                            end: end,
                            head: range.head,
                            tail: range.tail
                        });

                        //find max ? 
                        if (isMobilePhoneView) {
                            this._positionMobileEvent(range, this._createEventElement(occurrence), group);
                        } else {
                            this._positionEvent(range, this._createEventElement(occurrence), group);
                        }
                    }
                }
            }
        },
        _renderGroups: function (events, resources, offset, columnLevel) {
            var resource = resources[0];

            if (resource) {
                var view: Array<MyAndromeda.Hr.Models.IEmployeeTask> = resource.dataSource.view();
                Logger.Notify("resource view");
                Logger.Notify(view);

                /* sort by department */
                view = view.sort((a, b) => {
                    let aValue = a.Department ? a.Department : "NA",
                        bValue = b.Department ? b.Department : "NA";

                    return aValue.length - bValue.length;
                });

                for (var itemIdx = 0; itemIdx < view.length; itemIdx++) {
                    var value = this._resourceValue(resource, view[itemIdx]);

                    //MyAndromeda.Logger.Notify("group operator:");
                    //MyAndromeda.Logger.Notify(SchedulerView.groupEqFilter(value));

                    var tmp = new kData.Query(events).filter({
                        field: resource.field,
                        operator: SchedulerView.groupEqFilter(value)
                    }).toArray();

                    //tmp = tmp.sort((a, b) => {
                    //    let aValue = a.Department, bValue = b.Department;
                    //    return aValue.length - bValue.length;
                    //});
                   
                    if (resources.length > 1) {
                        offset = this._renderGroups(tmp, resources.slice(1), offset++, columnLevel + 1);
                    } else {
                        this._renderEvents(tmp, offset++);
                    }
                }
            }
            return offset;
        },
        render: function (events) {
            this.content.children('.k-event,.k-more-events,.k-events-container').remove();
            this._groups();
            events = new kData.Query(events).sort([
                {
                    field: 'start',
                    dir: 'asc'
                },
                {
                    field: 'end',
                    dir: 'desc'
                }
            ]).toArray();
            var resources = this.groupedResources;
            if (resources.length) {
                this._renderGroups(events, resources, 0, 1);
            } else {
                this._renderEvents(events, 0);
            }
            this.refreshLayout();
            this.trigger('activate');
        },
        //_addResourceView: function () {
        //    var resourceView = new ResourceView(this.groups.length, this._isRtl);
        //    this.groups.push(resourceView);
        //    return resourceView;
        //},
        _groups: function () {
            var groupCount = this._groupCount();
            var columnCount = NUMBER_OF_COLUMNS;
            var rowCount = NUMBER_OF_ROWS;
            this.groups = [];

            for (var idx = 0; idx < groupCount; idx++) {
                this._addResourceView(idx);
            }

            var tableRows = this.content[0].getElementsByTagName('tr');
            var startDate = this.startDate();

            for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
                var cellCount = 0;
                var rowMultiplier = 0;

                if (this._isVerticallyGrouped()) {
                    rowMultiplier = groupIndex;
                }

                for (var rowIndex = rowMultiplier * rowCount; rowIndex < (rowMultiplier + 1) * rowCount; rowIndex++)
                {

                    var group = this.groups[groupIndex];
                    var collection = group.addDaySlotCollection(kDate.addDays(startDate, cellCount), kDate.addDays(this.startDate(), cellCount + columnCount));
                    var tableRow = tableRows[rowIndex];
                    var cells = tableRow.children;
                    var cellMultiplier = 0;

                    Logger.Notify("group: ");
                    Logger.Notify(group);
                    Logger.Notify("collection: " + collection);
                    Logger.Notify(collection);

                    tableRow.setAttribute('role', 'row');

                    if (!this._isVerticallyGrouped())
                    {
                        cellMultiplier = groupIndex;
                    }

                    for (var cellIndex = cellMultiplier * columnCount; cellIndex < (cellMultiplier + 1) * columnCount; cellIndex++) {

                        var cell = cells[cellIndex];
                        var clientHeight = cell.clientHeight;
                        var firstChildHeight = cell.children.length ? cell.children[0].offsetHeight + 3 : 0;
                        var start = kDate.addDays(startDate, cellCount);
                        var end = kDate.MS_PER_DAY;

                        if (startDate.getHours() !== start.getHours()) {
                            end += (startDate.getHours() - start.getHours()) * kDate.MS_PER_HOUR;
                        }

                        start = kDate.toUtcTime(start);
                        end += start;
                        cellCount++;

                        //var eventCount = 20; 
                        var overflows = Math.floor((clientHeight - firstChildHeight - this.options.moreButtonHeight) / (this.options.eventHeight + 3));

                        //console.log("show events: " + eventCount);
                        //let majorColour = "#fff";
                        //let minorColour = "#465298";

                        //minorColour = ""; 

                        //let background = "repeating-linear-gradient(90deg, {0}, {0} 10px, {1} 10px, {1} 20px)";
                        //background = kendo.format(background, majorColour, minorColour);
                        //$(cell).css({
                        //    "background": background
                        //});
                        cell.setAttribute('role', 'gridcell');
                        cell.setAttribute('aria-selected', false);
                        collection.addDaySlot(cell, start, end, overflows);
                    }
                }
            }
        }
            
    };

    var SchedulerTimelineWeekView = SchedulerTimelineView.extend(timeLineOptions);
    var SchedulerMonthWeekView = SchedulerMonthView.extend(monthViewOptions);

    MyAndromeda.Logger.Notify("Defining SchedulerTimelineWeekView");

    extend(true, ui, {
        SchedulerTimelineWeekView: SchedulerTimelineWeekView,
        MonthTimeWeekView: SchedulerMonthWeekView
    });
     
}