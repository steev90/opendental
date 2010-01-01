using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Equipments {

		///<summary></summary>
		public static List<Equipment> GetList(DateTime fromDate,DateTime toDate,EnumEquipmentDisplayMode display) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Equipment>>(MethodBase.GetCurrentMethod(),fromDate,toDate,display);
			}
			string command="";
			if(display==EnumEquipmentDisplayMode.Purchased){
				command="SELECT * FROM equipment "
					+"WHERE DatePurchased >= "+POut.Date(fromDate)
					+" AND DatePurchased <= "+POut.Date(toDate)
					+" ORDER BY DatePurchased";
			}
			if(display==EnumEquipmentDisplayMode.Sold) {
				command="SELECT * FROM equipment "
					+"WHERE DateSold >= "+POut.Date(fromDate)
					+" AND DateSold <= "+POut.Date(toDate)
					+" ORDER BY DatePurchased";
			}
			if(display==EnumEquipmentDisplayMode.All) {
				command="SELECT * FROM equipment "
					+"WHERE (DatePurchased >= "+POut.Date(fromDate)
					+" AND DatePurchased <= "+POut.Date(toDate)+")"
					+" OR (DateSold >= "+POut.Date(fromDate)
					+" AND DateSold <= "+POut.Date(toDate)+")"
					+" ORDER BY DatePurchased";
			}
			DataTable table=Db.GetTable(command);
			List<Equipment> list=new List<Equipment>();
			Equipment equip;
			for(int i=0;i<table.Rows.Count;i++) {
				equip=new Equipment();
				equip.EquipmentNum = PIn.Long(table.Rows[i][0].ToString());
				equip.Description  = PIn.String(table.Rows[i][1].ToString());
				equip.SerialNumber = PIn.String(table.Rows[i][2].ToString());
				equip.ModelYear    = PIn.String(table.Rows[i][3].ToString());
				equip.DatePurchased= PIn.Date(table.Rows[i][4].ToString());
				equip.DateSold     = PIn.Date(table.Rows[i][5].ToString());
				equip.PurchaseCost = PIn.Double(table.Rows[i][6].ToString());
				equip.MarketValue  = PIn.Double(table.Rows[i][7].ToString());
				equip.Location     = PIn.String(table.Rows[i][8].ToString());
				equip.DateEntry    = PIn.Date(table.Rows[i][9].ToString());
				list.Add(equip);
			}
			return list;
		}

		///<summary></summary>
		public static long Insert(Equipment equip) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				equip.EquipmentNum=Meth.GetLong(MethodBase.GetCurrentMethod(),equip);
				return equip.EquipmentNum;
			}
			if(PrefC.RandomKeys) {
				equip.EquipmentNum=ReplicationServers.GetKey("equipment","EquipmentNum");
			}
			string command="INSERT INTO equipment (";
			if(PrefC.RandomKeys) {
				command+="EquipmentNum,";
			}
			command+="Description,SerialNumber,ModelYear,DatePurchased,DateSold,PurchaseCost,MarketValue,Location,DateEntry) VALUES(";
			if(PrefC.RandomKeys) {
				command+=POut.Long(equip.EquipmentNum)+", ";
			}
			command+=
				 "'"+POut.String(equip.Description)+"', "
				+"'"+POut.String(equip.SerialNumber)+"', "
				+"'"+POut.String(equip.ModelYear)+"', "
				+POut.Date(equip.DatePurchased)+", "
				+POut.Date(equip.DateSold)+", "
				+"'"+POut.Double(equip.PurchaseCost)+"', "
				+"'"+POut.Double(equip.MarketValue)+"', "
				+"'"+POut.String(equip.Location)+"', "
				+POut.Date(equip.DateEntry)+")";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				equip.EquipmentNum=Db.NonQ(command,true);
			}
			return equip.EquipmentNum;
		}

		///<summary></summary>
		public static void Update(Equipment equip) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),equip);
				return;
			}
			string command= "UPDATE equipment SET " 
				+ "Description = '"  +POut.String(equip.Description)+"'"
				+ ",SerialNumber = '"+POut.String(equip.SerialNumber)+"'"
				+ ",ModelYear = '"   +POut.String(equip.ModelYear)+"'"
				+ ",DatePurchased = "+POut.Date(equip.DatePurchased)
				+ ",DateSold = "     +POut.Date(equip.DateSold)
				+ ",PurchaseCost = '"+POut.Double(equip.PurchaseCost)+"'"
				+ ",MarketValue = '" +POut.Double(equip.MarketValue)+"'"
				+ ",Location = '"    +POut.String(equip.Location)+"'"
				+ ",DateEntry = "   +POut.Date(equip.DateEntry)
				+" WHERE equipmentNum = '" +POut.Long (equip.EquipmentNum)+"'";
 			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(Equipment equip) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),equip);
				return;
			}
			string command="DELETE FROM equipment" 
				+" WHERE EquipmentNum = "+POut.Long(equip.EquipmentNum);
 			Db.NonQ(command);
		}

		///<summary>Generates a unique 3 char alphanumeric serialnumber.  Checks to make sure it's not already in use.</summary>
		public static string GenerateSerialNum() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string retVal="";
			bool isDuplicate=true;
			Random rand=new Random();
			while(isDuplicate){
				retVal="";
				for(int i=0;i<4;i++) {
					int r=rand.Next(0,34);
					if(r<9) {
						retVal+=(char)('1'+r);//1-9, no zero
					}
					else {
						retVal+=(char)('A'+r-9);
					}
				}
				string command="SELECT COUNT(*) FROM equipment WHERE SerialNumber = '"+POut.String(retVal)+"'";
				if(Db.GetScalar(command)=="0") {
					isDuplicate=false;
				}
			}
			return retVal;
		}
		

		
		

	}

	public enum EnumEquipmentDisplayMode {
		Purchased,
		Sold,
		All
	}
	


}













