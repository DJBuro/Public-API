/// <reference path="hr.services.ts" />

module MyAndromeda.Hr.Services {

    export class EmployeeAvailabilityTestService {
        constructor(private scheduler: kendo.ui.Scheduler) { }

        private GetTasksInRange(start: Date, end: Date) {
            let occurences: Models.IEmployeeTask[] = this.scheduler.occurrencesInRange(start, end);

            return occurences;
        }

        private CheckTasksByEmployee(start: Date, end: Date, task: Models.IEmployeeTask) {
            var context = {
                start: start,
                end: end,
                task: task
            };

            let startCheck = start.toLocaleTimeString();
            let endCheck = end.toLocaleTimeString();

            //only interested in current employee, which is not the current task
            var currentTasks = this.GetTasksInRange(start, end);
            Logger.Notify("Tasks in range: " + currentTasks.length);

            Logger.Notify(currentTasks);
            currentTasks = currentTasks.filter(e=> e.id !== task.id);
            Logger.Notify("Tasks in range after removing self: " + currentTasks.length);

            currentTasks = currentTasks.filter(e=> e.EmployeeId === task.EmployeeId);
            Logger.Notify("Tasks in range - by employee: " + currentTasks.length);

            Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
            Logger.Notify(context);
            Logger.Notify("Tasks in range: " + currentTasks.length);

            return currentTasks.length === 0;
        }

        public IsDurationValid(start: Date, end: Date): boolean {
            //hours
            let duration = Math.abs(end.getTime() - start.getTime()) / 36e5;

            if (duration < 0.1) {
                return false;
            }

            return true;
        }

        public IsWorkAvailable(start, end, task): boolean {
            return this.CheckTasksByEmployee(start, end, task);
        }

        public IsAllDayValid(task: Models.IEmployeeTask, invalid: (message) => void): boolean {
            const msg = "A shift cannot be all day";
            Logger.Notify("test if all day");

            if (task.isAllDay) {
                Logger.Notify("it is all day - " + task.TaskType);

                let shift = Models.taskValues.Shift;
                let coveringShift = Models.taskValues.CoveringShift;
                
                switch (task.TaskType) {
                    case Models.taskValues.Shift:
                    case Models.taskValues.NeedCover:
                    case Models.taskValues.CoveringShift: {
                        Logger.Notify("invalid");
                        invalid(msg);
                        return false;
                    }
                }
            }
            return true;
        }
    }


}