//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcedureCodeCrud {
		///<summary>Gets one ProcedureCode object from the database using the primary key.  Returns null if not found.</summary>
		public static ProcedureCode SelectOne(long codeNum){
			string command="SELECT * FROM procedurecode "
				+"WHERE CodeNum = "+POut.Long(codeNum);
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcedureCode object from the database using a query.</summary>
		public static ProcedureCode SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcedureCode objects from the database using a query.</summary>
		public static List<ProcedureCode> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcedureCode> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ProcedureCode> TableToList(DataTable table){
			List<ProcedureCode> retVal=new List<ProcedureCode>();
			ProcedureCode procedureCode;
			for(int i=0;i<table.Rows.Count;i++) {
				procedureCode=new ProcedureCode();
				procedureCode.CodeNum           = PIn.Long  (table.Rows[i]["CodeNum"].ToString());
				procedureCode.ProcCode          = PIn.String(table.Rows[i]["ProcCode"].ToString());
				procedureCode.Descript          = PIn.String(table.Rows[i]["Descript"].ToString());
				procedureCode.AbbrDesc          = PIn.String(table.Rows[i]["AbbrDesc"].ToString());
				procedureCode.ProcTime          = PIn.String(table.Rows[i]["ProcTime"].ToString());
				procedureCode.ProcCat           = PIn.Long  (table.Rows[i]["ProcCat"].ToString());
				procedureCode.TreatArea         = (TreatmentArea)PIn.Int(table.Rows[i]["TreatArea"].ToString());
				procedureCode.NoBillIns         = PIn.Bool  (table.Rows[i]["NoBillIns"].ToString());
				procedureCode.IsProsth          = PIn.Bool  (table.Rows[i]["IsProsth"].ToString());
				procedureCode.DefaultNote       = PIn.String(table.Rows[i]["DefaultNote"].ToString());
				procedureCode.IsHygiene         = PIn.Bool  (table.Rows[i]["IsHygiene"].ToString());
				procedureCode.GTypeNum          = PIn.Int   (table.Rows[i]["GTypeNum"].ToString());
				procedureCode.AlternateCode1    = PIn.String(table.Rows[i]["AlternateCode1"].ToString());
				procedureCode.MedicalCode       = PIn.String(table.Rows[i]["MedicalCode"].ToString());
				procedureCode.IsTaxed           = PIn.Bool  (table.Rows[i]["IsTaxed"].ToString());
				procedureCode.PaintType         = (ToothPaintingType)PIn.Int(table.Rows[i]["PaintType"].ToString());
				procedureCode.GraphicColor      = Color.FromArgb(PIn.Int(table.Rows[i]["GraphicColor"].ToString()));
				procedureCode.LaymanTerm        = PIn.String(table.Rows[i]["LaymanTerm"].ToString());
				procedureCode.IsCanadianLab     = PIn.Bool  (table.Rows[i]["IsCanadianLab"].ToString());
				procedureCode.PreExisting       = PIn.Bool  (table.Rows[i]["PreExisting"].ToString());
				procedureCode.BaseUnits         = PIn.Int   (table.Rows[i]["BaseUnits"].ToString());
				procedureCode.SubstitutionCode  = PIn.String(table.Rows[i]["SubstitutionCode"].ToString());
				procedureCode.SubstOnlyIf       = (SubstitutionCondition)PIn.Int(table.Rows[i]["SubstOnlyIf"].ToString());
				procedureCode.DateTStamp        = PIn.DateT (table.Rows[i]["DateTStamp"].ToString());
				procedureCode.IsMultiVisit      = PIn.Bool  (table.Rows[i]["IsMultiVisit"].ToString());
				procedureCode.DrugNDC           = PIn.String(table.Rows[i]["DrugNDC"].ToString());
				procedureCode.RevenueCodeDefault= PIn.String(table.Rows[i]["RevenueCodeDefault"].ToString());
				procedureCode.ProvNumDefault    = PIn.Long  (table.Rows[i]["ProvNumDefault"].ToString());
				retVal.Add(procedureCode);
			}
			return retVal;
		}

		///<summary>Inserts one ProcedureCode into the database.  Returns the new priKey.</summary>
		public static long Insert(ProcedureCode procedureCode){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				procedureCode.CodeNum=DbHelper.GetNextOracleKey("procedurecode","CodeNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(procedureCode,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							procedureCode.CodeNum++;
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
				return Insert(procedureCode,false);
			}
		}

		///<summary>Inserts one ProcedureCode into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ProcedureCode procedureCode,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procedureCode.CodeNum=ReplicationServers.GetKey("procedurecode","CodeNum");
			}
			string command="INSERT INTO procedurecode (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CodeNum,";
			}
			command+="ProcCode,Descript,AbbrDesc,ProcTime,ProcCat,TreatArea,NoBillIns,IsProsth,DefaultNote,IsHygiene,GTypeNum,AlternateCode1,MedicalCode,IsTaxed,PaintType,GraphicColor,LaymanTerm,IsCanadianLab,PreExisting,BaseUnits,SubstitutionCode,SubstOnlyIf,IsMultiVisit,DrugNDC,RevenueCodeDefault,ProvNumDefault) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procedureCode.CodeNum)+",";
			}
			command+=
				 "'"+POut.String(procedureCode.ProcCode)+"',"
				+"'"+POut.String(procedureCode.Descript)+"',"
				+"'"+POut.String(procedureCode.AbbrDesc)+"',"
				+"'"+POut.String(procedureCode.ProcTime)+"',"
				+    POut.Long  (procedureCode.ProcCat)+","
				+    POut.Int   ((int)procedureCode.TreatArea)+","
				+    POut.Bool  (procedureCode.NoBillIns)+","
				+    POut.Bool  (procedureCode.IsProsth)+","
				+"'"+POut.String(procedureCode.DefaultNote)+"',"
				+    POut.Bool  (procedureCode.IsHygiene)+","
				+    POut.Int   (procedureCode.GTypeNum)+","
				+"'"+POut.String(procedureCode.AlternateCode1)+"',"
				+"'"+POut.String(procedureCode.MedicalCode)+"',"
				+    POut.Bool  (procedureCode.IsTaxed)+","
				+    POut.Int   ((int)procedureCode.PaintType)+","
				+    POut.Int   (procedureCode.GraphicColor.ToArgb())+","
				+"'"+POut.String(procedureCode.LaymanTerm)+"',"
				+    POut.Bool  (procedureCode.IsCanadianLab)+","
				+    POut.Bool  (procedureCode.PreExisting)+","
				+    POut.Int   (procedureCode.BaseUnits)+","
				+"'"+POut.String(procedureCode.SubstitutionCode)+"',"
				+    POut.Int   ((int)procedureCode.SubstOnlyIf)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (procedureCode.IsMultiVisit)+","
				+"'"+POut.String(procedureCode.DrugNDC)+"',"
				+"'"+POut.String(procedureCode.RevenueCodeDefault)+"',"
				+    POut.Long  (procedureCode.ProvNumDefault)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procedureCode.CodeNum=Db.NonQ(command,true);
			}
			return procedureCode.CodeNum;
		}

		///<summary>Updates one ProcedureCode in the database.</summary>
		public static void Update(ProcedureCode procedureCode){
			string command="UPDATE procedurecode SET "
				//ProcCode excluded from update
				+"Descript          = '"+POut.String(procedureCode.Descript)+"', "
				+"AbbrDesc          = '"+POut.String(procedureCode.AbbrDesc)+"', "
				+"ProcTime          = '"+POut.String(procedureCode.ProcTime)+"', "
				+"ProcCat           =  "+POut.Long  (procedureCode.ProcCat)+", "
				+"TreatArea         =  "+POut.Int   ((int)procedureCode.TreatArea)+", "
				+"NoBillIns         =  "+POut.Bool  (procedureCode.NoBillIns)+", "
				+"IsProsth          =  "+POut.Bool  (procedureCode.IsProsth)+", "
				+"DefaultNote       = '"+POut.String(procedureCode.DefaultNote)+"', "
				+"IsHygiene         =  "+POut.Bool  (procedureCode.IsHygiene)+", "
				+"GTypeNum          =  "+POut.Int   (procedureCode.GTypeNum)+", "
				+"AlternateCode1    = '"+POut.String(procedureCode.AlternateCode1)+"', "
				+"MedicalCode       = '"+POut.String(procedureCode.MedicalCode)+"', "
				+"IsTaxed           =  "+POut.Bool  (procedureCode.IsTaxed)+", "
				+"PaintType         =  "+POut.Int   ((int)procedureCode.PaintType)+", "
				+"GraphicColor      =  "+POut.Int   (procedureCode.GraphicColor.ToArgb())+", "
				+"LaymanTerm        = '"+POut.String(procedureCode.LaymanTerm)+"', "
				+"IsCanadianLab     =  "+POut.Bool  (procedureCode.IsCanadianLab)+", "
				+"PreExisting       =  "+POut.Bool  (procedureCode.PreExisting)+", "
				+"BaseUnits         =  "+POut.Int   (procedureCode.BaseUnits)+", "
				+"SubstitutionCode  = '"+POut.String(procedureCode.SubstitutionCode)+"', "
				+"SubstOnlyIf       =  "+POut.Int   ((int)procedureCode.SubstOnlyIf)+", "
				//DateTStamp can only be set by MySQL
				+"IsMultiVisit      =  "+POut.Bool  (procedureCode.IsMultiVisit)+", "
				+"DrugNDC           = '"+POut.String(procedureCode.DrugNDC)+"', "
				+"RevenueCodeDefault= '"+POut.String(procedureCode.RevenueCodeDefault)+"', "
				+"ProvNumDefault    =  "+POut.Long  (procedureCode.ProvNumDefault)+" "
				+"WHERE CodeNum = "+POut.Long(procedureCode.CodeNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcedureCode in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(ProcedureCode procedureCode,ProcedureCode oldProcedureCode){
			string command="";
			//ProcCode excluded from update
			if(procedureCode.Descript != oldProcedureCode.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(procedureCode.Descript)+"'";
			}
			if(procedureCode.AbbrDesc != oldProcedureCode.AbbrDesc) {
				if(command!=""){ command+=",";}
				command+="AbbrDesc = '"+POut.String(procedureCode.AbbrDesc)+"'";
			}
			if(procedureCode.ProcTime != oldProcedureCode.ProcTime) {
				if(command!=""){ command+=",";}
				command+="ProcTime = '"+POut.String(procedureCode.ProcTime)+"'";
			}
			if(procedureCode.ProcCat != oldProcedureCode.ProcCat) {
				if(command!=""){ command+=",";}
				command+="ProcCat = "+POut.Long(procedureCode.ProcCat)+"";
			}
			if(procedureCode.TreatArea != oldProcedureCode.TreatArea) {
				if(command!=""){ command+=",";}
				command+="TreatArea = "+POut.Int   ((int)procedureCode.TreatArea)+"";
			}
			if(procedureCode.NoBillIns != oldProcedureCode.NoBillIns) {
				if(command!=""){ command+=",";}
				command+="NoBillIns = "+POut.Bool(procedureCode.NoBillIns)+"";
			}
			if(procedureCode.IsProsth != oldProcedureCode.IsProsth) {
				if(command!=""){ command+=",";}
				command+="IsProsth = "+POut.Bool(procedureCode.IsProsth)+"";
			}
			if(procedureCode.DefaultNote != oldProcedureCode.DefaultNote) {
				if(command!=""){ command+=",";}
				command+="DefaultNote = '"+POut.String(procedureCode.DefaultNote)+"'";
			}
			if(procedureCode.IsHygiene != oldProcedureCode.IsHygiene) {
				if(command!=""){ command+=",";}
				command+="IsHygiene = "+POut.Bool(procedureCode.IsHygiene)+"";
			}
			if(procedureCode.GTypeNum != oldProcedureCode.GTypeNum) {
				if(command!=""){ command+=",";}
				command+="GTypeNum = "+POut.Int(procedureCode.GTypeNum)+"";
			}
			if(procedureCode.AlternateCode1 != oldProcedureCode.AlternateCode1) {
				if(command!=""){ command+=",";}
				command+="AlternateCode1 = '"+POut.String(procedureCode.AlternateCode1)+"'";
			}
			if(procedureCode.MedicalCode != oldProcedureCode.MedicalCode) {
				if(command!=""){ command+=",";}
				command+="MedicalCode = '"+POut.String(procedureCode.MedicalCode)+"'";
			}
			if(procedureCode.IsTaxed != oldProcedureCode.IsTaxed) {
				if(command!=""){ command+=",";}
				command+="IsTaxed = "+POut.Bool(procedureCode.IsTaxed)+"";
			}
			if(procedureCode.PaintType != oldProcedureCode.PaintType) {
				if(command!=""){ command+=",";}
				command+="PaintType = "+POut.Int   ((int)procedureCode.PaintType)+"";
			}
			if(procedureCode.GraphicColor != oldProcedureCode.GraphicColor) {
				if(command!=""){ command+=",";}
				command+="GraphicColor = "+POut.Int(procedureCode.GraphicColor.ToArgb())+"";
			}
			if(procedureCode.LaymanTerm != oldProcedureCode.LaymanTerm) {
				if(command!=""){ command+=",";}
				command+="LaymanTerm = '"+POut.String(procedureCode.LaymanTerm)+"'";
			}
			if(procedureCode.IsCanadianLab != oldProcedureCode.IsCanadianLab) {
				if(command!=""){ command+=",";}
				command+="IsCanadianLab = "+POut.Bool(procedureCode.IsCanadianLab)+"";
			}
			if(procedureCode.PreExisting != oldProcedureCode.PreExisting) {
				if(command!=""){ command+=",";}
				command+="PreExisting = "+POut.Bool(procedureCode.PreExisting)+"";
			}
			if(procedureCode.BaseUnits != oldProcedureCode.BaseUnits) {
				if(command!=""){ command+=",";}
				command+="BaseUnits = "+POut.Int(procedureCode.BaseUnits)+"";
			}
			if(procedureCode.SubstitutionCode != oldProcedureCode.SubstitutionCode) {
				if(command!=""){ command+=",";}
				command+="SubstitutionCode = '"+POut.String(procedureCode.SubstitutionCode)+"'";
			}
			if(procedureCode.SubstOnlyIf != oldProcedureCode.SubstOnlyIf) {
				if(command!=""){ command+=",";}
				command+="SubstOnlyIf = "+POut.Int   ((int)procedureCode.SubstOnlyIf)+"";
			}
			//DateTStamp can only be set by MySQL
			if(procedureCode.IsMultiVisit != oldProcedureCode.IsMultiVisit) {
				if(command!=""){ command+=",";}
				command+="IsMultiVisit = "+POut.Bool(procedureCode.IsMultiVisit)+"";
			}
			if(procedureCode.DrugNDC != oldProcedureCode.DrugNDC) {
				if(command!=""){ command+=",";}
				command+="DrugNDC = '"+POut.String(procedureCode.DrugNDC)+"'";
			}
			if(procedureCode.RevenueCodeDefault != oldProcedureCode.RevenueCodeDefault) {
				if(command!=""){ command+=",";}
				command+="RevenueCodeDefault = '"+POut.String(procedureCode.RevenueCodeDefault)+"'";
			}
			if(procedureCode.ProvNumDefault != oldProcedureCode.ProvNumDefault) {
				if(command!=""){ command+=",";}
				command+="ProvNumDefault = "+POut.Long(procedureCode.ProvNumDefault)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE procedurecode SET "+command
				+" WHERE CodeNum = "+POut.Long(procedureCode.CodeNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one ProcedureCode from the database.</summary>
		public static void Delete(long codeNum){
			string command="DELETE FROM procedurecode "
				+"WHERE CodeNum = "+POut.Long(codeNum);
			Db.NonQ(command);
		}

	}
}