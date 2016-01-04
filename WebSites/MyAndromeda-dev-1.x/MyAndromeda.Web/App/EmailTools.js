var MyAndromeda;
(function (MyAndromeda) {
    var KendoExtensions;
    (function (KendoExtensions) {
        var EditorTools;
        (function (EditorTools) {
            var emailEditorService = (function () {
                //public static editorOptions = {
                //    tools: [
                //        "bold", "italic", "underline",
                //        "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                //        "insertUnorderedList", "insertOrderedList",
                //        "indent", "outdent",
                //        "createLink", "unlink",
                //        {
                //            name: "tokenTool",
                //            tooltip: "Add tokens",
                //            template  
                //        }
                //    ]
                //};
                function emailEditorService(options) {
                    this.options = options;
                    this.element = $(options.elementId);
                    //document.getElementById(options.elementId);
                    this.fieldsTemplate = $(options.fieldsTemplateId);
                    //document.getElementById(options.fieldsTemplateId);
                }
                //confirm all the elements are set
                emailEditorService.prototype.checkIntegrity = function () {
                    if (!this.element) {
                        throw new Error("The element for the editor is not known");
                    }
                    if (!this.fieldsTemplate) {
                        throw new Error("the template for the field selection is not known");
                    }
                    if (!this.options.editorOptions) {
                        throw new Error("the editor options are not set");
                    }
                };
                emailEditorService.prototype.eventChangeTokenSelected = function (dropwDown, e) {
                    var dataItem = dropwDown.dataItem(e.item.index());
                    this.currentTemplate = dataItem.value;
                };
                emailEditorService.prototype.setupFieldsTemplate = function () {
                    var internal = this;
                    var dropDown = $("#ToolsInsertToken").kendoDropDownList({
                        dataSource: this.options.tokenOptions,
                        dataTextField: "text",
                        dataValueField: "value",
                        optionLabel: "Select"
                    }).data("kendoDropDownList");
                    dropDown.bind("select", function (e) {
                        internal.eventChangeTokenSelected(dropDown, e);
                        //$.proxy(internal, "eventChangeTokenSelected", [dropDown, e]);
                    });
                };
                emailEditorService.prototype.insertTemplate = function () {
                    var template = this.currentTemplate + "&nbsp;";
                    if (!template) {
                        alert("Please select a token first");
                    }
                    this.getEditor().paste(template, {});
                };
                emailEditorService.prototype.setupInsertButton = function () {
                    var internal = this;
                    var button = $(".k-insert-token-button").on("click", function (e) {
                        e.preventDefault();
                        internal.insertTemplate();
                    });
                };
                emailEditorService.prototype.manageSelection = function (e) {
                    //var range = this.getEditor().getRange();
                    //var selection = this.getEditor().getSelection();
                    var r = this.getEditor();
                    var a = 0;
                };
                emailEditorService.prototype.setupEditor = function () {
                    var internal = this;
                    var editorElememt = $(this.element);
                    var editor = editorElememt.kendoEditor(this.options.editorOptions).data("kendoEditor");
                    editor.bind("select", function (e) {
                        internal.manageSelection(e);
                    });
                };
                emailEditorService.prototype.init = function () {
                    //validate internal
                    this.checkIntegrity();
                    this.setupEditor();
                    this.setupFieldsTemplate();
                    this.setupInsertButton();
                };
                emailEditorService.prototype.getEditor = function () {
                    return $(this.element).data("kendoEditor");
                };
                emailEditorService.prototype.selected = function () {
                    return this.getEditor().selectedHtml();
                };
                //about range http://www.quirksmode.org/dom/range_intro.html
                emailEditorService.prototype.selectedRange = function () {
                    return this.getEditor().getRange();
                };
                return emailEditorService;
            })();
            EditorTools.emailEditorService = emailEditorService;
        })(EditorTools = KendoExtensions.EditorTools || (KendoExtensions.EditorTools = {}));
    })(KendoExtensions = MyAndromeda.KendoExtensions || (MyAndromeda.KendoExtensions = {}));
})(MyAndromeda || (MyAndromeda = {}));
