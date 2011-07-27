/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.UI;

namespace OpenDental{

	///<summary></summary>
	public class ContrApptSingle : System.Windows.Forms.UserControl{
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary>Set on mouse down or from Appt module</summary>
		public static long ClickedAptNum;
		/// <summary>This is not the best place for this, but changing it now would cause bugs.  Set manually</summary>
		public static long SelectedAptNum;
		///<summary>True if this control is on the pinboard</summary>
		public bool ThisIsPinBoard;
		///<summary>Stores the shading info for the provider bars on the left of the appointments module</summary>
		public static int[][] ProvBar;
		///<summary>Stores the background bitmap for this control</summary>
		public Bitmap Shadow;
		private Font baseFont=new Font("Arial",8);
		private Font boldFont=new Font("Arial",8,FontStyle.Bold);
		///<summary>The actual slashes and Xs showing for the current view.</summary>
		private string patternShowing;
		///<summary>This is a datarow that stores most of the info necessary to draw appt.  It comes from the table obtained in Appointments.GetPeriodApptsTable().</summary>
		public DataRow DataRoww;
		///<summary>This table contains all appointment fields for all appointments in the period. It's obtained in Appointments.GetApptFields().</summary>
		public DataTable TableApptFields;
		///<summary>This table contains all appointment fields for all appointments in the period. It's obtained in Appointments.GetApptFields().</summary>
		public DataTable TablePatFields;
		///<summary>Indicator that account has procedures with no ins claim</summary>
		public bool FamHasInsNotSent;
		///<Summary>Will show the highlight around the edges.  For now, this is only used for pinboard.  The ordinary selected appt is set with SelectedAptNum.</Summary>
		public bool IsSelected;


		///<summary></summary>
		public ContrApptSingle(){
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			//Info=new InfoApt();
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
				if(Shadow!=null) {
					Shadow.Dispose();
				}
				if(baseFont!=null) {
					baseFont.Dispose();
				}
				if(boldFont!=null) {
					boldFont.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			// 
			// ContrApptSingle
			// 
			this.Name = "ContrApptSingle";
			this.Size = new System.Drawing.Size(85, 72);
			this.Load += new System.EventHandler(this.ContrApptSingle_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContrApptSingle_MouseDown);

		}
		#endregion
		
		///<summary></summary>
		protected override void OnPaint(PaintEventArgs pea){
			//Graphics grfx=pea.Graphics;
			//grfx.DrawImage(shadow,0,0);
		}

		
		///<summary>This is only called when viewing appointments on the Appt module.  For Planned apt and pinboard, use SetSize instead so that the location won't change.</summary>
		public void SetLocation(){
			if(ContrApptSheet.IsWeeklyView) {
				Width=(int)ContrApptSheet.ColAptWidth;
				Location=new Point(ConvertToX(),ConvertToY());
			}
			else{
				Location=new Point(ConvertToX()+2,ConvertToY());
				Width=ContrApptSheet.ColWidth-5;
			}
			SetSize();
		}

		///<summary>Used from SetLocation. Also used for Planned apt and pinboard instead of SetLocation so that the location won't be altered.</summary>
		public void SetSize(){
			patternShowing=GetPatternShowing(DataRoww["Pattern"].ToString());
			//height is based on original 5 minute pattern. Might result in half-rows
			Height=DataRoww["Pattern"].ToString().Length*ContrApptSheet.Lh*ContrApptSheet.RowsPerIncr;
			//if(ContrApptSheet.TwoRowsPerIncrement){
			//	Height=Height*2;
			//}
			if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10){
				Height=Height/2;
			}
			if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15){
				Height=Height/3;
			}
			//if(ThisIsPinBoard){
				//hack:  it's at 99x96 right now
				//if(Height > ContrAppt.PinboardSize.Height-4)
				//	Height=ContrAppt.PinboardSize.Height-4;
				//if(Width > 99-2){
			//	Width=99-2;
				//}
			//}
		}
		
		///<summary>Called from SetLocation to establish X position of control.</summary>
		private int ConvertToX(){
			if(ContrApptSheet.IsWeeklyView) {
				//the next few lines are because we start on Monday instead of Sunday
				int dayofweek=(int)PIn.DateT(DataRoww["AptDateTime"].ToString()).DayOfWeek-1;
				if(dayofweek==-1) {
					dayofweek=6;
				}
				return ContrApptSheet.TimeWidth
					+ContrApptSheet.ColDayWidth*(dayofweek)+1
					+(int)(ContrApptSheet.ColAptWidth*(float)ApptViewItemL.GetIndexOp(PIn.Long(DataRoww["Op"].ToString())));
			}
			else {
				return ContrApptSheet.TimeWidth+ContrApptSheet.ProvWidth*ContrApptSheet.ProvCount
					+ContrApptSheet.ColWidth*(ApptViewItemL.GetIndexOp(PIn.Long(DataRoww["Op"].ToString())))+1;
					//Info.MyApt.Op))+1;
			}
		}

		///<summary>Called from SetLocation to establish Y position of control.  Also called from ContrAppt.RefreshDay when determining provBar markings. Does not round to the nearest row.</summary>
		public int ConvertToY(){
			DateTime aptDateTime=PIn.DateT(DataRoww["AptDateTime"].ToString());
			int retVal=(int)(((double)aptDateTime.Hour*(double)60
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				+(double)aptDateTime.Minute
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				)*(double)ContrApptSheet.Lh*ContrApptSheet.RowsPerIncr);
			return retVal;
		}

		///<summary>This converts the dbPattern in 5 minute interval into the pattern that will be viewed based on RowsPerIncrement and AppointmentTimeIncrement.  So it will always depend on the current view.Therefore, it should only be used for visual display purposes rather than within the FormAptEdit. If height of appointment allows a half row, then this includes an increment for that half row.</summary>
		public static string GetPatternShowing(string dbPattern){
			StringBuilder strBTime=new StringBuilder();
			for(int i=0;i<dbPattern.Length;i++){
				for(int j=0;j<ContrApptSheet.RowsPerIncr;j++){
					strBTime.Append(dbPattern.Substring(i,1));
				}
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10) {
					i++;//skip
				}
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15){
					i++;
					i++;//skip two
				}
			}
			return strBTime.ToString();
		}

		///<summary>It is planned to move some of this logic to OnPaint and use a true double buffer.</summary>
		public void CreateShadow(){
			if(this.Parent is ContrApptSheet) {
				bool isVisible=false;
				for(int j=0;j<ApptViewItemL.VisOps.Count;j++) {
					if(this.DataRoww["Op"].ToString()==ApptViewItemL.VisOps[j].OperatoryNum.ToString()){
						isVisible=true;
					}
				}
				if(!isVisible){
					return;
				}
			}
			if(Shadow!=null) {
				Shadow.Dispose();
				Shadow=null;
			}
			if(Width<4){
				return;
			}
			if(Height<4){
				return;
			}
			Shadow=new Bitmap(Width,Height);
			using(Graphics g=Graphics.FromImage(Shadow)) {
				ApptSingleDrawing.DrawEntireAppt(g,DataRoww,Width,Height,patternShowing,ContrApptSheet.Lh,ContrApptSheet.RowsPerIncr,
					IsSelected,ThisIsPinBoard,SelectedAptNum,ApptViewItemL.ApptRows,ApptViewItemL.ApptViewCur,TableApptFields,TablePatFields);
				this.BackgroundImage=Shadow;
			}
		}

		private void ContrApptSingle_Load(object sender, System.EventArgs e){
			/*
			if(Info.IsNext){
				Width=110;
				//don't change location
			}
			else{
				Location=new Point(ConvertToX(),ConvertToY());
				Width=ContrApptSheet.ColWidth-1;
				Height=Info.MyApt.Pattern.Length*ContrApptSheet.Lh;
			}
			*/
		}

		private void ContrApptSingle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			ClickedAptNum=PIn.Long(DataRoww["AptNum"].ToString());
		}



	}//end class
}//end namespace
