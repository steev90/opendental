//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class VitalsignCrud {
		///<summary>Gets one Vitalsign object from the database using the primary key.  Returns null if not found.</summary>
		internal static Vitalsign SelectOne(long vitalsignNum){
			string command="SELECT * FROM vitalsign "
				+"WHERE VitalsignNum = "+POut.Long(vitalsignNum);
			List<Vitalsign> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Vitalsign object from the database using a query.</summary>
		internal static Vitalsign SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Vitalsign> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Vitalsign objects from the database using a query.</summary>
		internal static List<Vitalsign> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Vitalsign> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Vitalsign> TableToList(DataTable table){
			List<Vitalsign> retVal=new List<Vitalsign>();
			Vitalsign vitalsign;
			for(int i=0;i<table.Rows.Count;i++) {
				vitalsign=new Vitalsign();
				vitalsign.VitalsignNum= PIn.Long  (table.Rows[i]["VitalsignNum"].ToString());
				vitalsign.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				vitalsign.Height      = PIn.Float (table.Rows[i]["Height"].ToString());
				vitalsign.Weight      = PIn.Int   (table.Rows[i]["Weight"].ToString());
				vitalsign.BpSystolic  = PIn.Int   (table.Rows[i]["BpSystolic"].ToString());
				vitalsign.BpDiastolic = PIn.Int   (table.Rows[i]["BpDiastolic"].ToString());
				vitalsign.DateTaken   = PIn.Date  (table.Rows[i]["DateTaken"].ToString());
				retVal.Add(vitalsign);
			}
			return retVal;
		}

		///<summary>Inserts one Vitalsign into the database.  Returns the new priKey.</summary>
		internal static long Insert(Vitalsign vitalsign){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				vitalsign.VitalsignNum=DbHelper.GetNextOracleKey("vitalsign","VitalsignNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(vitalsign,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							vitalsign.VitalsignNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(vitalsign,false);
			}
		}

		///<summary>Inserts one Vitalsign into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Vitalsign vitalsign,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				vitalsign.VitalsignNum=ReplicationServers.GetKey("vitalsign","VitalsignNum");
			}
			string command="INSERT INTO vitalsign (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="VitalsignNum,";
			}
			command+="PatNum,Height,Weight,BpSystolic,BpDiastolic,DateTaken) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(vitalsign.VitalsignNum)+",";
			}
			command+=
				     POut.Long  (vitalsign.PatNum)+","
				+    POut.Float (vitalsign.Height)+","
				+    POut.Int   (vitalsign.Weight)+","
				+    POut.Int   (vitalsign.BpSystolic)+","
				+    POut.Int   (vitalsign.BpDiastolic)+","
				+    POut.Date  (vitalsign.DateTaken)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				vitalsign.VitalsignNum=Db.NonQ(command,true);
			}
			return vitalsign.VitalsignNum;
		}

		///<summary>Updates one Vitalsign in the database.</summary>
		internal static void Update(Vitalsign vitalsign){
			string command="UPDATE vitalsign SET "
				+"PatNum      =  "+POut.Long  (vitalsign.PatNum)+", "
				+"Height      =  "+POut.Float (vitalsign.Height)+", "
				+"Weight      =  "+POut.Int   (vitalsign.Weight)+", "
				+"BpSystolic  =  "+POut.Int   (vitalsign.BpSystolic)+", "
				+"BpDiastolic =  "+POut.Int   (vitalsign.BpDiastolic)+", "
				+"DateTaken   =  "+POut.Date  (vitalsign.DateTaken)+" "
				+"WHERE VitalsignNum = "+POut.Long(vitalsign.VitalsignNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Vitalsign in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Vitalsign vitalsign,Vitalsign oldVitalsign){
			string command="";
			if(vitalsign.PatNum != oldVitalsign.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(vitalsign.PatNum)+"";
			}
			if(vitalsign.Height != oldVitalsign.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Float(vitalsign.Height)+"";
			}
			if(vitalsign.Weight != oldVitalsign.Weight) {
				if(command!=""){ command+=",";}
				command+="Weight = "+POut.Int(vitalsign.Weight)+"";
			}
			if(vitalsign.BpSystolic != oldVitalsign.BpSystolic) {
				if(command!=""){ command+=",";}
				command+="BpSystolic = "+POut.Int(vitalsign.BpSystolic)+"";
			}
			if(vitalsign.BpDiastolic != oldVitalsign.BpDiastolic) {
				if(command!=""){ command+=",";}
				command+="BpDiastolic = "+POut.Int(vitalsign.BpDiastolic)+"";
			}
			if(vitalsign.DateTaken != oldVitalsign.DateTaken) {
				if(command!=""){ command+=",";}
				command+="DateTaken = "+POut.Date(vitalsign.DateTaken)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE vitalsign SET "+command
				+" WHERE VitalsignNum = "+POut.Long(vitalsign.VitalsignNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Vitalsign from the database.</summary>
		internal static void Delete(long vitalsignNum){
			string command="DELETE FROM vitalsign "
				+"WHERE VitalsignNum = "+POut.Long(vitalsignNum);
			Db.NonQ(command);
		}

	}
}