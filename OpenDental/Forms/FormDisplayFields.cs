using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormDisplayFields : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.ODGrid gridMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butDefault;
		private Label label2;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private ListBox listAvailable;
		private Label labelAvailable;
		private OpenDental.UI.Button butRight;
		private OpenDental.UI.Button butLeft;
		private bool changed;
		private OpenDental.UI.Button butOK;
		private Label labelCategory;
		private Label labelCustomField;
		private TextBox textCustomField;
		public DisplayFieldCategory category;
		///<summary>When this form opens, this is the list of display fields that the user has already explicitly set to be showing.  If the user did not set any to be showing yet, then this will start out as the default list.  Except in ortho, where there is no default list.  For non-ortho categories, this is a subset of AvailList.  As this window is used, items are added to this list but not saved until window closes with OK.  For ortho category, items are also added to this list as the window is used.</summary>
		private List<DisplayField> ListShowing;
		///<summary>This is the list of all possible display fields.  If ortho, this list is a combination of current display fields and historical orthochart.FieldNames.</summary>
		private List<DisplayField> AvailList;

		///<summary></summary>
		public FormDisplayFields()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFields));
			this.label2 = new System.Windows.Forms.Label();
			this.listAvailable = new System.Windows.Forms.ListBox();
			this.labelAvailable = new System.Windows.Forms.Label();
			this.labelCategory = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelCustomField = new System.Windows.Forms.Label();
			this.textCustomField = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butDefault = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(111,48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(213,25);
			this.label2.TabIndex = 5;
			this.label2.Text = "Sets entire list to the default.";
			// 
			// listAvailable
			// 
			this.listAvailable.FormattingEnabled = true;
			this.listAvailable.IntegralHeight = false;
			this.listAvailable.Location = new System.Drawing.Point(373,89);
			this.listAvailable.Name = "listAvailable";
			this.listAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listAvailable.Size = new System.Drawing.Size(158,412);
			this.listAvailable.TabIndex = 15;
			this.listAvailable.Click += new System.EventHandler(this.listAvailable_Click);
			// 
			// labelAvailable
			// 
			this.labelAvailable.Location = new System.Drawing.Point(370,69);
			this.labelAvailable.Name = "labelAvailable";
			this.labelAvailable.Size = new System.Drawing.Size(213,17);
			this.labelAvailable.TabIndex = 16;
			this.labelAvailable.Text = "Available Fields";
			this.labelAvailable.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelCategory
			// 
			this.labelCategory.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelCategory.Location = new System.Drawing.Point(12,9);
			this.labelCategory.Name = "labelCategory";
			this.labelCategory.Size = new System.Drawing.Size(213,25);
			this.labelCategory.TabIndex = 57;
			this.labelCategory.Text = "Category Description";
			// 
			// gridMain
			// 
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12,76);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292,425);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = "Fields Showing";
			this.gridMain.TranslationName = "FormDisplayFields";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// labelCustomField
			// 
			this.labelCustomField.Location = new System.Drawing.Point(371,319);
			this.labelCustomField.Name = "labelCustomField";
			this.labelCustomField.Size = new System.Drawing.Size(213,17);
			this.labelCustomField.TabIndex = 58;
			this.labelCustomField.Text = "New Field";
			this.labelCustomField.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textCustomField
			// 
			this.textCustomField.Location = new System.Drawing.Point(373,339);
			this.textCustomField.Name = "textCustomField";
			this.textCustomField.Size = new System.Drawing.Size(158,20);
			this.textCustomField.TabIndex = 59;
			this.textCustomField.Click += new System.EventHandler(this.textCustomField_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(566,474);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 56;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(320,292);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35,24);
			this.butRight.TabIndex = 55;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1,0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(320,252);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35,24);
			this.butLeft.TabIndex = 54;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(109,507);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82,24);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0,1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(12,507);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82,24);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDefault
			// 
			this.butDefault.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDefault.Autosize = true;
			this.butDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefault.CornerRadius = 4F;
			this.butDefault.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDefault.Location = new System.Drawing.Point(12,42);
			this.butDefault.Name = "butDefault";
			this.butDefault.Size = new System.Drawing.Size(91,24);
			this.butDefault.TabIndex = 4;
			this.butDefault.Text = "Set to Default";
			this.butDefault.Click += new System.EventHandler(this.butDefault_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(566,504);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormDisplayFields
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(664,556);
			this.Controls.Add(this.textCustomField);
			this.Controls.Add(this.labelCustomField);
			this.Controls.Add(this.labelCategory);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.labelAvailable);
			this.Controls.Add(this.listAvailable);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDefault);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDisplayFields";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Display Fields";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDisplayFields_FormClosing);
			this.Load += new System.EventHandler(this.FormDisplayFields_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDisplayFields_Load(object sender,EventArgs e) {
			labelCategory.Text=category.ToString();
			textCustomField.Visible=false;
			labelCustomField.Visible=false;
			listAvailable.Height=412;
			DisplayFields.RefreshCache();
			ListShowing=DisplayFields.GetForCategory(category);
			if(category==DisplayFieldCategory.OrthoChart) {
				textCustomField.Visible=true;
				labelCustomField.Visible=true;
				listAvailable.Height=227;//227px for short, 412px for tall
				labelAvailable.Text=Lan.g(this,"Previously Used Fields");
			}
			FillGrids();
		}

		private void FillGrids(){
			AvailList=DisplayFields.GetAllAvailableList(category);//This one needs to be called repeatedly.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormDisplayFields","FieldName"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormDisplayFields","New Descript"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormDisplayFields","Width"),60);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListShowing.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(ListShowing[i].InternalName);
				row.Cells.Add(ListShowing[i].Description);
				row.Cells.Add(ListShowing[i].ColumnWidth.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<ListShowing.Count;i++){
				for(int j=0;j<AvailList.Count;j++) {
					//Only removing one item from AvailList per iteration of i.
					if(category==DisplayFieldCategory.OrthoChart) {
						//OrthoChart category does not use InternalNames.
						if(ListShowing[i].Description==AvailList[j].Description) {
							AvailList.RemoveAt(j);
							break;
						}
					}
					else {
						if(ListShowing[i].InternalName==AvailList[j].InternalName) {
							AvailList.RemoveAt(j);
							break;
						}
					}
				}
			}
			listAvailable.Items.Clear();
			for(int i=0;i<AvailList.Count;i++){
				listAvailable.Items.Add(AvailList[i]);
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDisplayFieldEdit formD=new FormDisplayFieldEdit();
			formD.FieldCur=ListShowing[e.Row];
			formD.ShowDialog();
			FillGrids();
			changed=true;
		}

		private void butDefault_Click(object sender,EventArgs e) {
//todo: if ortho, clear ListShowing.
//No changes needed.
			ListShowing=DisplayFields.GetDefaultList(category);
			FillGrids();
			changed=true;
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(category==DisplayFieldCategory.OrthoChart) {//Ortho Chart
				if(listAvailable.SelectedItems.Count==0 && textCustomField.Text=="") {
					MsgBox.Show(this,"Please select an item in the list on the right or create a new field first.");
					return;
				}
				if(textCustomField.Text!="") {//Add new ortho chart field
					for(int i=0;i<ListShowing.Count;i++) {
						if(textCustomField.Text==ListShowing[i].InternalName) {
							MsgBox.Show(this,"The \""+textCustomField.Text+"\" field is already displaying.");
							return;
						}
					}
					for(int i=0;i<AvailList.Count;i++) {
						if(textCustomField.Text==AvailList[i].InternalName) {
							ListShowing.Add(AvailList[i]);
							textCustomField.Text="";
							changed=true;
							FillGrids();
							return;
						}
					}
					ListShowing.Add(new DisplayField(textCustomField.Text,20,DisplayFieldCategory.OrthoChart));
					textCustomField.Text="";
				}
				else {//add existing ortho chart field(s)
					DisplayField field;
					for(int i=0;i<listAvailable.SelectedItems.Count;i++) {
						field=(DisplayField)listAvailable.SelectedItems[i];
						ListShowing.Add(field);
					}
				}
			}
			else {//All other display field types
				if(listAvailable.SelectedItems.Count==0) {
					MsgBox.Show(this,"Please select an item in the list on the right first.");
					return;
				}
				DisplayField field;
				for(int i=0;i<listAvailable.SelectedItems.Count;i++) {
					field=(DisplayField)listAvailable.SelectedItems[i];
					ListShowing.Add(field);
				}
			}
			changed=true;
			FillGrids();
		}

		private void butRight_Click(object sender,EventArgs e) {
//todo: ortho.
			//1. Remove from listShowing.
			//2. If it's not on AvailList, then it just goes away
			//3. FillGrid seems to be intelligent enough to decide whether it should show in the list at the right.
//No changes needed.
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid on the left first.");
				return;
			}
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--){//go backwards
				ListShowing.RemoveAt(gridMain.SelectedIndices[i]);
			}
			FillGrids();
			changed=true;
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[0]==0){
				return;
			}
			for(int i=0;i<selected.Length;i++){
				ListShowing.Reverse(selected[i]-1,2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++){
				gridMain.SetSelected(selected[i]-1,true);
			}
			changed=true;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				selected[i]=gridMain.SelectedIndices[i];
			}
			if(selected[selected.Length-1]==ListShowing.Count-1) {
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				ListShowing.Reverse(selected[i],2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
			changed=true;
		}

		private void listAvailable_Click(object sender,EventArgs e) {
			textCustomField.Text="";
		}

		private void textCustomField_Click(object sender,EventArgs e) {
			listAvailable.SelectedIndex=-1;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!changed) {
				DialogResult=DialogResult.OK;
				return;
			}
//todo: This will need lots of careful work: 
//No changes needed.
			if(category==DisplayFieldCategory.OrthoChart) {
				DisplayFields.SaveListForOrthoChart(ListShowing);
			}
			else {
				DisplayFields.SaveListForCategory(ListShowing,category);
			}
			DataValid.SetInvalid(InvalidType.DisplayFields);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormDisplayFields_FormClosing(object sender,FormClosingEventArgs e) {

		}

		
		

		

		

		

		

		


	}
}





















