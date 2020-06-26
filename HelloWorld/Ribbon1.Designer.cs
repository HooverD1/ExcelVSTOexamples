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
            this.btnCopyFormats = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.button2 = this.Factory.CreateRibbonButton();
            this.groupMathUtils = this.Factory.CreateRibbonGroup();
            this.btnAddFormulas = this.Factory.CreateRibbonButton();
            this.btnWorksheetFunction = this.Factory.CreateRibbonButton();
            this.btnTest = this.Factory.CreateRibbonButton();
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
            this.groupHello.Items.Add(this.btnCopyFormats);
            this.groupHello.Items.Add(this.button1);
            this.groupHello.Items.Add(this.button2);
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
            // btnCopyFormats
            // 
            this.btnCopyFormats.Label = "Copy Format";
            this.btnCopyFormats.Name = "btnCopyFormats";
            this.btnCopyFormats.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCopyFormats_Click);
            // 
            // button1
            // 
            this.button1.Label = "button1";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Label = "button2";
            this.button2.Name = "button2";
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // groupMathUtils
            // 
            this.groupMathUtils.Items.Add(this.btnAddFormulas);
            this.groupMathUtils.Items.Add(this.btnWorksheetFunction);
            this.groupMathUtils.Items.Add(this.btnTest);
            this.groupMathUtils.Label = "Math Utilities";
            this.groupMathUtils.Name = "groupMathUtils";
            // 
            // btnAddFormulas
            // 
            this.btnAddFormulas.Label = "Add Formulas";
            this.btnAddFormulas.Name = "btnAddFormulas";
            this.btnAddFormulas.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAddFormulas_Click);
            // 
            // btnWorksheetFunction
            // 
            this.btnWorksheetFunction.Label = "Use WorksheetFunction";
            this.btnWorksheetFunction.Name = "btnWorksheetFunction";
            this.btnWorksheetFunction.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnWorksheetFunction_Click);
            // 
            // btnTest
            // 
            this.btnTest.Label = "newbutton";
            this.btnTest.Name = "btnTest";
            this.btnTest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTest_Click);
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
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupMathUtils;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAddFormulas;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnWorksheetFunction;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCopyFormats;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTest;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
