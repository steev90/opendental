package com.opendental.odweb.client.windows;

import java.util.Date;

import com.google.gwt.core.client.GWT;
import com.google.gwt.core.client.Scheduler;
import com.google.gwt.core.client.Scheduler.ScheduledCommand;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.uibinder.client.UiHandler;
import com.google.gwt.user.client.ui.Button;
import com.google.gwt.user.client.ui.CheckBox;
import com.google.gwt.user.client.ui.DockPanel;
import com.google.gwt.user.client.ui.ListBox;
import com.google.gwt.user.client.ui.TextBox;
import com.google.gwt.user.client.ui.Widget;
import com.opendental.odweb.client.data.DataTable;
import com.opendental.odweb.client.datainterface.Patients;
import com.opendental.odweb.client.datainterface.Prefs;
import com.opendental.odweb.client.datainterface.Prefs.PrefName;
import com.opendental.odweb.client.remoting.Db;
import com.opendental.odweb.client.remoting.Db.RequestCallbackResult;
import com.opendental.odweb.client.ui.*;
import com.opendental.odweb.client.ui.ODGrid.ODGridDoubleClickHandler;

public class WindowPatientSelect extends ODWindow {
	private DataTable PatientTable=new DataTable();
	@UiField(provided=true) ODGrid gridMain;
	@UiField DockPanel panelContainer;
	@UiField TextBox textLName;
	@UiField TextBox textFName;
	@UiField TextBox textHmPhone;
	@UiField TextBox textAddress;
	@UiField TextBox textCity;
	@UiField TextBox textState;
	@UiField TextBox textSSN;
	@UiField TextBox textPatNum;
	@UiField TextBox textChartNumber;
	@UiField TextBox textBirthdate;
	@UiField TextBox textSubscriberID;
	@UiField TextBox textEmail;
	@UiField ListBox comboBillingType;
	@UiField ListBox comboSite;
	@UiField CheckBox checkGuarantors;
	@UiField CheckBox checkHideInactive;
	@UiField CheckBox checkShowArchived;
	@UiField Button butSearch;
	@UiField Button butGetAll;
	@UiField CheckBox checkRefresh;
	@UiField Button butAddPt;
	@UiField Button butAddAll;
	@UiField Button butOK;
	@UiField Button butCancel;
	/** When closing the form, this will hold the value of the newly selected PatNum. */
	private int SelectedPatNum;
	
	//These lines need to be in every class that uses UiBinder.  This is what makes this class point to it's respective ui.xml file. 
	private static WindowPatientSelectUiBinder uiBinder=GWT.create(WindowPatientSelectUiBinder.class);
	interface WindowPatientSelectUiBinder extends UiBinder<Widget, WindowPatientSelect> {
	}
	
	public WindowPatientSelect() {		
		super("Patient Select");		
		gridMain=new ODGrid("Select Patient");
		gridMain.setWidthAndHeight(500,625);
		//Add a double click handler to the grid.
		gridMain.addCellDoubleClickHandler(new gridMain_CellDoubleClick());
		//Fills the @UiField objects.
		uiBinder.createAndBindUi(this);
		this.add(panelContainer);
//		fillSearchOptions();
		fillGrid();
		//Simply setting the focus does not work.  A deferred scheduler needs to be used instead.
		Scheduler.get().scheduleDeferred(new ScheduledCommand() {
			public void execute() {
				textLName.setFocus(true);
			}
		});
	}
	
	public int getSelectedPatNum() {
		return SelectedPatNum;
	}

	public void setSelectedPatNum(int selectedPatNum) {
		SelectedPatNum=selectedPatNum;
	}

	private void fillSearchOptions() {
		if(Prefs.getBool(PrefName.PatientSelectUsesSearchButton)) {
			checkRefresh.setValue(false);
		}
		else {
			checkRefresh.setValue(true);
		}
	}
	
	private class FillSearchOptionsCallBack implements RequestCallbackResult {
		public void onSuccess(Object obj) {
			
		}
	}
	
	/** Refreshes the patient grid with the information in the PatientTable.  Does nothing if PatientTable is null. */
	private void fillGrid() {
		gridMain.beginUpdate();
		gridMain.Columns.clear();
		ODGridColumn col=new ODGridColumn("LName",100);
		gridMain.Columns.add(col);
		col=new ODGridColumn("FName",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("MI",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Pref Name",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Age",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("SSN",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Hm Phone",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("WkPhone",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("PatNum",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("ChartNum",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Address",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Status",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Bill Type",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("City",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("State",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Pri Prov",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Birthdate",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Site",80);
		gridMain.Columns.add(col);
		col=new ODGridColumn("Email",80);
		gridMain.Columns.add(col);
		gridMain.Rows.clear();
		ODGridRow row;
		for(int i=0;i<PatientTable.Rows.size();i++) {
			row=new ODGridRow();
			row.Cells.Add(PatientTable.getCellText(i,"LName"));
			row.Cells.Add(PatientTable.getCellText(i,"FName"));
			row.Cells.Add(PatientTable.getCellText(i,"MiddleI"));
			row.Cells.Add(PatientTable.getCellText(i,"Preferred"));
			row.Cells.Add(PatientTable.getCellText(i,"age"));
			row.Cells.Add(PatientTable.getCellText(i,"SSN"));
			row.Cells.Add(PatientTable.getCellText(i,"HmPhone"));
			row.Cells.Add(PatientTable.getCellText(i,"WkPhone"));
			row.Cells.Add(PatientTable.getCellText(i,"PatNum"));
			row.Cells.Add(PatientTable.getCellText(i,"ChartNumber"));
			row.Cells.Add(PatientTable.getCellText(i,"Address"));
			row.Cells.Add(PatientTable.getCellText(i,"PatStatus"));
			row.Cells.Add(PatientTable.getCellText(i,"BillingType"));
			row.Cells.Add(PatientTable.getCellText(i,"City"));
			row.Cells.Add(PatientTable.getCellText(i,"State"));
			row.Cells.Add(PatientTable.getCellText(i,"PriProv"));
			row.Cells.Add(PatientTable.getCellText(i,"Birthdate"));
			row.Cells.Add(PatientTable.getCellText(i,"site"));
			row.Cells.Add(PatientTable.getCellText(i,"Email"));
			gridMain.Rows.add(row);
		}
		gridMain.endUpdate();
	}
	
	/** Makes a request to the server for the patient data table based on the text boxes filled in and populates gridMain with the results. 
	 *  @param limit Adds a LIMIT restriction to the SQL query so that the query doesn't take as long. */
	private void fillGrid(boolean limit) {
		int billingType=0;
//				if(comboBillingType.SelectedIndex!=0){
//					billingType=DefC.Short[(int)DefCat.BillingTypes][comboBillingType.SelectedIndex-1].DefNum;
//				}
		int siteNum=0;
//				if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
//					siteNum=SiteC.List[comboSite.SelectedIndex-1].SiteNum;
//				}
		Date birthdate=new Date();//PIn.Date(textBirthdate.Text);//this will frequently be 0001-01-01.
		int clinicNum=0;//all clinics
//				if(Security.CurUser.ClinicNum!=0 && Security.CurUser.ClinicIsRestricted){
//					clinicNum=Security.CurUser.ClinicNum;
//				}
		Db.sendRequest(Patients.getPtDataTable(limit, textLName.getText(), textFName.getText(), textHmPhone.getText(),
				textAddress.getText(), checkHideInactive.getValue(), textCity.getText(), textState.getText(),
				textSSN.getText(), textPatNum.getText(), textChartNumber.getText(), billingType,
				checkGuarantors.getValue(), checkShowArchived.getValue(), clinicNum, birthdate, siteNum, textSubscriberID.getText(), textEmail.getText())
				, new ButSearchCallback());
	}
	
	private class ButSearchCallback implements RequestCallbackResult {
		public void onSuccess(Object obj) {
			PatientTable=(DataTable)obj;
			fillGrid();
		}
	}
	
	/** If refresh while typing is checked, this will make a call to the database on each key stroke in any search by field. */
	@SuppressWarnings("unused")
	private void onDataEntered() {
		if(checkRefresh.getValue()) {
			fillGrid(true);
		}
	}

	@UiHandler("butSearch")
	void butSearch_Click(ClickEvent event) {
		fillGrid(true);
	}
	
	@UiHandler("butGetAll")
	void butGetAll_Click(ClickEvent event) {
		fillGrid(false);
	}
	
	@UiHandler("butAddPt")
	void butAddPt_Click(ClickEvent event) {
		
	}
	
	@UiHandler("butAddAll")
	void butAddAll_Click(ClickEvent event) {
		
	}
	
	private void patSelected() {
		SelectedPatNum=Integer.parseInt(PatientTable.getCellText(gridMain.getSelectedIndex(), "PatNum"));
		DialogResultCallback.OK();
		this.hide();
	}
	
	private class gridMain_CellDoubleClick implements ODGridDoubleClickHandler {
		public void onCellDoubleClick() {
			patSelected();
		}
	}
	
	@UiHandler("butOK")
	void butOK_Click(ClickEvent event) {
		if(gridMain.getSelectedIndex()==-1) {
			MsgBox.show("Please select a patient first.");
			return;
		}
		patSelected();
	}
	
	@UiHandler("butCancel")
	void butCancel_Click(ClickEvent event) {
		this.hide();
	}

}
