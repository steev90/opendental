using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EhrLaboratories;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabs {

		#region HL7 Message Processing

		///<summary>Given an HL7 message, will attempt to fin the corresponding patient.  Will return null if not found.</summary>
		/// <param name="message"></param>
		public static Patient FindAttachedPatient(string message) {
			//TODO: implement this patient select function if needed.
			return null;
		}

		public static List<EhrLab> ProcessHl7Message(string message) {
			return ProcessHl7Message(message,null, false);
		}

		public static List<EhrLab> ProcessHl7Message(string message,Patient patCur) {
			return ProcessHl7Message(message,patCur,false);
		}

		///<summary>Allows message to be processed and imported and will initialize empty lists instead of leaving them null.</summary>
		///<param name="IsImport">Set to true if using for import preview.</param>
		public static List<EhrLab> ProcessHl7Message(string message,bool IsImport) {
			return ProcessHl7Message(message,null, IsImport);
		}

		///<summary>Surround with Try/Catch.  Processes an HL7 message into an EHRLab object.</summary>
		public static List<EhrLab> ProcessHl7Message(string message, Patient patCur, bool isImport){
			//Patient patcur;
			List<EhrLab> listRetVal=new List<EhrLab>();
			EhrLab ehrLabCur=new EhrLab();
			if(!message.StartsWith("MSH")){
				//cannnot parse message without message header at the very least
				throw new Exception("MSH segment not found.");
			}
			string[] segments=message.Split(new string[] { "\r\n" },StringSplitOptions.None);
			string[] fields;
			string noteSegment="";//used to attach notes to the right objects.
			for(int s=0;s<segments.Length;s++) {
				string segment=segments[s];
				fields=segment.Split('|');
				switch(fields[0]) {//Segment Identifier.
					case "MSH":
						if(fields[8]!="ORU^R01^ORU_R01") {
							throw new Exception("MSH.9 contained wrong value.  \""+fields[8]+"\" was found, \"ORU^R01^ORU_R01\" was expected.");
						}
						if(fields[11]!="2.5.1") {
							throw new Exception("MSH.12 shows message version \""+fields[11]+"\", only version \"2.5.1\" is currently supported.");
						}
						containsRequiredSegmentsHelper(message); //validate required segments here, after we have verified this is an ORU_R01 message
						if(fields[20].Split('~').Length==0) {
							throw new Exception("MSH.21 does not contain any values, the LRI_GU_RU_Profile value \"2.16.840.1.113883.9.17\" is expected.");
						}
						for(int i=0;i<fields[20].Split('~').Length;i++) {
							if(i==fields[20].Split('~').Length) {
								throw new Exception("MSH.21 ("+i+") indicates sender's message does not conform to LRI_GU_RU_Profile \"2.16.840.1.113883.9.17\"");
							}
							if(fields[20].Split('~')[i]=="2.16.840.1.113883.9.17") {
								break;//found expected value.
							}
						}
						break;
					//case "SFT": //Software Segment
					//	break;
					case "PID":
						noteSegment="PID";
						for(int i=0;i<fields[3].Split('~').Length;i++) {
							if(patCur==null) {
								patCur=Patients.GetByGUID(fields[3].Split('~')[i].Split('^')[0],								//ID Number
																					fields[3].Split('~')[i].Split('^')[3].Split('&')[1]);	//Assigning Authority ID 
							}
							if(patCur!=null) {
								ehrLabCur.PatNum=patCur.PatNum;
							}
							else {
								ehrLabCur.PatNum=0;
								//if(i==fields[3].Split('~').Length) {//we have checked all patient ID's and none of them were a valid patnum in our DB.
								//	throw new Exception("PID.3 does not contain a known patient ID.");//we should have an option to manually associate lab results with a patient record, in the UI layer.
								//}
							}
						}
						//all other PID segments are informative, PID.3 is the only one we need to process.
						break;
					//case "PD1": //patient demographics
					//	break;
					//case "NK1": //Next of Kin/Associated Parties
					//	break;
					//case "PV1": //Patient Visit
					//	break;
					//case "PV2": //Patient Visit addiotional information
					//	break;
					case "ORC":
						//Each new ORC segment designates a new EhrLabOrder attached to the same patient.
						if(ehrLabCur.PlacerOrderNum!=null || ehrLabCur.FillerOrderNum!=null) {//these fields are filled by the ORC segment, if they are blank, this is the first ORC segment encountered.
							listRetVal.Add(ehrLabCur);
							ehrLabCur=new EhrLab();
							ehrLabCur.PatNum=listRetVal[0].PatNum;
						}
						try {
							ehrLabCur.OrderControlCode=(HL70119)Enum.Parse(typeof(HL70119),fields[1]);
						}
						catch {
							throw new Exception("ORC.1 does not contain a valid Order Control Code (HL70119 value set).");
						}
						//Placer Order Num
						if(fields[2].Length!=0) {//optional field, length may be 0 if field was ommitted.
							ehrLabCur.PlacerOrderNum							=fields[2].Split('^')[0];
							ehrLabCur.PlacerOrderNamespace				=fields[2].Split('^')[1];
							ehrLabCur.PlacerOrderUniversalID			=fields[2].Split('^')[2];
							ehrLabCur.PlacerOrderUniversalIDType	=fields[2].Split('^')[3];
						}
						//Filler Order Num
						ehrLabCur.FillerOrderNum							=fields[3].Split('^')[0];
						ehrLabCur.FillerOrderNamespace				=fields[3].Split('^')[1];
						ehrLabCur.FillerOrderUniversalID			=fields[3].Split('^')[2];
						ehrLabCur.FillerOrderUniversalIDType	=fields[3].Split('^')[3];
						//Filler Group Num
						if(fields[4].Length!=0) {
							ehrLabCur.PlacerGroupNum							=fields[4].Split('^')[0];
							ehrLabCur.PlacerGroupNamespace				=fields[4].Split('^')[1];
							ehrLabCur.PlacerGroupUniversalID			=fields[4].Split('^')[2];
							ehrLabCur.PlacerGroupUniversalIDType	=fields[4].Split('^')[3];
						}
						//Ordering Provider
						ehrLabCur.OrderingProviderID															=fields[12].Split('^')[0];
						ehrLabCur.OrderingProviderLName													=fields[12].Split('^')[1];
						ehrLabCur.OrderingProviderFName													=fields[12].Split('^')[2];
						ehrLabCur.OrderingProviderMiddleNames										=fields[12].Split('^')[3];
						ehrLabCur.OrderingProviderSuffix													=fields[12].Split('^')[4];
						ehrLabCur.OrderingProviderPrefix													=fields[12].Split('^')[5];
						ehrLabCur.OrderingProviderAssigningAuthorityNamespaceID	=fields[12].Split('^')[8].Split('&')[0];
						ehrLabCur.OrderingProviderAssigningAuthorityUniversalID	=fields[12].Split('^')[8].Split('&')[1];
						ehrLabCur.OrderingProviderAssigningAuthorityIDType				=fields[12].Split('^')[8].Split('&')[2];
						try {
							ehrLabCur.OrderingProviderNameTypeCode=(HL70200)Enum.Parse(typeof(HL70200),fields[12].Split('^')[9]);
						}
						catch {
							throw new Exception("ORC.12.10 does not contain a valid Name Type Code (HL70200 value set).");
						}
						try {
							ehrLabCur.OrderingProviderIdentifierTypeCode	=(HL70203)Enum.Parse(typeof(HL70203),fields[12].Split('^')[12]);
						}
						catch {
							throw new Exception("ORC.12.13 does not contain a valid Identifier Type Code (HL70203 value set).");
						}
						break;
					case "OBR":
						noteSegment="OBR";
						ehrLabCur.SetIdOBR=PIn.Long(fields[1]);
						if(isImport) {
							ehrLabCur.ListEhrLabNotes=new List<EhrLabNote>();
							ehrLabCur.ListEhrLabResults=new List<EhrLabResult>();
						}
						//OBR order num should always be identical to OCR order number//Not true.
						if(ehrLabCur.FillerOrderNum!=fields[3].Split('^')[0]) {
							throw new Exception("Filler order numbers in OCR.3 and OBR.3 segments do not match.");
						}
						//Universal Service ID
						ehrLabCur.UsiID											=																			fields[4].Split('^')[0];
						ehrLabCur.UsiText										=																			fields[4].Split('^')[1];
						try { ehrLabCur.UsiCodeSystemName			=																			fields[4].Split('^')[2]; }
						catch { }
						try { ehrLabCur.UsiIDAlt							=																			fields[4].Split('^')[3]; }
						catch { }
						try { ehrLabCur.UsiTextAlt						=																			fields[4].Split('^')[4]; }
						catch { }
						try { ehrLabCur.UsiCodeSystemNameAlt	=																			fields[4].Split('^')[5]; }
						catch { }
						try { ehrLabCur.UsiTextOriginal				=																			fields[4].Split('^')[6]; }
						catch { }
						//Observation Date Time
						ehrLabCur.ObservationDateTimeStart		=fields[7];
						if(fields[8].Length!=0) {
							ehrLabCur.ObservationDateTimeEnd		=fields[8];
						}
						try { ehrLabCur.SpecimenActionCode		=(HL70065)Enum.Parse(typeof(HL70065),fields[11]); }
						catch { }
						if(isImport) {
							ehrLabCur.ListRelevantClinicalInformations=new List<EhrLabClinicalInfo>();
						}
						for(int i=0;i<fields[13].Split('~').Length;i++) {
							if(fields[13].Length==0) {
								break;//nothing to process
							}
							string tempClinInfo=fields[13].Split('~')[i];
							EhrLabClinicalInfo ehrLabClinicalInfo=new EhrLabClinicalInfo();
							ehrLabClinicalInfo.ClinicalInfoID											=tempClinInfo.Split('^')[0];
							try { ehrLabClinicalInfo.ClinicalInfoText										=tempClinInfo.Split('^')[1]; }
							catch { }
							try { ehrLabClinicalInfo.ClinicalInfoCodeSystemName					=tempClinInfo.Split('^')[2]; }
							catch { }
							try { ehrLabClinicalInfo.ClinicalInfoIDAlt									=tempClinInfo.Split('^')[3]; }
							catch { }
							try { ehrLabClinicalInfo.ClinicalInfoTextAlt								=tempClinInfo.Split('^')[4]; }
							catch { }
							try { ehrLabClinicalInfo.ClinicalInfoCodeSystemNameAlt			=tempClinInfo.Split('^')[5]; }
							catch { }
							try { ehrLabClinicalInfo.ClinicalInfoTextOriginal						=tempClinInfo.Split('^')[6]; }
							catch { }
							ehrLabCur.ListRelevantClinicalInformations.Add(ehrLabClinicalInfo);
						}
						//OBR 16; Ordering Provider same as OCR. //not validating or checking at this time.
						ehrLabCur.ResultDateTime=fields[22];
						if(fields.Length<=25) { break; }//likely that fields beyond this are left out.
						try { ehrLabCur.ResultStatus												=(HL70123)Enum.Parse(typeof(HL70123),fields[25]); }
						catch { }
						//Parent Result
						if(fields.Length<=26) { break; }//likely that fields beyond this are left out.
						ehrLabCur.ParentObservationID												=fields[26].Split('^')[0].Split('&')[0];
						try { ehrLabCur.ParentObservationText									=fields[26].Split('^')[0].Split('&')[1]; }
						catch { }
						try { ehrLabCur.ParentObservationCodeSystemName				=fields[26].Split('^')[0].Split('&')[2]; }
						catch { }
						try { ehrLabCur.ParentObservationIDAlt								=fields[26].Split('^')[0].Split('&')[3]; }
						catch { }
						try { ehrLabCur.ParentObservationTextAlt							=fields[26].Split('^')[0].Split('&')[4]; }
						catch { }
						try { ehrLabCur.ParentObservationCodeSystemNameAlt		=fields[26].Split('^')[0].Split('&')[5]; }
						catch { }
						try { ehrLabCur.ParentObservationTextOriginal					=fields[26].Split('^')[0].Split('&')[6]; }
						catch { }
						try { ehrLabCur.ParentObservationSubID								=fields[26].Split('^')[1]; }
						catch { }
						//Results Copy To
						if(isImport) {
							ehrLabCur.ListEhrLabResultsCopyTo=new List<EhrLabResultsCopyTo>();
						}
						if(fields.Length<28) { break; }//likely that fields beyond this are left out.
						for(int i=0;i<fields[28].Split('~').Length;i++) {
							EhrLabResultsCopyTo tempRCT=new EhrLabResultsCopyTo();
							string stringRCT=fields[28].Split('~')[i];
							if(stringRCT.Length==0) {
								continue;//usually only if nothing was sent, but also if an empty field was sent.
							}
							tempRCT.CopyToID															=stringRCT.Split('^')[0];
							tempRCT.CopyToLName														=stringRCT.Split('^')[1];
							tempRCT.CopyToFName														=stringRCT.Split('^')[2];
							tempRCT.CopyToMiddleNames											=stringRCT.Split('^')[3];
							tempRCT.CopyToSuffix													=stringRCT.Split('^')[4];
							tempRCT.CopyToPrefix													=stringRCT.Split('^')[5];
							tempRCT.CopyToAssigningAuthorityUniversalID		=stringRCT.Split('^')[8].Split('&')[0];
							tempRCT.CopyToAssigningAuthorityNamespaceID		=stringRCT.Split('^')[8].Split('&')[1];
							tempRCT.CopyToAssigningAuthorityIDType				=stringRCT.Split('^')[8].Split('&')[2];
							try {
								tempRCT.CopyToNameTypeCode=(HL70200)Enum.Parse(typeof(HL70200),stringRCT.Split('^')[9]);
							}
							catch {
								throw new Exception("ORC.12.10 does not contain a valid Name Type Code (HL70200 value set).");
							}
							try {
								ehrLabCur.OrderingProviderIdentifierTypeCode	=(HL70203)Enum.Parse(typeof(HL70203),stringRCT.Split('^')[12]);
							}
							catch {
								throw new Exception("ORC.12.13 does not contain a valid Identifier Type Code (HL70203 value set).");
							}
							ehrLabCur.ListEhrLabResultsCopyTo.Add(tempRCT);
						}
						//Parent Lab
						if(fields.Length<29) { break; }//likely that fields beyond this are left out.
						ehrLabCur.ParentPlacerOrderNum												=fields[29].Split('^')[0].Split('&')[0];
						try { ehrLabCur.ParentPlacerOrderNamespace							=fields[29].Split('^')[0].Split('&')[1]; }
						catch { }
						try { ehrLabCur.ParentPlacerOrderUniversalID						=fields[29].Split('^')[0].Split('&')[2]; }
						catch { }
						try { ehrLabCur.ParentPlacerOrderUniversalIDType				=fields[29].Split('^')[0].Split('&')[3]; }
						catch { }
						try { ehrLabCur.ParentFillerOrderNum										=fields[29].Split('^')[1].Split('&')[0]; }
						catch { }
						try { ehrLabCur.ParentFillerOrderNamespace							=fields[29].Split('^')[1].Split('&')[1]; }
						catch { }
						try { ehrLabCur.ParentFillerOrderUniversalID						=fields[29].Split('^')[1].Split('&')[2]; }
						catch { }
						try { ehrLabCur.ParentFillerOrderUniversalIDType				=fields[29].Split('^')[1].Split('&')[3]; }
						catch { }
						if(fields.Length<31) {
							break;//next segment. all additional fields were omitted from this one.
						}
						if(fields.Length<49) { break; }//likely that fields beyond this are left out.
						//result Handling
						ehrLabCur.ListEhrLabResultsHandlingF										=fields[49].Contains("F");
						ehrLabCur.ListEhrLabResultsHandlingN										=fields[49].Contains("N");
						break;
					case "NTE":
						//Each not can contain any number of comments, these comments will be carrot delimited. That will be handled later in the UI.  Just store this NTE Segment in an EHRLabNote
						EhrLabNote ehrNote=new EhrLabNote();
						//todo:No SetIDNTE?
						ehrNote.Comments=fields[3];
						switch(noteSegment) {
							case "OBX":
								//Attach note to observation.
								ehrLabCur.ListEhrLabResults[ehrLabCur.ListEhrLabResults.Count-1].ListEhrLabResultNotes.Add(ehrNote);
								break;
							case "PID":
							case "OBR":
							default:
								//if PID, OBR, or Other just attach note to the lab.
								ehrLabCur.ListEhrLabNotes.Add(ehrNote);
								break;
						}
						break;
					case "TQ1":
						ehrLabCur.TQ1SetId=PIn.Long(fields[1]);
						ehrLabCur.TQ1DateTimeStart=fields[7];
						ehrLabCur.TQ1DateTimeEnd=fields[8];
						break;
					//case "TQ2": //Timing/Quantity Order Sequence
					//	break;
					//case "CTD": //Contact Data
					//	break;
					case "OBX":
						noteSegment="OBX";
						//if(ehrLabCur.ListEhrLabResults==null) {
						//	ehrLabCur.ListEhrLabResults=new List<EhrLabResult>();
						//}
						EhrLabResult labResult=new EhrLabResult();
						labResult.SetIdOBX=PIn.Long(fields[1]);
						try { labResult.ValueType=(HL70125)Enum.Parse(typeof(HL70125),fields[2]); }
						catch { }
						//Lab Result Observation Identifier (LOINC)
						labResult.ObservationIdentifierID									=fields[3].Split('^')[0];
						try { labResult.ObservationIdentifierText								=fields[3].Split('^')[1]; }
						catch { }
						try { labResult.ObservationIdentifierCodeSystemName			=fields[3].Split('^')[2]; }
						catch { }
						try { labResult.ObservationIdentifierIDAlt							=fields[3].Split('^')[3]; }
						catch { }
						try { labResult.ObservationIdentifierTextAlt						=fields[3].Split('^')[4]; }
						catch { }
						try { labResult.ObservationIdentifierCodeSystemNameAlt	=fields[3].Split('^')[5]; }
						catch { }
						if(fields[3].Split('^').Length>6) {
							labResult.ObservationIdentifierTextOriginal							=fields[3].Split('^')[6];
						}
						labResult.ObservationIdentifierSub=fields[4];
						//Observation Value
						switch(labResult.ValueType) {
							case HL70125.CE:
							case HL70125.CWE:
								labResult.ObservationValueCodedElementID										=fields[5].Split('^')[0];
								try { labResult.ObservationValueCodedElementText									=fields[5].Split('^')[1]; }
								catch { }
								try { labResult.ObservationValueCodedElementCodeSystemName				=fields[5].Split('^')[2]; }
								catch { }
								try { labResult.ObservationValueCodedElementIDAlt									=fields[5].Split('^')[3]; }
								catch { }
								try { labResult.ObservationValueCodedElementTextAlt								=fields[5].Split('^')[4]; }
								catch { }
								try { labResult.ObservationValueCodedElementCodeSystemNameAlt			=fields[5].Split('^')[5]; }
								catch { }
								if(labResult.ValueType==HL70125.CWE) {
									labResult.ObservationValueCodedElementTextOriginal=fields[5].Split('^')[6];
								}
								break;
							case HL70125.DT:
							case HL70125.TS:
								labResult.ObservationValueDateTime=fields[5];
								break;
							case HL70125.FT://formatted text
							case HL70125.ST://string
							case HL70125.TX://text
								labResult.ObservationValueText=fields[5];
								break;
							case HL70125.NM:
								//data may contain positive or negative sign.  Below, the sign is handled first, and then multiplied by PIn.Double(val)
								labResult.ObservationValueNumeric=  (fields[5].Contains("-")?-1f:1f)  *  PIn.Double(fields[5].Trim('+').Trim('-'));
								break;
							case HL70125.SN:
								labResult.ObservationValueComparator						=						fields[5].Split('^')[0];
								try { labResult.ObservationValueNumber1						=PIn.Double(fields[5].Split('^')[1]); }
								catch { }//optional, may be a null reference
								try { labResult.ObservationValueSeparatorOrSuffix	=						fields[5].Split('^')[2]; }
								catch { }//optional, may be a null reference
								try { labResult.ObservationValueNumber2						=PIn.Double(fields[5].Split('^')[3]); }
								catch { }//optional, may be a null reference
								break;
							case HL70125.TM:
								labResult.ObservationValueTime=PIn.Time(fields[5]);
								break;
						}
						//Units
						if(fields[6].Length!=0) {
							labResult.UnitsID											=fields[6].Split('^')[0];
							labResult.UnitsText										=fields[6].Split('^')[1];
							try { labResult.UnitsCodeSystemName			=fields[6].Split('^')[2]; }
							catch { }
							try { labResult.UnitsIDAlt							=fields[6].Split('^')[3]; }
							catch { }
							try { labResult.UnitsTextAlt						=fields[6].Split('^')[4]; }
							catch { }
							try { labResult.UnitsCodeSystemNameAlt	=fields[6].Split('^')[5]; }
							catch { }
							try { labResult.UnitsTextOriginal				=fields[6].Split('^')[6]; }
							catch { }
						}

						labResult.referenceRange=fields[7];
						labResult.AbnormalFlags=fields[8].Replace('~',',');//TODO: may need additional formatting/testing
						try { labResult.ObservationResultStatus	=(HL70085)Enum.Parse(typeof(HL70085),fields[11]); }
						catch { }
						labResult.ObservationDateTime=fields[14];
						labResult.AnalysisDateTime=fields[19];
						//performing organization Name (with additional info)
						labResult.PerformingOrganizationName=fields[23].Split('^')[0];
						labResult.PerformingOrganizationNameAssigningAuthorityNamespaceId			=fields[23].Split('^')[5].Split('&')[0];
						labResult.PerformingOrganizationNameAssigningAuthorityUniversalId			=fields[23].Split('^')[5].Split('&')[1];
						labResult.PerformingOrganizationNameAssigningAuthorityUniversalIdType	=fields[23].Split('^')[5].Split('&')[2];
						try { labResult.PerformingOrganizationIdentifierTypeCode	=(HL70203)Enum.Parse(typeof(HL70203),fields[23].Split('^')[7]); }
						catch { }
						labResult.PerformingOrganizationIdentifier=fields[23].Split('^')[9];
						//Performing Organization Address
						labResult.PerformingOrganizationAddressStreet									=fields[24].Split('^')[0].Split('&')[0];
						try { labResult.PerformingOrganizationAddressOtherDesignation		=fields[24].Split('^')[1]; }
						catch { }
						try { labResult.PerformingOrganizationAddressCity								=fields[24].Split('^')[2]; }
						catch { }
						try { labResult.PerformingOrganizationAddressStateOrProvince		=(USPSAlphaStateCode)Enum.Parse(typeof(USPSAlphaStateCode),fields[24].Split('^')[3]); }
						catch { }
						try { labResult.PerformingOrganizationAddressZipOrPostalCode		=fields[24].Split('^')[4]; }
						catch { }
						try { labResult.PerformingOrganizationAddressCountryCode				=fields[24].Split('^')[5]; }
						catch { }
						try { labResult.PerformingOrganizationAddressAddressType				=(HL70190)Enum.Parse(typeof(HL70190),fields[24].Split('^')[6]); }
						catch { }
						try { labResult.PerformingOrganizationAddressCountyOrParishCode	=fields[24].Split('^')[7]; }
						catch { }
						//Performing Organization Medical Director
						if(fields.Length<=25) {
							break;//next segment. this one is finished.
						}
						labResult.MedicalDirectorID										=fields[25].Split('^')[0];
						try { labResult.MedicalDirectorFName						=fields[25].Split('^')[1]; }
						catch { }
						try { labResult.MedicalDirectorLName						=fields[25].Split('^')[2]; }
						catch { }
						try { labResult.MedicalDirectorMiddleNames			=fields[25].Split('^')[3]; }
						catch { }
						try { labResult.MedicalDirectorSuffix						=fields[25].Split('^')[4]; }
						catch { }
						try { labResult.MedicalDirectorPrefix						=fields[25].Split('^')[5]; }
						catch { }
						try { labResult.MedicalDirectorAssigningAuthorityNamespaceID		=fields[25].Split('^')[8].Split('&')[0]; }
						catch { }
						try { labResult.MedicalDirectorAssigningAuthorityUniversalID		=fields[25].Split('^')[8].Split('&')[1]; }
						catch { }
						try { labResult.MedicalDirectorAssigningAuthorityIDType					=fields[25].Split('^')[8].Split('&')[2]; }
						catch { }
						try { labResult.MedicalDirectorNameTypeCode						=(HL70200)Enum.Parse(typeof(HL70200),fields[25].Split('^')[9]); }
						catch { }
						try { labResult.MedicalDirectorIdentifierTypeCode			=(HL70203)Enum.Parse(typeof(HL70203),fields[25].Split('^')[12]); }
						catch { }
						ehrLabCur.ListEhrLabResults.Add(labResult);
						break;
					//case "FTI": //Financial Transaction
					//	break;
					//case "CTI": //Clinical Trial Identification
					//	break;
					case "SPM":
						//if(retVal.ListEhrLabSpecimin==null) {
						//	retVal.ListEhrLabSpecimin=new List<EhrLabSpecimen>();
						//}
						EhrLabSpecimen ehrLabSpecimen=new EhrLabSpecimen();
						ehrLabSpecimen.SetIdSPM=PIn.Long(fields[1]);
						//Specimen Type
						ehrLabSpecimen.SpecimenTypeID											= fields[4].Split('^')[0];
						try { ehrLabSpecimen.SpecimenTypeText								= fields[4].Split('^')[1]; }
						catch { }
						try { ehrLabSpecimen.SpecimenTypeCodeSystemName			= fields[4].Split('^')[2]; }
						catch { }
						try { ehrLabSpecimen.SpecimenTypeIDAlt							= fields[4].Split('^')[3]; }
						catch { }
						try { ehrLabSpecimen.SpecimenTypeTextAlt						= fields[4].Split('^')[4]; }
						catch { }
						try { ehrLabSpecimen.SpecimenTypeCodeSystemNameAlt	= fields[4].Split('^')[5]; }
						catch { }
						try { ehrLabSpecimen.SpecimenTypeTextOriginal				= fields[4].Split('^')[6]; }
						catch { }
						//TODO:? check to see if either triplet contained a valid code.
						//Collection Date Time
						ehrLabSpecimen.CollectionDateTimeStart		=fields[17].Split('^')[0];
						try { ehrLabSpecimen.CollectionDateTimeEnd	=fields[17].Split('^')[1]; }
						catch { }
						if(fields.Length<19) {
							ehrLabCur.ListEhrLabSpecimens.Add(ehrLabSpecimen);
							break;//next segment. This one has no more fields
						}
						//if(ehrLabSpecimen.ListEhrLabSpecimenRejectReason==null) {
						//	ehrLabSpecimen.ListEhrLabSpecimenRejectReason=new List<EhrLabSpecimenRejectReason>();
						//}
						//Reject Reason
						for(int i=0;i<fields[21].Split('~').Length;i++) {
							if(fields[21].Length==0) {
								break;//nothing in this field
							}
							EhrLabSpecimenRejectReason ehrLabRR=new EhrLabSpecimenRejectReason();
							ehrLabRR.SpecimenRejectReasonID											=fields[21].Split('~')[i].Split('^')[0];
							try { ehrLabRR.SpecimenRejectReasonText								=fields[21].Split('~')[i].Split('^')[1]; }
							catch { }
							try { ehrLabRR.SpecimenRejectReasonCodeSystemName			=fields[21].Split('~')[i].Split('^')[2]; }
							catch { }
							try { ehrLabRR.SpecimenRejectReasonIDAlt							=fields[21].Split('~')[i].Split('^')[3]; }
							catch { }
							try { ehrLabRR.SpecimenRejectReasonTextAlt						=fields[21].Split('~')[i].Split('^')[4]; }
							catch { }
							try { ehrLabRR.SpecimenRejectReasonCodeSystemNameAlt	=fields[21].Split('~')[i].Split('^')[5]; }
							catch { }
							try { ehrLabRR.SpecimenRejectReasonTextOriginal				=fields[21].Split('~')[i].Split('^')[6]; }
							catch { }
							//TODO:? check to see if either triplet contained a valid code.
							ehrLabSpecimen.ListEhrLabSpecimenRejectReason.Add(ehrLabRR);
						}
						//Specimen Condition
						for(int i=0;i<fields[24].Split('~').Length;i++) {
							if(fields[24].Length==0) {
								break;//nothing in this field
							}
							EhrLabSpecimenCondition ehrLabSC=new EhrLabSpecimenCondition();
							ehrLabSC.SpecimenConditionID											=fields[24].Split('~')[i].Split('^')[0];
							try { ehrLabSC.SpecimenConditionText								=fields[24].Split('~')[i].Split('^')[1]; }
							catch { }
							try { ehrLabSC.SpecimenConditionCodeSystemName			=fields[24].Split('~')[i].Split('^')[2]; }
							catch { }
							try { ehrLabSC.SpecimenConditionIDAlt								=fields[24].Split('~')[i].Split('^')[3]; }
							catch { }
							try { ehrLabSC.SpecimenConditionTextAlt							=fields[24].Split('~')[i].Split('^')[4]; }
							catch { }
							try { ehrLabSC.SpecimenConditionCodeSystemNameAlt		=fields[24].Split('~')[i].Split('^')[5]; }
							catch { }
							try { ehrLabSC.SpecimenConditionTextOriginal				=fields[24].Split('~')[i].Split('^')[6]; }
							catch { }
							ehrLabSpecimen.ListEhrLabSpecimenCondition.Add(ehrLabSC);
						}
						ehrLabCur.ListEhrLabSpecimens.Add(ehrLabSpecimen);
						break;
					default:
						//to catch unsupported or malformed segments.
						break;
				}//end switch
			}//end foreach segment
			//TODO:Message has been processed into an EHR Lab... Now we can do other things if we want to...
			listRetVal.Add(ehrLabCur);//other labs may have already been added to retval in the code above.
			return listRetVal;
		}

		///<summary>Not implemented. We do not yet need to acknowledge incoming messages.</summary>
		public string GenerateAckMsg(string message) {
			StringBuilder strb=new StringBuilder();
			//we do not need to implement this yet. But probably will for EHR Round 3...
			return strb.ToString();
		}

		///<summary>Throws an exception if message does not contain all required segments, or contains too many segments of a given type.  Does not validate contents of segments.</summary>
		private static void containsRequiredSegmentsHelper(string message) {
			string errors="";
			string[] segments=message.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<segments.Length;i++){
				segments[i]=segments[i].Split('|')[0];///now each segment only contains the segment identifier.
			}
			//Look for each element/error sperately because there can be many variation of message structure
			//MSH
			int mshCount=0;
			for(int i=0;i<segments.Length;i++) {
				if(segments[i]=="MSH") {
					mshCount++;
				}
			}
			if(mshCount!=1) {
				errors+="Message should contain exactly 1 MSH segment, "+mshCount+" MSH segments found.\r\n";
			}
			//PID
			int pidCount=0;
			for(int i=0;i<segments.Length;i++){
				if(segments[i]=="PID"){
					pidCount++;
				}
			}
			if(pidCount!=1) {
				errors+="Message should contain exactly 1 PID segment, "+pidCount+" PID segments found.\r\n";
			}
			//ORC
			int orcCount=0;
			for(int i=0;i<segments.Length;i++){
				if(segments[i]=="ORC"){
					orcCount++;
				}
			}
			if(pidCount==0) {
				errors+="Message should contain 1 or more ORC segments, "+pidCount+" PID segments found.\r\n";
			}
			//ORC followed by OBR
			for(int i=0;i<segments.Length;i++){
				if(segments[i]=="ORC"){
					if(i+1==segments.Length || segments[i+1]!="OBR"){
						errors+="Message contains \"ORC\" segment that is not followed by \"OBR\" segment.\r\n";
						continue;
					}
					continue;
				}
			}
			//All other segments are optional according to the LRI standard.
			for(int i=0;i<segments.Length;i++) {
				switch(segments[i]) {
					case "MSH":
					case "SFT":
					case "PID":
					case "PD1":
					case "NTE":
					case "NK1":
					case "PV1":
					case "PV2":
					case "ORC":
					case "OBR":
					case "TQ1":
					case "TQ2":
					case "CTD":
					case "OBX":
					case "FTI":
					case "CTI":
					case "SPM":
						//these are the only allowed segments in this message type.
						break;
					default:
						errors+="\""+segments[i]+" is not a supported segment type.";
						break;
				}
			}
			if(errors!="") {
				throw new Exception(errors);
			}
		}
		#endregion

		///<summary>Saves EhrLab to DB and all child elements.  Note: this can be used to overwrite new data with old data when viewing old messages.  
		///Make sure you want to save all new data.</summary>
		public static EhrLab SaveToDB(EhrLab ehrLab) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EhrLab>(MethodBase.GetCurrentMethod(),ehrLab);
			}
			//check for existing EhrLab by universal identifier. 
			if(GetByGUID(ehrLab.PlacerOrderUniversalID,ehrLab.PlacerOrderNum)!=null) {
				ehrLab.EhrLabNum=GetByGUID(ehrLab.PlacerOrderUniversalID,ehrLab.PlacerOrderNum).EhrLabNum;//identified by placer order num... should be the case
			}
			else if(GetByGUID(ehrLab.FillerOrderUniversalID,ehrLab.FillerOrderNum)!=null) {
				ehrLab.EhrLabNum=GetByGUID(ehrLab.FillerOrderUniversalID,ehrLab.FillerOrderNum).EhrLabNum;//identified by the filler order num... rarely
			}
			//Insert or update everything
			if(ehrLab.EhrLabNum==0) {//new; Insert new EhrLab, Insert all new children
				ehrLab.EhrLabNum=Insert(ehrLab);
			}
			else {//existing; update EhrLab, Delete All children so new ones can be reinserted.
				Update(ehrLab);
				EhrLabNotes.DeleteForLab(ehrLab.EhrLabNum);
				EhrLabResults.DeleteForLab(ehrLab.EhrLabNum);
				EhrLabResultsCopyTos.DeleteForLab(ehrLab.EhrLabNum);
				EhrLabClinicalInfos.DeleteForLab(ehrLab.EhrLabNum);
				EhrLabSpecimens.DeleteForLab(ehrLab.EhrLabNum);
			}
			//Insert new child elements
			for(int i=0;i<ehrLab.ListEhrLabNotes.Count;i++) {//must be before lab results.
				ehrLab.ListEhrLabNotes[i].EhrLabNum=ehrLab.EhrLabNum;
				ehrLab.ListEhrLabNotes[i].EhrLabNoteNum=EhrLabNotes.Insert(ehrLab.ListEhrLabNotes[i]);
			}
			for(int i=0;i<ehrLab.ListEhrLabResults.Count;i++) {
				ehrLab.ListEhrLabResults[i].EhrLabNum=ehrLab.EhrLabNum;
				ehrLab.ListEhrLabResults[i]=EhrLabResults.InsertItem(ehrLab.ListEhrLabResults[i]);//insert/return entire object so that child objects are also updated.
			}
			for(int i=0;i<ehrLab.ListEhrLabResultsCopyTo.Count;i++) {
				ehrLab.ListEhrLabResultsCopyTo[i].EhrLabNum=ehrLab.EhrLabNum;
				ehrLab.ListEhrLabResultsCopyTo[i].EhrLabResultsCopyToNum=EhrLabResultsCopyTos.Insert(ehrLab.ListEhrLabResultsCopyTo[i]);
			}
			for(int i=0;i<ehrLab.ListEhrLabSpecimens.Count;i++) {
				ehrLab.ListEhrLabSpecimens[i].EhrLabNum=ehrLab.EhrLabNum;
				ehrLab.ListEhrLabSpecimens[i]=EhrLabSpecimens.InsertItem(ehrLab.ListEhrLabSpecimens[i]);//Insert item to return the whole object.
			}
			for(int i=0;i<ehrLab.ListRelevantClinicalInformations.Count;i++) {
				ehrLab.ListRelevantClinicalInformations[i].EhrLabNum=ehrLab.EhrLabNum;
				ehrLab.ListRelevantClinicalInformations[i].EhrLabClinicalInfoNum=EhrLabClinicalInfos.Insert(ehrLab.ListRelevantClinicalInformations[i]);
			}
			return ehrLab;
		}

		///<summary>Gets one EhrLab from the db.</summary>
		public static EhrLab GetOne(long ehrLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EhrLab>(MethodBase.GetCurrentMethod(),ehrLabNum);
			}
			return Crud.EhrLabCrud.SelectOne(ehrLabNum);
		}

		public static long GetNextOrderNum() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command="SELECT MAX(EhrLabNum) FROM ehrlab";
			long retval=0;
			try {
				PIn.Long(Db.GetScalar(command));//can be null if table is empty.
			}
			catch { }
			return retval+1;
		}

		/////<summary>Gets one EhrLab from the db.</summary>
		//public static long GetNumFromMessage(string message) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetObject<EhrLab>(MethodBase.GetCurrentMethod(),message);
		//	}
		//	long retVal=0;
		//	string[] segments=message.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
		//	foreach(string segment in segments) {
		//		if(!segment.StartsWith("ORC")) {
		//			continue;
		//		}
		//		string[] fields=segment.Split('|');

		//	}


		//	return retVal;
		//}

		///<summary>Gets one EhrLab from the db.</summary>
		public static EhrLab GetByGUID(string root, string extension) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EhrLab>(MethodBase.GetCurrentMethod(),root, extension);
			}
			string command="SELECT * FROM ehrlab WHERE (PlacerOrderNum='"+extension+"' AND PlacerOrderUniversalID='"+root+"'AND PlacerOrderNum!='' AND PlacerOrderUniversalID!='') "
			+"OR (FillerOrderNum='"+extension+"' AND FillerOrderUniversalID='"+root+"' AND FillerOrderNum!='' AND FillerOrderUniversalID!='')";
			return Crud.EhrLabCrud.SelectOne(command);
		}

		///<summary></summary>
		public static List<EhrLab> GetAllForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrLab>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM ehrlab WHERE PatNum = "+POut.Long(patNum);
			List<EhrLab> retVal=Crud.EhrLabCrud.SelectMany(command);

			return retVal;
		}

		///<summary></summary>
		public static List<EhrLab> GetAllForPatInDateRange(long patNum, DateTime dateStart, DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrLab>>(MethodBase.GetCurrentMethod(),patNum);
			}
			//TODO: Finish this with Travis' Stuff
			string command="SELECT * FROM ehrlab WHERE PatNum = "+POut.Long(patNum);
			List<EhrLab> retVal=Crud.EhrLabCrud.SelectMany(command);

			return retVal;
		}

		///<summary>Helper function to return a list of descriptions for the HL70203 enumeration.  First item in the list is blank.</summary>
		public static List<string> GetHL70203Descriptions() {
			List<string> retVal=new List<string>();
			retVal.Add("");//Blank
			retVal.Add("Account number");//AN
			retVal.Add("Account number creditor");//ANC
			retVal.Add("Account number debitor");//AND
			retVal.Add("Advanced practice registered nurse");//APRN
			retVal.Add("American express");//AM
			retVal.Add("Anonymous identifier");//ANON
			retVal.Add("Bank account number");//BA
			retVal.Add("Bank card number");//BC
			retVal.Add("Birth registry number");//BR
			retVal.Add("Breed registry number");//BRN
			retVal.Add("Cost center number");//CC
			retVal.Add("County number");//CY
			retVal.Add("Dentist license number");//DDS
			retVal.Add("Diner's club card");//DI
			retVal.Add("Discover card");//DS
			retVal.Add("Doctor number");//DN
			retVal.Add("Donor registration number");//DR
			retVal.Add("Driver's license number");//DL
			retVal.Add("Drug enforcement administration");//DEA
			retVal.Add("Drug furnishing number");//DFN
			retVal.Add("Employee number");//EI
			retVal.Add("Employer number");//EN
			retVal.Add("Facility ID");//FI
			retVal.Add("General ledger number");//GL
			retVal.Add("Guarantor external identifier");//GN
			retVal.Add("Guarantor internal identifier");//GI
			retVal.Add("Health card number");//HC
			retVal.Add("Indigenous/Aboriginal");//IND
			retVal.Add("Jurisdictional health number");//JHN
			retVal.Add("Labor and industries number");//LI
			retVal.Add("License number");//LN
			retVal.Add("Living subject enterprise number");//PE
			retVal.Add("Local registry ID");//LR
			retVal.Add("MasterCard");//MS
			retVal.Add("Medical license number");//MD
			retVal.Add("Medical record number");//MR
			retVal.Add("Universal physician identification numbers");//UPIN
			retVal.Add("Member number");//MB
			retVal.Add("Microchip number");//MCN
			retVal.Add("Military ID number");//MI
			retVal.Add("National employer identifier");//NE
			retVal.Add("National health plan identifier");//NH
			retVal.Add("National insurance organization identifier");//NII
			retVal.Add("National insurance payor identifier");//NIIP
			retVal.Add("National person identifier");//NNxxx
			retVal.Add("National provider identifier");//NPI
			retVal.Add("National unique individual identifier");//NI
			retVal.Add("Nurse practitioner number");//NP
			retVal.Add("Optometrist license number");//OD
			retVal.Add("Organization identifier");//XX
			retVal.Add("Osteopathic license number");//DO
			retVal.Add("Passport number");//PPN
			retVal.Add("Patient external identifier");//PT
			retVal.Add("Patient internal identifier");//PI
			retVal.Add("Patient Medicaid number");//MA
			retVal.Add("Patient's Medicare number");//MC
			retVal.Add("Penitentiary institution number");//PCN
			retVal.Add("Pension number");//PEN
			retVal.Add("Permanent resident card number");//PRC
			retVal.Add("Person number");//PN
			retVal.Add("Pharmacist license number");//RPH
			retVal.Add("Physician assistant number");//PA
			retVal.Add("Podiatrist license number");//DPM
			retVal.Add("Practitioner Medicaid number");//MCD
			retVal.Add("Practitioner Medicare number");//MCR
			retVal.Add("Provider number");//PRN
			retVal.Add("QA number");//QA
			retVal.Add("Railroad retirement number");//RR
			retVal.Add("Regional registry ID");//RRI
			retVal.Add("Registered nurse number");//RN
			retVal.Add("Resource identifier");//RI
			retVal.Add("Social security number");//SS
			retVal.Add("Specimen identifier");//SID
			retVal.Add("State license");//SL
			retVal.Add("State registry ID");//SR
			retVal.Add("Subscriber number");//SN
			retVal.Add("Tax ID number");//TAX
			retVal.Add("Temporary account number");//ANT
			retVal.Add("Temporary living subject number");//PNT
			retVal.Add("Temporary medical record number");//MRT
			retVal.Add("Treaty Number");//TN
			retVal.Add("Unspecified identifier");//U
			retVal.Add("VISA");//VS
			retVal.Add("Visit number");//VN
			retVal.Add("WIC identifier");//WC
			retVal.Add("Workers' comp number");//WCN
			return retVal;
		}

		///<summary>Helper function to return a list of descriptions for the HL70065 enumeration.  First item in the list is blank.</summary>
		public static List<string> GetHL70065Descriptions() {
			//No need to check RemotingRole;
			List<string> retVal=new List<string>();
			retVal.Add("");//Blank		
			retVal.Add("Add ordered tests to the existing specimen");//A,
			retVal.Add("Generated order; reflex order");//G,
			retVal.Add("Lab to obtain specimen from patient");//L,
			retVal.Add("Specimen obtained by service other than lab");//O
			return retVal;
		}

		///<summary>Helper function to return a list of descriptions for the HL70123 enumeration.  First item in the list is blank.</summary>
		public static List<string> GetHL70123Descriptions() {
			//No need to check RemotingRole;
			List<string> retVal=new List<string>();
			retVal.Add("");//Blank		
			retVal.Add("Some but not all results available.");//A,
			retVal.Add("Correction to results.");//C,
			retVal.Add("Final Results; results stored and verified. Can only be changed with a corrected result.");//F,
			retVal.Add("No results available; specimen received, procedure incomplete.");//I,
			retVal.Add("Order received; specimen not yet received.");//O,
			retVal.Add("Preliminary: A verified early result is available, final results not yet obtained.");//P,
			retVal.Add("Results stored; not yet verified.");//R,
			retVal.Add("No results available; procedure scheduled but not done.");//S,
			retVal.Add("No results available; Order canceled.");//X
			return retVal;
		}

		///<summary></summary>
		public static long Insert(EhrLab ehrLab) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				ehrLab.EhrLabNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ehrLab);
				return ehrLab.EhrLabNum;
			}
			return Crud.EhrLabCrud.Insert(ehrLab);
		}

		///<summary></summary>
		public static void Update(EhrLab ehrLab) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrLab);
				return;
			}
			Crud.EhrLabCrud.Update(ehrLab);
		}

		///<summary>Deletes EhrLab, EhrLabResults, EhrLabResultsCopyTos, EhrLabClinicalInfos, and EhrLabSpecimens.</summary>
		public static void Delete(long ehrLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrLabNum);
				return;
			}
			string command= "DELETE FROM ehrlab WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			Db.NonQ(command);
			EhrLabNotes.DeleteForLab(ehrLabNum);
			EhrLabResults.DeleteForLab(ehrLabNum);
			EhrLabResultsCopyTos.DeleteForLab(ehrLabNum);
			EhrLabClinicalInfos.DeleteForLab(ehrLabNum);
			EhrLabSpecimens.DeleteForLab(ehrLabNum);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all EhrLabs.</summary>
		private static List<EhrLab> listt;

		///<summary>A list of all EhrLabs.</summary>
		public static List<EhrLab> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM ehrlab ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EhrLab";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EhrLabCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static long Insert(EhrLab ehrLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				ehrLab.EhrLabNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ehrLab);
				return ehrLab.EhrLabNum;
			}
			return Crud.EhrLabCrud.Insert(ehrLab);
		}

		///<summary></summary>
		public static void Update(EhrLab ehrLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrLab);
				return;
			}
			Crud.EhrLabCrud.Update(ehrLab);
		}
		*/



	}
}