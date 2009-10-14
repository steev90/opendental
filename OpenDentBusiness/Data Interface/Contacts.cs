using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Contacts{
		//<summary></summary>
		//public static Contact[] List;//for one category only. Not refreshed with local data

		///<summary></summary>
		public static Contact[] Refresh(long category) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Contact[]>(MethodBase.GetCurrentMethod(),category);
			}
			string command="SELECT * from contact WHERE category = '"+category+"'"
				+" ORDER BY LName";
			DataTable table=Db.GetTable(command);
			Contact[] List = new Contact[table.Rows.Count];
			for(int i=0;i<List.Length;i++){
				List[i]=new Contact();
				List[i].ContactNum = PIn.PLong   (table.Rows[i][0].ToString());
				List[i].LName      = PIn.PString(table.Rows[i][1].ToString());
				List[i].FName      = PIn.PString(table.Rows[i][2].ToString());
				List[i].WkPhone    = PIn.PString(table.Rows[i][3].ToString());
				List[i].Fax        = PIn.PString(table.Rows[i][4].ToString());
				List[i].Category   = PIn.PLong   (table.Rows[i][5].ToString());
				List[i].Notes      = PIn.PString(table.Rows[i][6].ToString());
			}
			return List;
		}

		///<summary></summary>
		public static long Insert(Contact Cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.ContactNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.ContactNum;
			}
			if(PrefC.RandomKeys){
				Cur.ContactNum=ReplicationServers.GetKey("contact","ContactNum");
			}
			string command="INSERT INTO contact (";
			if(PrefC.RandomKeys){
				command+="ContactNum,";
			}
			command+="LName,FName,WkPhone,Fax,Category,"
				+"Notes) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.PLong(Cur.ContactNum)+"', ";
			}
			command+=
				 "'"+POut.PString(Cur.LName)+"', "
				+"'"+POut.PString(Cur.FName)+"', "
				+"'"+POut.PString(Cur.WkPhone)+"', "
				+"'"+POut.PString(Cur.Fax)+"', "
				+"'"+POut.PLong   (Cur.Category)+"', "
				+"'"+POut.PString(Cur.Notes)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				Cur.ContactNum=Db.NonQ(command,true);
			}
			return Cur.ContactNum;
		}

		///<summary></summary>
		public static void Update(Contact Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "UPDATE contact SET "
				+"lname = '"    +POut.PString(Cur.LName)+"' "
				+",fname = '"   +POut.PString(Cur.FName)+"' "
				+",wkphone = '" +POut.PString(Cur.WkPhone)+"' "
				+",fax = '"     +POut.PString(Cur.Fax)+"' "
				+",category = '"+POut.PLong   (Cur.Category)+"' "
				+",notes = '"   +POut.PString(Cur.Notes)+"' "
				+"WHERE contactnum = '"+POut.PLong  (Cur.ContactNum)+"'";
			//MessageBox.Show(string command);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(Contact Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "DELETE FROM contact WHERE contactnum = '"+Cur.ContactNum.ToString()+"'";
			Db.NonQ(command);
		}

	}

	
}