using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using CodeBase;

namespace OpenDentBusiness {
	public class MountItems {

		public static int Insert(MountItem mountItem) {
			string command="INSERT INTO mountitem (MountItemNum,MountNum,Xpos,Ypos,OrdinalPos,Width,Height) VALUES ("
				+"'"+POut.PInt(mountItem.MountItemNum)+"',"
				+"'"+POut.PInt(mountItem.MountNum)+"',"
				+"'"+POut.PInt(mountItem.Xpos)+"',"
				+"'"+POut.PInt(mountItem.Ypos)+"',"
				+"'"+POut.PInt(mountItem.OrdinalPos)+"',"
				+"'"+POut.PInt(mountItem.Width)+"',"
				+"'"+POut.PInt(mountItem.Height)+"')";
			return Db.NonQ(command,true);
		}

		public static int Update(MountItem mountItem) {
			string command="UPDATE mountitem SET "
				+"MountNum='"+POut.PInt(mountItem.MountNum)+"',"
				+"Xpos='"+POut.PInt(mountItem.Xpos)+"',"
				+"Ypos='"+POut.PInt(mountItem.Ypos)+"',"
				+"OrdinalPos='"+POut.PInt(mountItem.OrdinalPos)+"',"
				+"Width='"+POut.PInt(mountItem.Width)+"',"
				+"Height='"+POut.PInt(mountItem.Height)+"' "
				+"WHERE MountItemNum='"+POut.PInt(mountItem.MountItemNum)+"'";
			return Db.NonQ(command);
		}

		public static void Delete(MountItem mountItem) {
			string command="DELETE FROM mountitem WHERE MountItemNum='"+POut.PInt(mountItem.MountItemNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Converts the given datarow to a mountitem, assuming that the row represents a mountitem.</summary>
		public static MountItem Fill(DataRow mountItemRow){
			MountItem mountItem=new MountItem();
			mountItem.MountItemNum=PIn.PInt(mountItemRow["MountItemNum"].ToString());
			mountItem.MountNum=PIn.PInt(mountItemRow["MountNum"].ToString());
			mountItem.Xpos=PIn.PInt(mountItemRow["Xpos"].ToString());
			mountItem.Ypos=PIn.PInt(mountItemRow["Ypos"].ToString());
			mountItem.OrdinalPos=PIn.PInt(mountItemRow["OrdinalPos"].ToString());
			mountItem.Width=PIn.PInt(mountItemRow["Width"].ToString());
			mountItem.Height=PIn.PInt(mountItemRow["Height"].ToString());
			return mountItem;
		}

		///<summary>Returns the list of mount items associated with the given mount key.</summary>
		public static MountItem[] GetItemsForMount(int mountNum){
			string command="SELECT * FROM mountitem WHERE MountNum='"+POut.PInt(mountNum)+"' ORDER BY OrdinalPos";
			DataTable result=Db.GetTable(command);
			MountItem[] mountItems=new MountItem[result.Rows.Count];
			for(int i=0;i<mountItems.Length;i++){
				mountItems[i]=Fill(result.Rows[i]);
			}
			return mountItems;
		}

	}
}