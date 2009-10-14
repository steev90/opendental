using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SecurityLogs {

		///<summary>Used when viewing securityLog from the security admin window.  PermTypes can be length 0 to get all types.</summary>
		public static SecurityLog[] Refresh(DateTime dateFrom,DateTime dateTo,Permissions permType,long patNum,
			long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SecurityLog[]>(MethodBase.GetCurrentMethod(),dateFrom,dateTo,permType,patNum,userNum);
			}
			string command="SELECT * FROM securitylog "
				+"WHERE LogDateTime >= "+POut.PDate(dateFrom)+" "
				+"AND LogDateTime <= "+POut.PDate(dateTo.AddDays(1));
			if(patNum !=0) {
				command+=" AND PatNum= '"+POut.PLong(patNum)+"'";
			}
			if(permType!=Permissions.None) {
				command+=" AND PermType="+POut.PLong((int)permType);
			}
			if(userNum!=0) {
				command+=" AND UserNum="+POut.PLong(userNum);
			}
			DataTable table=Db.GetTable(command);
			SecurityLog[] List=new SecurityLog[table.Rows.Count];
			for(int i=0;i<List.Length;i++) {
				List[i]=new SecurityLog();
				List[i].SecurityLogNum= PIn.PLong(table.Rows[i][0].ToString());
				List[i].PermType      = (Permissions)PIn.PLong(table.Rows[i][1].ToString());
				List[i].UserNum       = PIn.PLong(table.Rows[i][2].ToString());
				List[i].LogDateTime   = PIn.PDateT(table.Rows[i][3].ToString());
				List[i].LogText       = PIn.PString(table.Rows[i][4].ToString());
				List[i].PatNum        = PIn.PLong(table.Rows[i][5].ToString());
			}
			return List;
		}

		///<summary></summary>
		public static long Insert(SecurityLog log){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				log.SecurityLogNum=Meth.GetLong(MethodBase.GetCurrentMethod(),log);
				return log.SecurityLogNum;
			}
			if(PrefC.RandomKeys){
				log.SecurityLogNum=ReplicationServers.GetKey("securitylog","SecurityLogNum");
			}
			string command= "INSERT INTO securitylog (";
			if(PrefC.RandomKeys){
				command+="SecurityLogNum,";
			}
			command+="PermType,UserNum,LogDateTime,LogText,PatNum) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.PLong(log.SecurityLogNum)+"', ";
			}
			command+=
				 "'"+POut.PLong   ((int)log.PermType)+"', "
				+"'"+POut.PLong   (log.UserNum)+"', "
				+"NOW(), "//LogDateTime set to current server time
				+"'"+POut.PString(log.LogText)+"', "
				+"'"+POut.PLong   (log.PatNum)+"')";
 			if(PrefC.RandomKeys){
				Db.NonQ(command);
			}
			else{
 				log.SecurityLogNum=Db.NonQ(command,true);
			}
			return log.SecurityLogNum;
		}

		//there are no methods for deleting or changing log entries because that will never be allowed.

		

	
  

		///<summary>Used when viewing various audit trails of specific types.</summary>
		public static SecurityLog[] Refresh(long patNum,List<Permissions> permTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SecurityLog[]>(MethodBase.GetCurrentMethod(),patNum,permTypes);
			}
			string types="";
			for(int i=0;i<permTypes.Count;i++){
				if(i>0){
					types+=" OR";
				}
				types+=" PermType="+POut.PLong((int)permTypes[i]);
			}
			string command="SELECT * FROM securitylog "
				+"WHERE PatNum= '"+POut.PLong(patNum)+"' "
				+"AND ("+types+")";
			DataTable table=Db.GetTable(command);
			SecurityLog[] List=new SecurityLog[table.Rows.Count];
			for(int i=0;i<List.Length;i++){
				List[i]=new SecurityLog();
				List[i].SecurityLogNum= PIn.PLong   (table.Rows[i][0].ToString());
				List[i].PermType      = (Permissions)PIn.PLong(table.Rows[i][1].ToString());
				List[i].UserNum       = PIn.PLong   (table.Rows[i][2].ToString());
				List[i].LogDateTime   = PIn.PDateT (table.Rows[i][3].ToString());	
				List[i].LogText       = PIn.PString(table.Rows[i][4].ToString());
				List[i].PatNum        = PIn.PLong   (table.Rows[i][5].ToString());
			}
			return List;
		}

		///<summary>PatNum can be 0.</summary>
		public static void MakeLogEntry(Permissions permType,long patNum,string logText) {
			//No need to check RemotingRole; no call to db.
			SecurityLog securityLog=new SecurityLog();
			securityLog.PermType=permType;
			securityLog.UserNum=Security.CurUser.UserNum;
			securityLog.LogText="From: "+Environment.MachineName+" - "+logText;
			securityLog.PatNum=patNum;
			SecurityLogs.Insert(securityLog);
		}	

	}
}