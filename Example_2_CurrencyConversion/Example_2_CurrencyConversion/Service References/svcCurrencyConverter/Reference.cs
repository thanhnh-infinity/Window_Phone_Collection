﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace Example_2_CurrencyConversion.svcCurrencyConverter {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.webserviceX.NET/", ConfigurationName="svcCurrencyConverter.CurrencyConvertorSoap")]
    public interface CurrencyConvertorSoap {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://www.webserviceX.NET/ConversionRate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.IAsyncResult BeginConversionRate(Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency, Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency, System.AsyncCallback callback, object asyncState);
        
        double EndConversionRate(System.IAsyncResult result);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.webserviceX.NET/")]
    public enum Currency {
        
        /// <remarks/>
        AFA,
        
        /// <remarks/>
        ALL,
        
        /// <remarks/>
        DZD,
        
        /// <remarks/>
        ARS,
        
        /// <remarks/>
        AWG,
        
        /// <remarks/>
        AUD,
        
        /// <remarks/>
        BSD,
        
        /// <remarks/>
        BHD,
        
        /// <remarks/>
        BDT,
        
        /// <remarks/>
        BBD,
        
        /// <remarks/>
        BZD,
        
        /// <remarks/>
        BMD,
        
        /// <remarks/>
        BTN,
        
        /// <remarks/>
        BOB,
        
        /// <remarks/>
        BWP,
        
        /// <remarks/>
        BRL,
        
        /// <remarks/>
        GBP,
        
        /// <remarks/>
        BND,
        
        /// <remarks/>
        BIF,
        
        /// <remarks/>
        XOF,
        
        /// <remarks/>
        XAF,
        
        /// <remarks/>
        KHR,
        
        /// <remarks/>
        CAD,
        
        /// <remarks/>
        CVE,
        
        /// <remarks/>
        KYD,
        
        /// <remarks/>
        CLP,
        
        /// <remarks/>
        CNY,
        
        /// <remarks/>
        COP,
        
        /// <remarks/>
        KMF,
        
        /// <remarks/>
        CRC,
        
        /// <remarks/>
        HRK,
        
        /// <remarks/>
        CUP,
        
        /// <remarks/>
        CYP,
        
        /// <remarks/>
        CZK,
        
        /// <remarks/>
        DKK,
        
        /// <remarks/>
        DJF,
        
        /// <remarks/>
        DOP,
        
        /// <remarks/>
        XCD,
        
        /// <remarks/>
        EGP,
        
        /// <remarks/>
        SVC,
        
        /// <remarks/>
        EEK,
        
        /// <remarks/>
        ETB,
        
        /// <remarks/>
        EUR,
        
        /// <remarks/>
        FKP,
        
        /// <remarks/>
        GMD,
        
        /// <remarks/>
        GHC,
        
        /// <remarks/>
        GIP,
        
        /// <remarks/>
        XAU,
        
        /// <remarks/>
        GTQ,
        
        /// <remarks/>
        GNF,
        
        /// <remarks/>
        GYD,
        
        /// <remarks/>
        HTG,
        
        /// <remarks/>
        HNL,
        
        /// <remarks/>
        HKD,
        
        /// <remarks/>
        HUF,
        
        /// <remarks/>
        ISK,
        
        /// <remarks/>
        INR,
        
        /// <remarks/>
        IDR,
        
        /// <remarks/>
        IQD,
        
        /// <remarks/>
        ILS,
        
        /// <remarks/>
        JMD,
        
        /// <remarks/>
        JPY,
        
        /// <remarks/>
        JOD,
        
        /// <remarks/>
        KZT,
        
        /// <remarks/>
        KES,
        
        /// <remarks/>
        KRW,
        
        /// <remarks/>
        KWD,
        
        /// <remarks/>
        LAK,
        
        /// <remarks/>
        LVL,
        
        /// <remarks/>
        LBP,
        
        /// <remarks/>
        LSL,
        
        /// <remarks/>
        LRD,
        
        /// <remarks/>
        LYD,
        
        /// <remarks/>
        LTL,
        
        /// <remarks/>
        MOP,
        
        /// <remarks/>
        MKD,
        
        /// <remarks/>
        MGF,
        
        /// <remarks/>
        MWK,
        
        /// <remarks/>
        MYR,
        
        /// <remarks/>
        MVR,
        
        /// <remarks/>
        MTL,
        
        /// <remarks/>
        MRO,
        
        /// <remarks/>
        MUR,
        
        /// <remarks/>
        MXN,
        
        /// <remarks/>
        MDL,
        
        /// <remarks/>
        MNT,
        
        /// <remarks/>
        MAD,
        
        /// <remarks/>
        MZM,
        
        /// <remarks/>
        MMK,
        
        /// <remarks/>
        NAD,
        
        /// <remarks/>
        NPR,
        
        /// <remarks/>
        ANG,
        
        /// <remarks/>
        NZD,
        
        /// <remarks/>
        NIO,
        
        /// <remarks/>
        NGN,
        
        /// <remarks/>
        KPW,
        
        /// <remarks/>
        NOK,
        
        /// <remarks/>
        OMR,
        
        /// <remarks/>
        XPF,
        
        /// <remarks/>
        PKR,
        
        /// <remarks/>
        XPD,
        
        /// <remarks/>
        PAB,
        
        /// <remarks/>
        PGK,
        
        /// <remarks/>
        PYG,
        
        /// <remarks/>
        PEN,
        
        /// <remarks/>
        PHP,
        
        /// <remarks/>
        XPT,
        
        /// <remarks/>
        PLN,
        
        /// <remarks/>
        QAR,
        
        /// <remarks/>
        ROL,
        
        /// <remarks/>
        RUB,
        
        /// <remarks/>
        WST,
        
        /// <remarks/>
        STD,
        
        /// <remarks/>
        SAR,
        
        /// <remarks/>
        SCR,
        
        /// <remarks/>
        SLL,
        
        /// <remarks/>
        XAG,
        
        /// <remarks/>
        SGD,
        
        /// <remarks/>
        SKK,
        
        /// <remarks/>
        SIT,
        
        /// <remarks/>
        SBD,
        
        /// <remarks/>
        SOS,
        
        /// <remarks/>
        ZAR,
        
        /// <remarks/>
        LKR,
        
        /// <remarks/>
        SHP,
        
        /// <remarks/>
        SDD,
        
        /// <remarks/>
        SRG,
        
        /// <remarks/>
        SZL,
        
        /// <remarks/>
        SEK,
        
        /// <remarks/>
        CHF,
        
        /// <remarks/>
        SYP,
        
        /// <remarks/>
        TWD,
        
        /// <remarks/>
        TZS,
        
        /// <remarks/>
        THB,
        
        /// <remarks/>
        TOP,
        
        /// <remarks/>
        TTD,
        
        /// <remarks/>
        TND,
        
        /// <remarks/>
        TRL,
        
        /// <remarks/>
        USD,
        
        /// <remarks/>
        AED,
        
        /// <remarks/>
        UGX,
        
        /// <remarks/>
        UAH,
        
        /// <remarks/>
        UYU,
        
        /// <remarks/>
        VUV,
        
        /// <remarks/>
        VEB,
        
        /// <remarks/>
        VND,
        
        /// <remarks/>
        YER,
        
        /// <remarks/>
        YUM,
        
        /// <remarks/>
        ZMK,
        
        /// <remarks/>
        ZWD,
        
        /// <remarks/>
        TRY,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CurrencyConvertorSoapChannel : Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConversionRateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public ConversionRateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public double Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((double)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CurrencyConvertorSoapClient : System.ServiceModel.ClientBase<Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap>, Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap {
        
        private BeginOperationDelegate onBeginConversionRateDelegate;
        
        private EndOperationDelegate onEndConversionRateDelegate;
        
        private System.Threading.SendOrPostCallback onConversionRateCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public CurrencyConvertorSoapClient() {
        }
        
        public CurrencyConvertorSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CurrencyConvertorSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CurrencyConvertorSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CurrencyConvertorSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<ConversionRateCompletedEventArgs> ConversionRateCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap.BeginConversionRate(Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency, Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginConversionRate(FromCurrency, ToCurrency, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        double Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap.EndConversionRate(System.IAsyncResult result) {
            return base.Channel.EndConversionRate(result);
        }
        
        private System.IAsyncResult OnBeginConversionRate(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency = ((Example_2_CurrencyConversion.svcCurrencyConverter.Currency)(inValues[0]));
            Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency = ((Example_2_CurrencyConversion.svcCurrencyConverter.Currency)(inValues[1]));
            return ((Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap)(this)).BeginConversionRate(FromCurrency, ToCurrency, callback, asyncState);
        }
        
        private object[] OnEndConversionRate(System.IAsyncResult result) {
            double retVal = ((Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap)(this)).EndConversionRate(result);
            return new object[] {
                    retVal};
        }
        
        private void OnConversionRateCompleted(object state) {
            if ((this.ConversionRateCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.ConversionRateCompleted(this, new ConversionRateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void ConversionRateAsync(Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency, Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency) {
            this.ConversionRateAsync(FromCurrency, ToCurrency, null);
        }
        
        public void ConversionRateAsync(Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency, Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency, object userState) {
            if ((this.onBeginConversionRateDelegate == null)) {
                this.onBeginConversionRateDelegate = new BeginOperationDelegate(this.OnBeginConversionRate);
            }
            if ((this.onEndConversionRateDelegate == null)) {
                this.onEndConversionRateDelegate = new EndOperationDelegate(this.OnEndConversionRate);
            }
            if ((this.onConversionRateCompletedDelegate == null)) {
                this.onConversionRateCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnConversionRateCompleted);
            }
            base.InvokeAsync(this.onBeginConversionRateDelegate, new object[] {
                        FromCurrency,
                        ToCurrency}, this.onEndConversionRateDelegate, this.onConversionRateCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap CreateChannel() {
            return new CurrencyConvertorSoapClientChannel(this);
        }
        
        private class CurrencyConvertorSoapClientChannel : ChannelBase<Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap>, Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap {
            
            public CurrencyConvertorSoapClientChannel(System.ServiceModel.ClientBase<Example_2_CurrencyConversion.svcCurrencyConverter.CurrencyConvertorSoap> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginConversionRate(Example_2_CurrencyConversion.svcCurrencyConverter.Currency FromCurrency, Example_2_CurrencyConversion.svcCurrencyConverter.Currency ToCurrency, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = FromCurrency;
                _args[1] = ToCurrency;
                System.IAsyncResult _result = base.BeginInvoke("ConversionRate", _args, callback, asyncState);
                return _result;
            }
            
            public double EndConversionRate(System.IAsyncResult result) {
                object[] _args = new object[0];
                double _result = ((double)(base.EndInvoke("ConversionRate", _args, result)));
                return _result;
            }
        }
    }
}
