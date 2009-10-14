using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class LabCases {

		///<summary>Gets a filtered list of all labcases.</summary>
		public static DataTable Refresh(DateTime aptStartDate,DateTime aptEndDate,bool showCompleted,bool ShowUnattached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),aptStartDate,aptEndDate,showCompleted,ShowUnattached);
			}
			DataTable table=new DataTable();
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("AptDateTime",typeof(DateTime));
			table.Columns.Add("aptDateTime");
			table.Columns.Add("AptNum");
			table.Columns.Add("lab");
			table.Columns.Add("LabCaseNum");
			table.Columns.Add("patient");
			table.Columns.Add("phone");
			table.Columns.Add("ProcDescript");
			table.Columns.Add("status");
			List<DataRow> rows=new List<DataRow>();
			//the first query only gets labcases that are attached to scheduled appointments
			string command="SELECT AptDateTime,appointment.AptNum,DateTimeChecked,DateTimeRecd,DateTimeSent,"
				+"LabCaseNum,laboratory.Description,LName,FName,Preferred,MiddleI,Phone,ProcDescript "
				+"FROM labcase "
				+"LEFT JOIN appointment ON labcase.AptNum=appointment.AptNum "
				+"LEFT JOIN patient ON labcase.PatNum=patient.PatNum "
				+"LEFT JOIN laboratory ON labcase.LaboratoryNum=laboratory.LaboratoryNum "
				+"WHERE AptDateTime > "+POut.PDate(aptStartDate)+" "
				+"AND AptDateTime < "+POut.PDate(aptEndDate.AddDays(1))+" ";
			if(!showCompleted){
				command+="AND (AptStatus="+POut.PLong((int)ApptStatus.ASAP)
					+" OR AptStatus="+POut.PLong((int)ApptStatus.Broken)
					+" OR AptStatus="+POut.PLong((int)ApptStatus.None)
					+" OR AptStatus="+POut.PLong((int)ApptStatus.Scheduled)
					+" OR AptStatus="+POut.PLong((int)ApptStatus.UnschedList)+") ";
			}
			DataTable raw=Db.GetTable(command);
			DateTime AptDateTime;
			DateTime date;
			for(int i=0;i<raw.Rows.Count;i++) {
				row=table.NewRow();
		    AptDateTime=PIn.PDateT(raw.Rows[i]["AptDateTime"].ToString());
				row["AptDateTime"]=AptDateTime;
				row["aptDateTime"]=AptDateTime.ToShortDateString()+" "+AptDateTime.ToShortTimeString();
				row["AptNum"]=raw.Rows[i]["AptNum"].ToString();
				row["lab"]=raw.Rows[i]["Description"].ToString();
				row["LabCaseNum"]=raw.Rows[i]["LabCaseNum"].ToString();
				row["patient"]=PatientLogic.GetNameLF(raw.Rows[i]["LName"].ToString(),raw.Rows[i]["FName"].ToString(),
					raw.Rows[i]["Preferred"].ToString(),raw.Rows[i]["MiddleI"].ToString());
				row["phone"]=raw.Rows[i]["Phone"].ToString();
				row["ProcDescript"]=raw.Rows[i]["ProcDescript"].ToString();
				date=PIn.PDateT(raw.Rows[i]["DateTimeChecked"].ToString());
				if(date.Year>1880) {
					row["status"]=Lans.g("FormLabCases","Quality Checked");
				}
				else {
					date=PIn.PDateT(raw.Rows[i]["DateTimeRecd"].ToString());
					if(date.Year>1880) {
						row["status"]=Lans.g("FormLabCases","Received");
					}
					else {
						date=PIn.PDateT(raw.Rows[i]["DateTimeSent"].ToString());
						if(date.Year>1880) {
							row["status"]=Lans.g("FormLabCases","Sent");//sent but not received
						}
						else {
							row["status"]=Lans.g("FormLabCases","Not Sent");
						}
					}
				}
				rows.Add(row);
			}
			if(ShowUnattached) {
				//Then, this second query gets labcases not attached to appointments.  No date filter.  No date displayed.
				command="SELECT DateTimeChecked,DateTimeRecd,DateTimeSent,"
					+"LabCaseNum,laboratory.Description,LName,FName,Preferred,MiddleI,Phone "
					+"FROM labcase "
					+"LEFT JOIN patient ON labcase.PatNum=patient.PatNum "
					+"LEFT JOIN laboratory ON labcase.LaboratoryNum=laboratory.LaboratoryNum "
					+"WHERE AptNum=0";
				raw=Db.GetTable(command);
				for(int i=0;i<raw.Rows.Count;i++) {
					row=table.NewRow();
					row["AptDateTime"]=DateTime.MinValue;
					row["aptDateTime"]="";
					row["AptNum"]=0;
					row["lab"]=raw.Rows[i]["Description"].ToString();
					row["LabCaseNum"]=raw.Rows[i]["LabCaseNum"].ToString();
					row["patient"]=PatientLogic.GetNameLF(raw.Rows[i]["LName"].ToString(),raw.Rows[i]["FName"].ToString(),
						raw.Rows[i]["Preferred"].ToString(),raw.Rows[i]["MiddleI"].ToString());
					row["phone"]=raw.Rows[i]["Phone"].ToString();
					row["ProcDescript"]="";
					row["status"]="";
					date=PIn.PDateT(raw.Rows[i]["DateTimeChecked"].ToString());
					if(date.Year>1880) {
						row["status"]=Lans.g("FormLabCases","Quality Checked");
					}
					else {
						date=PIn.PDateT(raw.Rows[i]["DateTimeRecd"].ToString());
						if(date.Year>1880) {
							row["status"]=Lans.g("FormLabCases","Received");
						}
						else {
							date=PIn.PDateT(raw.Rows[i]["DateTimeSent"].ToString());
							if(date.Year>1880) {
								row["status"]=Lans.g("FormLabCases","Sent");//sent but not received
							}
							else {
								row["status"]=Lans.g("FormLabCases","Not Sent");
							}
						}
					}
					rows.Add(row);
				}
			}
			LabCaseComparer comparer=new LabCaseComparer();
			rows.Sort(comparer);
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Used when drawing the appointments for a day.</summary>
		public static List<LabCase> GetForPeriod(DateTime startDate,DateTime endDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<LabCase>>(MethodBase.GetCurrentMethod(),startDate,endDate);
			} 
			string command="SELECT labcase.* FROM labcase,appointment "
				+"WHERE labcase.AptNum=appointment.AptNum "
				+"AND (appointment.AptStatus=1 OR appointment.AptStatus=2 OR appointment.AptStatus=4) "//scheduled,complete,or ASAP
				+"AND AptDateTime >= "+POut.PDate(startDate)
				+" AND AptDateTime < "+POut.PDate(endDate.AddDays(1));//midnight of the next morning.
			DataTable table=Db.GetTable(command);
			return FillFromTable(table);
		}

		///<summary>Used when drawing the planned appointment.</summary>
		public static LabCase GetForPlanned(long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<LabCase>(MethodBase.GetCurrentMethod(),aptNum);
			}
			string command="SELECT labcase.* FROM labcase,appointment "
				+"WHERE labcase.PlannedAptNum="+POut.PLong(aptNum);
			DataTable table=Db.GetTable(command);
			List<LabCase> list=FillFromTable(table);
			if(list.Count==0){
				return null;
			}
			return list[0];
		}

		///<summary>Gets one labcase from database.</summary>
		public static LabCase GetOne(long labCaseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<LabCase>(MethodBase.GetCurrentMethod(),labCaseNum);
			}
			string command="SELECT * FROM labcase WHERE LabCaseNum="+POut.PLong(labCaseNum);
			DataTable table=Db.GetTable(command);
			return FillFromTable(table)[0];
		}

		///<summary>Gets all labcases for a patient which have not been attached to an appointment.  Usually one or none.  Only used when attaching a labcase from within an appointment.</summary>
		public static List<LabCase> GetForPat(long patNum,bool isPlanned) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<LabCase>>(MethodBase.GetCurrentMethod(),patNum,isPlanned);
			}
			string command="SELECT * FROM labcase WHERE PatNum="+POut.PLong(patNum)+" AND ";
			if(isPlanned){
				command+="PlannedAptNum=0 AND AptNum=0";//We only show lab cases that have not been attached to any kind of appt.
			}
			else{
				command+="AptNum=0";
			}
			DataTable table=Db.GetTable(command);
			return FillFromTable(table);
		}

		public static List<LabCase> FillFromTable(DataTable table){
			LabCase lab;
			List<LabCase> retVal=new List<LabCase>();
			for(int i=0;i<table.Rows.Count;i++) {
				lab=new LabCase();
				lab.LabCaseNum     = PIn.PLong   (table.Rows[i][0].ToString());
				lab.PatNum         = PIn.PLong   (table.Rows[i][1].ToString());
				lab.LaboratoryNum  = PIn.PLong   (table.Rows[i][2].ToString());
				lab.AptNum         = PIn.PLong   (table.Rows[i][3].ToString());
				lab.PlannedAptNum  = PIn.PLong   (table.Rows[i][4].ToString());
				lab.DateTimeDue    = PIn.PDateT (table.Rows[i][5].ToString());
				lab.DateTimeCreated= PIn.PDateT (table.Rows[i][6].ToString());
				lab.DateTimeSent   = PIn.PDateT (table.Rows[i][7].ToString());
				lab.DateTimeRecd   = PIn.PDateT (table.Rows[i][8].ToString());
				lab.DateTimeChecked= PIn.PDateT (table.Rows[i][9].ToString());
				lab.ProvNum        = PIn.PLong   (table.Rows[i][10].ToString());
				lab.Instructions   = PIn.PString(table.Rows[i][11].ToString());
				retVal.Add(lab);
			}
			return retVal;
		}

		///<summary></summary>
		public static long Insert(LabCase lab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				lab.LabCaseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),lab);
				return lab.LabCaseNum;
			}
			if(PrefC.RandomKeys) {
				lab.LabCaseNum=ReplicationServers.GetKey("labcase","LabCaseNum");
			}
			string command="INSERT INTO labcase (";
			if(PrefC.RandomKeys) {
				command+="LabCaseNum,";
			}
			command+="PatNum,LaboratoryNum,AptNum,PlannedAptNum,DateTimeDue,DateTimeCreated,"
				+"DateTimeSent,DateTimeRecd,DateTimeChecked,ProvNum,Instructions) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.PLong(lab.LabCaseNum)+"', ";
			}
			command+=
				 "'"+POut.PLong   (lab.PatNum)+"', "
				+"'"+POut.PLong   (lab.LaboratoryNum)+"', "
				+"'"+POut.PLong   (lab.AptNum)+"', "
				+"'"+POut.PLong   (lab.PlannedAptNum)+"', "
				    +POut.PDateT (lab.DateTimeDue)+", "
				    +POut.PDateT (lab.DateTimeCreated)+", "
				    +POut.PDateT (lab.DateTimeSent)+", "
				    +POut.PDateT (lab.DateTimeRecd)+", "
				    +POut.PDateT (lab.DateTimeChecked)+", "
				+"'"+POut.PLong   (lab.ProvNum)+"', "
				+"'"+POut.PString(lab.Instructions)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				lab.LabCaseNum=Db.NonQ(command,true);
			}
			return lab.LabCaseNum;
		}

		///<summary></summary>
		public static void Update(LabCase lab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),lab);
				return;
			}
			string command= "UPDATE labcase SET " 
				+ "PatNum = '"          +POut.PLong   (lab.PatNum)+"'"
				+ ",LaboratoryNum = '"  +POut.PLong   (lab.LaboratoryNum)+"'"
				+ ",AptNum = '"         +POut.PLong   (lab.AptNum)+"'"
				+ ",PlannedAptNum = '"  +POut.PLong   (lab.PlannedAptNum)+"'"
				+ ",DateTimeDue = "     +POut.PDateT (lab.DateTimeDue)
				+ ",DateTimeCreated = " +POut.PDateT (lab.DateTimeCreated)
				+ ",DateTimeSent = "    +POut.PDateT (lab.DateTimeSent)
				+ ",DateTimeRecd = "    +POut.PDateT (lab.DateTimeRecd)
				+ ",DateTimeChecked = " +POut.PDateT (lab.DateTimeChecked)
				+ ",ProvNum = '"        +POut.PLong   (lab.ProvNum)+"'"
				+ ",Instructions = '"   +POut.PString(lab.Instructions)+"'"
				+" WHERE LabCaseNum = '"+POut.PLong(lab.LabCaseNum)+"'";
 			Db.NonQ(command);
		}


		///<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		public static void Delete(long labCaseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),labCaseNum);
				return;
			}
			string command;
			/*
			//check patients for dependencies
			string command="SELECT LName,FName FROM patient WHERE LabCaseNum ="
				+POut.PInt(LabCase.LabCaseNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lans.g("LabCases","Cannot delete LabCase because ")+pats);
			}*/
			//delete
			command= "DELETE FROM labcase WHERE LabCaseNum = "+POut.PLong(labCaseNum);
 			Db.NonQ(command);
		}

		///<summary>Attaches a labcase to an appointment.</summary>
		public static void AttachToAppt(long labCaseNum,long aptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),labCaseNum,aptNum);
				return;
			}
			string command="UPDATE labcase SET AptNum="+POut.PLong(aptNum)+" WHERE LabCaseNum="+POut.PLong(labCaseNum);
			Db.NonQ(command);
		}

		///<summary>Attaches a labcase to a planned appointment.</summary>
		public static void AttachToPlannedAppt(long labCaseNum,long plannedAptNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),labCaseNum,plannedAptNum);
				return;
			}
			string command="UPDATE labcase SET PlannedAptNum="+POut.PLong(plannedAptNum)+" WHERE LabCaseNum="+POut.PLong(labCaseNum);
			Db.NonQ(command);
		}

		///<summary>Frequently returns null.</summary>
		public static LabCase GetOneFromList(List<LabCase> labCaseList,long aptNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<labCaseList.Count;i++){
				if(labCaseList[i].AptNum==aptNum){
					return labCaseList[i];
				}
			}
			return null;
		}

	}



	///<summary>The supplied DataRows must include the following columns: AptDateTime,patient</summary>
	class LabCaseComparer:IComparer<DataRow> {

		///<summary></summary>
		public int Compare(DataRow x,DataRow y) {
			DateTime dtx=(DateTime)x["AptDateTime"];
			DateTime dty=(DateTime)y["AptDateTime"];
			if(dty != dtx) {
				return dtx.CompareTo(dty);
			}
			return x["patient"].ToString().CompareTo(y["patient"].ToString());
		}
	}
	


}













