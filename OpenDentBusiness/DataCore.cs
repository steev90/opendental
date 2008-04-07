using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OpenDentBusiness {
	public class DataCore {
		///<summary></summary>
		public static DataSet GetTable(string command) {
			DataConnection dcon=new DataConnection();
			DataTable table=dcon.GetTable(command);
			DataSet retVal=new DataSet();
			//table.TableName="table";
			retVal.Tables.Add(table);
			//retVal.Tables[0].TableName="";
			return retVal;
		}

		///<summary>Only used if using the server component.  This is used for queries written by the user.  It uses the user with lower privileges  to prevent injection attack.</summary>
		public static DataSet GetTableLow(string command) {
			DataConnection dcon=new DataConnection(true);
			DataTable table=dcon.GetTable(command);
			DataSet retVal=new DataSet();
			retVal.Tables.Add(table);
			return retVal;
		}

		///<summary>This query is run with full privileges.  This is for commands generated by the main program, and the user will not have access for injection attacks.  Result is usually number of rows changed, or can be insert id if requested.</summary>
		public static int NonQ(string command,bool getInsertID) {
			DataConnection dcon=new DataConnection();
			int rowsChanged=dcon.NonQ(command,getInsertID);
			if(getInsertID){
				return dcon.InsertID;
			}
			else{
				return rowsChanged;
			}
		}

		public static int NonQ(string command){
			return NonQ(command,false);
		}

		///<summary>This is for multiple queries all concatenated together with ;</summary>
		public static DataSet GetDataSet(string commands){
			DataConnection dcon=new DataConnection();
			//DataTable table=dcon.GetTable(command);
			DataSet retVal=dcon.GetDs(commands);
			//retVal.Tables.Add(table);
			return retVal;
		}

		///<summary></summary>
		public static DataSet GetDsByMethod(MethodNameDS methodName, object[] parameters) {
			switch (methodName){
				default:
					throw new ApplicationException("MethodName not found");
				case MethodNameDS.AccountModule_GetAll:
					return AccountModules.GetAll((int)parameters[0],(bool)parameters[1],(DateTime)parameters[2],(DateTime)parameters[3],(bool)parameters[4]);
				case MethodNameDS.AccountModule_GetPayPlanAmort:
					return AccountModules.GetPayPlanAmort((int)parameters[0]);
				case  MethodNameDS.AccountModule_GetStatement:
					return AccountModules.GetStatement((int)parameters[0],(bool)parameters[1],(DateTime)parameters[2],(DateTime)parameters[3],(bool)parameters[4]);
				case  MethodNameDS.Appointment_GetApptEdit:
					return Appointments.GetApptEdit((int)parameters[0]);
				case  MethodNameDS.Appointment_RefreshPeriod:
					return Appointments.RefreshPeriod((DateTime)parameters[0],(DateTime)parameters[1]);
				case  MethodNameDS.Appointment_RefreshOneApt:
					return Appointments.RefreshOneApt((int)parameters[0],(bool)parameters[1]);
				case  MethodNameDS.Chart_GetAll:
					return ChartModuleB.GetAll((int)parameters[0],(bool)parameters[1]);
				
			}

		}

		///<summary></summary>
		public static DataTable GetTableByMethod(MethodNameTable methodName, object[] parameters) {
			switch (methodName){
				default:
					throw new ApplicationException("MethodName not found");
				case MethodNameTable.Account_RefreshCache:
					return Accounts.RefreshCache();
				case  MethodNameTable.Definition_RefreshCache:
					return Defs.RefreshCache();
				case  MethodNameTable.MountDef_RefreshCache:
					return MountDefs.RefreshCache();
				case  MethodNameTable.Prefs_RefreshCache:
					return Prefs.RefreshCache();
				case  MethodNameTable.Providers_RefreshCache:
					return Providers.RefreshCache();
			}
		}
		


	}

	
}
