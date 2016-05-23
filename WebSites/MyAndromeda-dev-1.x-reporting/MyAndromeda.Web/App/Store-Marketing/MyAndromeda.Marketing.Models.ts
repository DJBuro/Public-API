/// <reference path="myandromeda.marketing.ts" />
module MyAndromeda.Marketing.Models {
    export interface IToken {
        Text: string;
        Value: string;
    }

    export interface IMarketingContact {
        Name: string;
        From: string;
        ReplyTo: string;
        Address: string;
        City: string;
        State: string;
        Zip: string;
        Country: string;
    }
    export interface IMarketingType {
        From: string;
        ReplyTo: string;
        Enabled: boolean;
        EmailContent: string;
        MobileContent: string;

        Preview: string;
    }
    export interface IPreviewSettings {
        To: string;
    }
    export interface IPreviewMarketing {
        Model: IMarketingType,
        Preview: IPreviewSettings
    }

} 