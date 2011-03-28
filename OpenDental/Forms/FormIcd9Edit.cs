using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormIcd9Edit:Form {
		private ICD9 Icd9Cur;
		public bool IsNew;

		public FormIcd9Edit(ICD9 icd9Cur) {
			InitializeComponent();
			Lan.F(this);
			Icd9Cur=icd9Cur;
		}

		private void butOK_Click(object sender,EventArgs e) {
			Icd9Cur.ICD9Code=textCode.Text;
			Icd9Cur.Description=textDescription.Text;
			if(IsNew) {//Used the "+Add" button to open this form.
				if(ICD9s.Listt.Exists(CheckCode)) {//Must enter a unique code.
					MsgBox.Show(this,"You must choose a unique code.");
					return;
				}
				ICD9s.Insert(Icd9Cur);
			}
			else {
				ICD9s.Update(Icd9Cur);
			}
			DialogResult=DialogResult.OK;
		}

		private bool CheckCode(ICD9 icd9) {
			if(icd9.ICD9Code==Icd9Cur.ICD9Code) {
				return true;
			}
			else {
				return false;
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormIcd9Edit_Load(object sender,EventArgs e) {
			if(IsNew) {
				textCode.Enabled=true;
			}
			textCode.Text=Icd9Cur.ICD9Code;
			textDescription.Text=Icd9Cur.Description;
		}

		private void buttonDelete_Click(object sender,EventArgs e) {
			if(MsgBox.Show(this,true,"Delete this ICD9?")){
				if(IsNew) {
					DialogResult=DialogResult.Cancel;
					return;
				}
				try {
					ICD9s.Delete(Icd9Cur.ICD9Num);
					DialogResult=DialogResult.OK;
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
				}
			}
		}
	}
}