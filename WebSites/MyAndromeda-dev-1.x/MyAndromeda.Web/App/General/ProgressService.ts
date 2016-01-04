
module MyAndromeda.Services {
    var m = angular.module("MyAndromeda.Progress", []);

    export class ProgressService {
        public $element: HTMLElement;
        constructor() { }

        public Create($element: HTMLElement): ProgressService {
            this.$element = $element;
            return this;
        }

        public Show(): void {
            kendo.ui.progress($(this.$element), true);
        }

        public ShowProgress($element): void {
            kendo.ui.progress($($element), true);
        }

        public Hide(): void {
            kendo.ui.progress($(this.$element), false);
        }

        public HideProgress($element): void {
            kendo.ui.progress($($element), false);
        }
    }

    m.factory("progressService", () => {
        return new ProgressService();
    });
}