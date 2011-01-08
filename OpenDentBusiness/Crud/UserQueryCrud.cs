//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class UserQueryCrud {
		///<summary>Gets one UserQuery object from the database using the primary key.  Returns null if not found.</summary>
		internal static UserQuery SelectOne(long queryNum){
			string command="SELECT * FROM userquery "
				+"WHERE QueryNum = "+POut.Long(queryNum);
			List<UserQuery> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one UserQuery object from the database using a query.</summary>
		internal static UserQuery SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UserQuery> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of UserQuery objects from the database using a query.</summary>
		internal static List<UserQuery> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<UserQuery> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<UserQuery> TableToList(DataTable table){
			List<UserQuery> retVal=new List<UserQuery>();
			UserQuery userQuery;
			for(int i=0;i<table.Rows.Count;i++) {
				userQuery=new UserQuery();
				userQuery.QueryNum   = PIn.Long  (table.Rows[i]["QueryNum"].ToString());
				userQuery.Description= PIn.String(table.Rows[i]["Description"].ToString());
				userQuery.FileName   = PIn.String(table.Rows[i]["FileName"].ToString());
				userQuery.QueryText  = PIn.String(table.Rows[i]["QueryText"].ToString());
				retVal.Add(userQuery);
			}
			return retVal;
		}

		///<summary>Inserts one UserQuery into the database.  Returns the new priKey.</summary>
		internal static long Insert(UserQuery userQuery){
			return Insert(userQuery,false);
		}

		///<summary>Inserts one UserQuery into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(UserQuery userQuery,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				userQuery.QueryNum=ReplicationServers.GetKey("userquery","QueryNum");
			}
			string command="INSERT INTO userquery (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="QueryNum,";
			}
			command+="Description,FileName,QueryText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(userQuery.QueryNum)+",";
			}
			command+=
				 "'"+POut.String(userQuery.Description)+"',"
				+"'"+POut.String(userQuery.FileName)+"',"
				+DbHelper.ParamChar+"paramQueryText)";
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,userQuery.QueryText);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramQueryText);
			}
			else {
				userQuery.QueryNum=Db.NonQ(command,true,paramQueryText);
			}
			return userQuery.QueryNum;
		}

		///<summary>Updates one UserQuery in the database.</summary>
		internal static void Update(UserQuery userQuery){
			string command="UPDATE userquery SET "
				+"Description= '"+POut.String(userQuery.Description)+"', "
				+"FileName   = '"+POut.String(userQuery.FileName)+"', "
				+"QueryText  =  "+DbHelper.ParamChar+"paramQueryText "
				+"WHERE QueryNum = "+POut.Long(userQuery.QueryNum);
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,userQuery.QueryText);
			Db.NonQ(command,paramQueryText);
		}

		///<summary>Updates one UserQuery in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(UserQuery userQuery,UserQuery oldUserQuery){
			string command="";
			if(userQuery.Description != oldUserQuery.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(userQuery.Description)+"'";
			}
			if(userQuery.FileName != oldUserQuery.FileName) {
				if(command!=""){ command+=",";}
				command+="FileName = '"+POut.String(userQuery.FileName)+"'";
			}
			if(userQuery.QueryText != oldUserQuery.QueryText) {
				if(command!=""){ command+=",";}
				command+="QueryText = "+DbHelper.ParamChar+"paramQueryText";
			}
			if(command==""){
				return;
			}
			if(userQuery.QueryText==null) {
				userQuery.QueryText="";
			}
			OdSqlParameter paramQueryText=new OdSqlParameter("paramQueryText",OdDbType.Text,userQuery.QueryText);
			command="UPDATE userquery SET "+command
				+" WHERE QueryNum = "+POut.Long(userQuery.QueryNum);
			Db.NonQ(command,paramQueryText);
		}

		///<summary>Deletes one UserQuery from the database.</summary>
		internal static void Delete(long queryNum){
			string command="DELETE FROM userquery "
				+"WHERE QueryNum = "+POut.Long(queryNum);
			Db.NonQ(command);
		}

	}
}