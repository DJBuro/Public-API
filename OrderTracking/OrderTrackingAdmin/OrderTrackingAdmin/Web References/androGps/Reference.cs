﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.261.
// 
#pragma warning disable 1591

namespace OrderTrackingAdmin.androGps {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TrackersSoap", Namespace="http://gps.andromedagps.com")]
    public partial class Trackers : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetTrackerByNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTrackersByNamesOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddTrackerOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateTrackerPhoneNumberOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLicenseOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Trackers() {
            this.Url = global::OrderTrackingAdmin.Properties.Settings.Default.OrderTrackingAdmin_androGps_Trackers;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetTrackerByNameCompletedEventHandler GetTrackerByNameCompleted;
        
        /// <remarks/>
        public event GetTrackersByNamesCompletedEventHandler GetTrackersByNamesCompleted;
        
        /// <remarks/>
        public event AddTrackerCompletedEventHandler AddTrackerCompleted;
        
        /// <remarks/>
        public event UpdateTrackerPhoneNumberCompletedEventHandler UpdateTrackerPhoneNumberCompleted;
        
        /// <remarks/>
        public event GetLicenseCompletedEventHandler GetLicenseCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.andromedagps.com/GetTrackerByName", RequestNamespace="http://gps.andromedagps.com", ResponseNamespace="http://gps.andromedagps.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Tracker GetTrackerByName(string trackerName) {
            object[] results = this.Invoke("GetTrackerByName", new object[] {
                        trackerName});
            return ((Tracker)(results[0]));
        }
        
        /// <remarks/>
        public void GetTrackerByNameAsync(string trackerName) {
            this.GetTrackerByNameAsync(trackerName, null);
        }
        
        /// <remarks/>
        public void GetTrackerByNameAsync(string trackerName, object userState) {
            if ((this.GetTrackerByNameOperationCompleted == null)) {
                this.GetTrackerByNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTrackerByNameOperationCompleted);
            }
            this.InvokeAsync("GetTrackerByName", new object[] {
                        trackerName}, this.GetTrackerByNameOperationCompleted, userState);
        }
        
        private void OnGetTrackerByNameOperationCompleted(object arg) {
            if ((this.GetTrackerByNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTrackerByNameCompleted(this, new GetTrackerByNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.andromedagps.com/GetTrackersByNames", RequestNamespace="http://gps.andromedagps.com", ResponseNamespace="http://gps.andromedagps.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Tracker[] GetTrackersByNames(string[] trackerNames) {
            object[] results = this.Invoke("GetTrackersByNames", new object[] {
                        trackerNames});
            return ((Tracker[])(results[0]));
        }
        
        /// <remarks/>
        public void GetTrackersByNamesAsync(string[] trackerNames) {
            this.GetTrackersByNamesAsync(trackerNames, null);
        }
        
        /// <remarks/>
        public void GetTrackersByNamesAsync(string[] trackerNames, object userState) {
            if ((this.GetTrackersByNamesOperationCompleted == null)) {
                this.GetTrackersByNamesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTrackersByNamesOperationCompleted);
            }
            this.InvokeAsync("GetTrackersByNames", new object[] {
                        trackerNames}, this.GetTrackersByNamesOperationCompleted, userState);
        }
        
        private void OnGetTrackersByNamesOperationCompleted(object arg) {
            if ((this.GetTrackersByNamesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTrackersByNamesCompleted(this, new GetTrackersByNamesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.andromedagps.com/AddTracker", RequestNamespace="http://gps.andromedagps.com", ResponseNamespace="http://gps.andromedagps.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool AddTracker(string trackerName, string phoneNumber, int trackerType) {
            object[] results = this.Invoke("AddTracker", new object[] {
                        trackerName,
                        phoneNumber,
                        trackerType});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void AddTrackerAsync(string trackerName, string phoneNumber, int trackerType) {
            this.AddTrackerAsync(trackerName, phoneNumber, trackerType, null);
        }
        
        /// <remarks/>
        public void AddTrackerAsync(string trackerName, string phoneNumber, int trackerType, object userState) {
            if ((this.AddTrackerOperationCompleted == null)) {
                this.AddTrackerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddTrackerOperationCompleted);
            }
            this.InvokeAsync("AddTracker", new object[] {
                        trackerName,
                        phoneNumber,
                        trackerType}, this.AddTrackerOperationCompleted, userState);
        }
        
        private void OnAddTrackerOperationCompleted(object arg) {
            if ((this.AddTrackerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddTrackerCompleted(this, new AddTrackerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.andromedagps.com/UpdateTrackerPhoneNumber", RequestNamespace="http://gps.andromedagps.com", ResponseNamespace="http://gps.andromedagps.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool UpdateTrackerPhoneNumber(string trackerName, string phoneNumber) {
            object[] results = this.Invoke("UpdateTrackerPhoneNumber", new object[] {
                        trackerName,
                        phoneNumber});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateTrackerPhoneNumberAsync(string trackerName, string phoneNumber) {
            this.UpdateTrackerPhoneNumberAsync(trackerName, phoneNumber, null);
        }
        
        /// <remarks/>
        public void UpdateTrackerPhoneNumberAsync(string trackerName, string phoneNumber, object userState) {
            if ((this.UpdateTrackerPhoneNumberOperationCompleted == null)) {
                this.UpdateTrackerPhoneNumberOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateTrackerPhoneNumberOperationCompleted);
            }
            this.InvokeAsync("UpdateTrackerPhoneNumber", new object[] {
                        trackerName,
                        phoneNumber}, this.UpdateTrackerPhoneNumberOperationCompleted, userState);
        }
        
        private void OnUpdateTrackerPhoneNumberOperationCompleted(object arg) {
            if ((this.UpdateTrackerPhoneNumberCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateTrackerPhoneNumberCompleted(this, new UpdateTrackerPhoneNumberCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://gps.andromedagps.com/GetLicense", RequestNamespace="http://gps.andromedagps.com", ResponseNamespace="http://gps.andromedagps.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public License GetLicense() {
            object[] results = this.Invoke("GetLicense", new object[0]);
            return ((License)(results[0]));
        }
        
        /// <remarks/>
        public void GetLicenseAsync() {
            this.GetLicenseAsync(null);
        }
        
        /// <remarks/>
        public void GetLicenseAsync(object userState) {
            if ((this.GetLicenseOperationCompleted == null)) {
                this.GetLicenseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLicenseOperationCompleted);
            }
            this.InvokeAsync("GetLicense", new object[0], this.GetLicenseOperationCompleted, userState);
        }
        
        private void OnGetLicenseOperationCompleted(object arg) {
            if ((this.GetLicenseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLicenseCompleted(this, new GetLicenseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gps.andromedagps.com")]
    public partial class Tracker {
        
        private string nameField;
        
        private System.Nullable<double> longitudeField;
        
        private System.Nullable<double> latitudeField;
        
        private System.Nullable<int> batteryLevelField;
        
        private bool hasFixField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> Longitude {
            get {
                return this.longitudeField;
            }
            set {
                this.longitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> Latitude {
            get {
                return this.latitudeField;
            }
            set {
                this.latitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> BatteryLevel {
            get {
                return this.batteryLevelField;
            }
            set {
                this.batteryLevelField = value;
            }
        }
        
        /// <remarks/>
        public bool HasFix {
            get {
                return this.hasFixField;
            }
            set {
                this.hasFixField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gps.andromedagps.com")]
    public partial class License {
        
        private System.Nullable<int> idField;
        
        private int licensedusersField;
        
        private int registeredDevicesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public int Licensedusers {
            get {
                return this.licensedusersField;
            }
            set {
                this.licensedusersField = value;
            }
        }
        
        /// <remarks/>
        public int RegisteredDevices {
            get {
                return this.registeredDevicesField;
            }
            set {
                this.registeredDevicesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetTrackerByNameCompletedEventHandler(object sender, GetTrackerByNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTrackerByNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTrackerByNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Tracker Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Tracker)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetTrackersByNamesCompletedEventHandler(object sender, GetTrackersByNamesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTrackersByNamesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTrackersByNamesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Tracker[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Tracker[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void AddTrackerCompletedEventHandler(object sender, AddTrackerCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddTrackerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddTrackerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateTrackerPhoneNumberCompletedEventHandler(object sender, UpdateTrackerPhoneNumberCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateTrackerPhoneNumberCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateTrackerPhoneNumberCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetLicenseCompletedEventHandler(object sender, GetLicenseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLicenseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLicenseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public License Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((License)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591