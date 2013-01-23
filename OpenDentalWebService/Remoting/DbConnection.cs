﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using OpenDentBusiness;

namespace OpenDentalWebService.Remoting {
	///<summary>This class is only used the first time the web service is turned on.  It is used to read the xml config file that contains the connection information and return a DataConnection.</summary>
	public class DbConnection {

		///<summary>Reads the xml config file that contains the connection information for the correct database and returns a DataConnection to it.</summary>
		public static DataConnection GetDataConnectionFromConfigFile() {
			string xmlPath=Path.Combine(HttpRuntime.AppDomainAppPath,"OpenDentalWebConfig.xml");
			if(!File.Exists(xmlPath)) {
				throw new ApplicationException("Cannot find config file: "+xmlPath);
			}
			try {
				string server="";
				string db="";
				string mysqlUser="";
				string mysqlUserLow="";
				string mysqlPassword="";
				string mysqlPasswordLow="";
				string connectionString="";
				DataConnection con=new DataConnection();
				XmlDocument document=new XmlDocument();
				document.Load(xmlPath);
				XPathNavigator navigator=document.CreateNavigator();
				XPathNavigator nav=null;
				//DatabaseType
				nav=navigator.SelectSingleNode("//DatabaseType");
				DatabaseType dbType=DatabaseType.MySql;
				if(nav!=null && nav.Value=="Oracle") {
					dbType=DatabaseType.Oracle;
				}
				//ConnectionString
				nav=navigator.SelectSingleNode("//ConnectionString");
				if(nav!=null) {
					connectionString=nav.Value;
				}
				//DatabaseConnection
				nav=navigator.SelectSingleNode("//DatabaseConnection");
				if(nav!=null) {
					try {
						server=nav.SelectSingleNode("ComputerName").Value;
						db=nav.SelectSingleNode("Database").Value;
						mysqlUser=nav.SelectSingleNode("User").Value;
						mysqlPassword=nav.SelectSingleNode("Password").Value;
						mysqlUserLow=nav.SelectSingleNode("UserLow").Value;
						mysqlPasswordLow=nav.SelectSingleNode("PasswordLow").Value;
					}
					catch(Exception) {
						throw new Exception("Malformed OpenDentalWebConfig file.  DatabaseConnection node needs to have ComputerName, Database, User, Password, UserLow and PasswordLow child nodes.");
					}
				}
				if(connectionString!="") {
					con.SetDb(connectionString,"",DataConnection.DBtype);
				}
				else {
					con.SetDb(server,db,mysqlUser,mysqlPassword,mysqlUserLow,mysqlPasswordLow,dbType);
				}
				return con;
			}
			catch(Exception ex) {
				throw new Exception("Error making a connection to the database with the settings in the OpenDentalWebConfig.xml:\r\n"+ex.Message);
			}
		}



	}
}