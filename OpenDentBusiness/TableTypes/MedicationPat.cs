using System;

namespace OpenDentBusiness{

	///<summary>Links medications to patients.  For ehr, some of these can be considered 'medication orders', but only if they contain a PatNote (instructions), a ProvNum, and a DateStart.</summary>
	[Serializable]
	public class MedicationPat:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long MedicationPatNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>FK to medication.MedicationNum.  If 0, implies that the medication order came from NewCrop.</summary>
		public long MedicationNum;
		///<summary>Medication notes specific to this patient.</summary>
		public string PatNote;
		///<summary>The last date and time this row was altered.  Not user editable.  Will be set to NOW by OD if this patient gets an OnlinePassword assigned.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		///<summary>Date that the medication was started.  Can be minval if unknown.</summary>
		public DateTime DateStart;
		///<summary>Date that the medication was stopped.  Can be minval if unknown.  If minval, then the medication is not "discontinued".  If prior to today, then the medication is "discontinued".  If today or a future date, then not discontinued yet.</summary>
		public DateTime DateStop;
		///<summary>FK to provider.ProvNum. Can be 0. Gets set to the patient's primary provider when adding a new med.  If adding the med from EHR, gets set to the ProvNum of the logged-in user.</summary>
		public long ProvNum;
		///<summary>Only use when MedicationNum=0.  For medication orders pulled back from NewCrop during synch.</summary>
		public string MedDescript;
		///<summary>For NewCrop medical orders, corresponds to the RxCui of the prescription (NewCrop only returns a value sometimes).  Otherwise, this field is synched with the medication.RxCui field based on medication.MedicationNum.  We should have used a string type.  The only purpose of this field is so that when CCDs are created, we have structured data to put in the XML, not just plain text.  Allergies exported in CCD do not look at this table, but only at the medication table.  Medications require MedicationPat.RxCui or Medication.RxCui to be exported on CCD.</summary>
		public long RxCui;
		///<summary>Only use when MedicationNum=0.  For medication orders pulled back from NewCrop during synch.  The NewCrop GUID which uniquely identifies the prescription corresponding to the medical order. Allows us to update existing NewCrop medical orders when refreshing prescriptions in the Chart (similar to how prescriptions are updated).</summary>
		public string NewCropGuid;
		///<summary>If NewCrop is used to prescribe a medication, a medication order is created automatically within Open Dental.  If a provider is logged in, then this is CPOE (Computerized Provider Order Entry), and this will be true.  If a staff person is logged in, or if a provider enters an Rx through Open Dental instead of NewCrop,then this is non-CPOE, so false.</summary>
		public bool IsCpoe;
	}


	





}










