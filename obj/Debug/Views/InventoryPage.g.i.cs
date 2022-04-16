﻿#pragma checksum "..\..\..\Views\InventoryPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "19F82156C1CD975996B5472B44EB1D6B92F5EF3C073EAC52C50EB8908C7BECDB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome5;
using FontAwesome5.Converters;
using ItalianPizza.Views;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ItalianPizza.Views {
    
    
    /// <summary>
    /// InventoryPage
    /// </summary>
    public partial class InventoryPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border InitialMessageBorder;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid InventoryTableGrid;
        
        #line default
        #line hidden
        
        
        #line 239 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FontAwesome5.ImageAwesome FilterImageAwesome;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border SecondLayerFilterBorder;
        
        #line default
        #line hidden
        
        
        #line 253 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border FirstLayerFilterBorder;
        
        #line default
        #line hidden
        
        
        #line 255 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel FiltersStackPanel;
        
        #line default
        #line hidden
        
        
        #line 332 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border ThirdLayerInformationBorder;
        
        #line default
        #line hidden
        
        
        #line 335 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border QuarterLayerInformationBorder;
        
        #line default
        #line hidden
        
        
        #line 337 "..\..\..\Views\InventoryPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid OrderInformationGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ItalianPizza;component/views/inventorypage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\InventoryPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.InitialMessageBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.InventoryTableGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            
            #line 230 "..\..\..\Views\InventoryPage.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.ShowSearchResults);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 236 "..\..\..\Views\InventoryPage.xaml"
            ((System.Windows.Controls.StackPanel)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.ShowFilters);
            
            #line default
            #line hidden
            return;
            case 5:
            this.FilterImageAwesome = ((FontAwesome5.ImageAwesome)(target));
            return;
            case 6:
            this.SecondLayerFilterBorder = ((System.Windows.Controls.Border)(target));
            
            #line 251 "..\..\..\Views\InventoryPage.xaml"
            this.SecondLayerFilterBorder.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.HideFilters);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FirstLayerFilterBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 8:
            this.FiltersStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            this.ThirdLayerInformationBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 10:
            this.QuarterLayerInformationBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 11:
            this.OrderInformationGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 12:
            
            #line 341 "..\..\..\Views\InventoryPage.xaml"
            ((System.Windows.Controls.StackPanel)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.HideSpecificOrderInformation);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

