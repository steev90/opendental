using System;
using System.Collections;

namespace OpenDentBusiness{
	
	///<summary>Allows customization of which fields display in various lists and grids.  For now, the only grid is ProgressNotes.  Will also eventually let users set column widths and translate titles.  For now, the selections are the same for all computers.</summary>
	public class DisplayField{
		///<summary>Primary key.</summary>
		public long DisplayFieldNum;
		///<summary>This is the internal name that OD uses to identify the field within this category.  This will be the default description if the user doesn't specify an alternate.</summary>
		public string InternalName;
		///<summary>Order to display in the grid or list. Every entry must have a unique itemorder.</summary>
		public int ItemOrder;
		///<summary>Optional alternate description to display for field.  Can be in another language.</summary>
		public string Description;
		///<summary>For grid columns, this lets user override the column width.  Especially useful for foreign languages.</summary>
		public int ColumnWidth;
		///<summary>Enum:DisplayFieldCategory.  If category is 0, then this is attached to a ChartView.</summary>
		public DisplayFieldCategory Category;
		///<summary>FK to chartview.ChartViewNum.  0 if attached to a category.</summary>
		public long ChartViewNum;


		public DisplayField(){

		}

		public DisplayField(string internalName,int columnWidth,DisplayFieldCategory category){
			this.InternalName=internalName;
			//this.Description=description;
			this.ColumnWidth=columnWidth;
			this.Description="";
			this.Category=category;
		}

		///<summary>Returns a copy.</summary>
		public DisplayField Copy() {
			return (DisplayField)this.MemberwiseClone();
		}

		public override string ToString(){
			return this.InternalName;
		}

	}


	//<summary>0=ProgressNotes</summary>
	//public enum DisplayFieldCategory {
	//	ProgressNotes
	//}
	

	


}









