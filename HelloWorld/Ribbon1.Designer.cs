namespace HelloWorld
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.groupHello = this.Factory.CreateRibbonGroup();
            this.menu1 = this.Factory.CreateRibbonMenu();
            this.btnHello = this.Factory.CreateRibbonButton();
            this.groupMathUtils = this.Factory.CreateRibbonGroup();
            this.btnConvert = this.Factory.CreateRibbonButton();
            this.btnPrimeFactors = this.Factory.CreateRibbonButton();
            this.btnOnes = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.groupHello.SuspendLayout();
            this.groupMathUtils.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.groupHello);
            this.tab1.Groups.Add(this.groupMathUtils);
            this.tab1.Label = "Hello";
            this.tab1.Name = "tab1";
            // 
            // groupHello
            // 
            this.groupHello.Items.Add(this.menu1);
            this.groupHello.Items.Add(this.btnOnes);
            this.groupHello.Label = "Hello";
            this.groupHello.Name = "groupHello";
            // 
            // menu1
            // 
            this.menu1.Items.Add(this.btnHello);
            this.menu1.Label = "Hello Menu";
            this.menu1.Name = "menu1";
            // 
            // btnHello
            // 
            this.btnHello.Label = "Hello World";
            this.btnHello.Name = "btnHello";
            this.btnHello.ShowImage = true;
            this.btnHello.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnHello_Click);
            // 
            // groupMathUtils
            // 
            this.groupMathUtils.Items.Add(this.btnConvert);
            this.groupMathUtils.Items.Add(this.btnPrimeFactors);
            this.groupMathUtils.Label = "Math Utilities";
            this.groupMathUtils.Name = "groupMathUtils";
            // 
            // btnConvert
            // 
            this.btnConvert.Label = "Convert to Number";
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnConvert_Click);
            // 
            // btnPrimeFactors
            // 
            this.btnPrimeFactors.Label = "Get Prime Factors";
            this.btnPrimeFactors.Name = "btnPrimeFactors";
            this.btnPrimeFactors.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPrimeFactors_Click);
            // 
            // btnOnes
            // 
            this.btnOnes.Label = "Ones!!";
            this.btnOnes.Name = "btnOnes";
            this.btnOnes.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOnes_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.groupHello.ResumeLayout(false);
            this.groupHello.PerformLayout();
            this.groupMathUtils.ResumeLayout(false);
            this.groupMathUtils.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupHello;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnHello;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu menu1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConvert;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnPrimeFactors;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupMathUtils;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOnes;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
