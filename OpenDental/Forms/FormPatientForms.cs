using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPatientForms:Form {
		DataTable table;
		public long PatNum;
		//<summary>When closing this form, if sheets were sent to the terminal, this will be true.  Indicating that the Terminal Manager should show.</summary>
		//public bool TerminalSent;

		public FormPatientForms() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPatientForms_Load(object sender,EventArgs e) {
			Patient pat=Patients.GetLim(PatNum);
			Text=Lan.g(this,"Patient Forms for")+" "+pat.GetNameFL();
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Time"),42);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Terminal"),55,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),210);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Image Category"),120);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			table=Sheets.GetPatientFormsTable(PatNum);
			for(int i=0;i<table.Rows.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["date"].ToString());
				row.Cells.Add(table.Rows[i]["time"].ToString());
				row.Cells.Add(table.Rows[i]["showInTerminal"].ToString());
				row.Cells.Add(table.Rows[i]["description"].ToString());
				row.Cells.Add(table.Rows[i]["imageCat"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(table.Rows[e.Row]["DocNum"].ToString()!="0") {
				long docNum=PIn.Long(table.Rows[e.Row]["DocNum"].ToString());
				GotoModule.GotoImage(PatNum,docNum); 
				return;
			}
			//Sheets
			long sheetNum=PIn.Long(table.Rows[e.Row]["SheetNum"].ToString());
			Sheet sheet=Sheets.GetSheet(sheetNum);
			FormSheetFillEdit FormSF=new FormSheetFillEdit(sheet);
			FormSF.ShowDialog();
			if(FormSF.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butImage_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefinitions formD=new FormDefinitions(DefCat.ImageCats);
			formD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Defs");
			FillGrid();
		}

		private void butSheets_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSheetDefs FormSD=new FormSheetDefs();
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sheets");
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormSheetPicker FormS=new FormSheetPicker();
			FormS.SheetType=SheetTypeEnum.PatientForm;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			SheetDef sheetDef;
			Sheet sheet=null;//only useful if not Terminal
			for(int i=0;i<FormS.SelectedSheetDefs.Count;i++) {
				sheetDef=FormS.SelectedSheetDefs[i];
				sheet=SheetUtil.CreateSheet(sheetDef,PatNum);
				SheetParameter.SetParameter(sheet,"PatNum",PatNum);
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet,this.CreateGraphics());
				if(FormS.TerminalSend) {
					sheet.InternalNote="";//because null not ok
					sheet.ShowInTerminal=true;
					Sheets.SaveNewSheet(sheet);//save each sheet.
				}
			}
			if(FormS.TerminalSend) {
				//if sent to terminal, do not show a dialog now.
				//TerminalSent=true;
				//Close();
				//User will need to click the terminal button.
				FillGrid();
			}
			else{
				FormSheetFillEdit FormSF=new FormSheetFillEdit(sheet);
				FormSF.ShowDialog();
				if(FormSF.DialogResult==DialogResult.OK) {
					FillGrid();
				}
			}
		}

		private void butTerminal_Click(object sender,EventArgs e) {
			bool hasTerminal=false;
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["showInTerminal"].ToString()=="X") {
					hasTerminal=true;
					break;
				}
			}
			if(!hasTerminal) {
				MsgBox.Show(this,"No forms for this patient are set to show in the terminal.");
				return;
			}

		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

		

		

		

		

		
	}
}