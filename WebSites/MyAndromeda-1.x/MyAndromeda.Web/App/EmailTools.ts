module MyAndromeda.KendoExtensions.EditorTools {
            
    export class emailEditorService {
        private options: IEditorToolsOptions;
        private element: JQuery;
        private fieldsTemplate: JQuery;

        private currentTemplate: string;
        private editorOptions: kendo.ui.EditorOptions;

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

        constructor(options: IEditorToolsOptions)
        {
            this.options = options;
            this.element = $(options.elementId); 
            //document.getElementById(options.elementId);
            this.fieldsTemplate = $(options.fieldsTemplateId); 
            //document.getElementById(options.fieldsTemplateId);
        }


        //confirm all the elements are set
        private checkIntegrity()
        {
            if (!this.element) { throw new Error("The element for the editor is not known"); }
            if (!this.fieldsTemplate) { throw new Error("the template for the field selection is not known"); }
            if (!this.options.editorOptions) { throw new Error("the editor options are not set"); }
        }

        private eventChangeTokenSelected(dropwDown : kendo.ui.DropDownList, e : any) : void
        {
            var dataItem = dropwDown.dataItem(e.item.index());

            this.currentTemplate = dataItem.value; 
        }

        private setupFieldsTemplate() : void
        {
            var internal = this;
            var dropDown = <kendo.ui.DropDownList>$("#ToolsInsertToken").kendoDropDownList({
                dataSource: this.options.tokenOptions,
                dataTextField: "text",
                dataValueField: "value",
                optionLabel: "Select"
            }).data("kendoDropDownList");

            dropDown.bind("select", function (e) {
                internal.eventChangeTokenSelected(dropDown, e);
                //$.proxy(internal, "eventChangeTokenSelected", [dropDown, e]);
            });
        }

        private insertTemplate(): void {
            var template = this.currentTemplate + "&nbsp;";
            if (!template) { alert("Please select a token first"); }

            this.getEditor().paste(template);
        }

        private setupInsertButton(): void {
            var internal = this;
            var button = $(".k-insert-token-button").on("click", function (e) {
                e.preventDefault();
                internal.insertTemplate();
            }); 
        }

        private manageSelection(e: any): void {
            //var range = this.getEditor().getRange();
            //var selection = this.getEditor().getSelection();
            var r = this.getEditor();
            var a = 0;
        }

        private setupEditor(): void {
            var internal = this;
            var editorElememt = $(this.element);
            var editor = <kendo.ui.Editor>editorElememt.kendoEditor(this.options.editorOptions).data("kendoEditor");

            editor.bind("select", function (e) {
                internal.manageSelection(e);
            });
        }

        public init(): void
        {
            //validate internal
            this.checkIntegrity();
            this.setupEditor();

            this.setupFieldsTemplate();
            this.setupInsertButton();
        }

        public getEditor(): kendo.ui.Editor {
            return <kendo.ui.Editor>$(this.element).data("kendoEditor");
        }

        public selected(): string {
            return this.getEditor().selectedHtml();
        }

        //about range http://www.quirksmode.org/dom/range_intro.html
        public selectedRange(): Range {
            return this.getEditor().getRange();
        }
    } 
}

interface IEditorToolsOptions {
    elementId: string;
    fieldsTemplateId: string;
    editorOptions: kendo.ui.EditorOptions;
    tokenOptions: tokenOption[];
} 

interface tokenOption {
    text: string;
    value: string;
}