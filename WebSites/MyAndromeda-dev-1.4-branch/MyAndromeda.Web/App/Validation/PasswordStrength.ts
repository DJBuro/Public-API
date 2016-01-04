module MyAndromeda.Validation {
    export class PasswordValidator {
        public static BannedPasswordMessage = "Banned or invalid password";
        public static EmptyMessage = "Empty";
        public static WeakMessage = "Weak";
        public static GoodMessage = "Good";
        public static StrongMessage = "Strong";

        private options: IPasswordValidatorOptions;
        private progressBar: kendo.ui.ProgressBar;

        constructor(options: IPasswordValidatorOptions)
        {
            this.options = options;
            this.BuildProgress();
            this.SeekEvents();
        }

        private BuildProgress() : void {
            var internal = this;
            this.progressBar = <kendo.ui.ProgressBar> $(this.options.ProgressElement).kendoProgressBar({
                change: internal.OnProgressChange,
                max: 9,
                min: -1,
                value: 0,
                animation: true,
                type: "value"
            }).data("kendoProgressBar");

            this.progressBar.progressStatus.text("Empty");
        }

        private SeekEvents(): void {
            var internal = this;
            $(this.options.InputElement).on("change keyup", function (e) {
                //validation logic?
                var $object = $(this);
                var length = $object.length;
                var password = <string>$object.val();
                var pattern = "^.*(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d\W]).*$",
                    patterns = PasswordValidator.Patterns,
                    banned = PasswordValidator.BannedList;

                var matches = new Array<number>();

                patterns.forEach(item => {
                    matches.push(item.r.test(password) ? item.score : 0);
                });

                var matchCount = 0;
                matches.forEach((value) => {
                    matchCount += value;
                });

                if (banned.indexOf(password.toLowerCase()) > 0)
                {
                    internal.progressBar.value(-1);
                    //internal.progressBar.progressStatus.text(PasswordValidator.BannedPasswordMessage);
                    return;
                }

                internal.progressBar.value(matchCount);
            });
        }

        private OnProgressChange(e: kendo.ui.ProgressBarChangeEvent): void {
            var progressStatus = e.sender.progressStatus,
                progressWrapper = e.sender.progressWrapper;

            progressStatus.css({
                "background-image": "none",
                "border-image": "none"
            });

            if (e.value < 0) {
                progressStatus.text(PasswordValidator.BannedPasswordMessage);
            } else if (e.value < 1) {
                progressStatus.text(PasswordValidator.EmptyMessage);
            } else if (e.value <= 3) {
                progressStatus.text(PasswordValidator.WeakMessage);
                
                progressWrapper.css({
                    "background-color": "#EE9F05",
                    "border-color": "#EE9F05"
                });
            } else if (e.value <= 6) {
                progressStatus.text(PasswordValidator.GoodMessage);

                progressWrapper.css({
                    "background-color": "#428bca",
                    "border-color": "#428bca"
                });
            } else {
                progressStatus.text(PasswordValidator.StrongMessage);

                progressWrapper.css({
                    "background-color": "#8EBC00",
                    "border-color": "#8EBC00"
                });
            }
        }

        public static Patterns = [
            {
                name: "length1",
                r: new RegExp("(.{8,})"),
                score: 2
            },
            {
                name: "length2",
                r: new RegExp("(.{12,})"),
                score: 2
            },
            {
                name: "length3",
                r: new RegExp("(.{16,})"),
                score: 3
            },
            {
                name: "lowercasePattern",
                r: new RegExp("[a-z]"),
                score: 1
            },
            {
                name: "uppercasePattern",
                r: new RegExp("[A-Z]"),
                score: 1
            },
            {
                name: "digitPattern",
                r: new RegExp("[0-9]"),
                score: 1
            },
            {
                name: "specialCharacter",
                r: new RegExp(".[!@#$%^&*?_~]"),
                score: 2
            }
        ]; 

        public static BannedList = [
            '1234',
            '12345',
            '123456',
            '1234567',
            '12345678',
            '123456789',
            'password',
            'pass',
            'pass1',
            'pass12',
            'pass123',
            'pass1234',
            'iloveyou',
            'princess',
            'rockyou',
            '12345678',
            'abc123',
            'babygirl',
            'monkey',
            'lovely',
            '654321',
            'qwerty',
            'qwerty1234',
            'password1',
            'welcome',
            'welcome1',
            'password2',
            'password01',
            'password3',
            'p@ssw0rd',
            'passw0rd',
            'password4',
            'password123',
            'summer09',
            'password6',
            'password7',
            'password9',
            'password8',
            'welcome2',
            'welcome01',
            'winter12',
            'spring2012',
            'summer',
            'winter',
            'password123456789',
            'password12345678910',
            'password1234567890'
        ];
    }
}

interface IPasswordValidatorOptions {
    InputElement: string;
    ProgressElement: string;
}