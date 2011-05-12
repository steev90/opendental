﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace OpenDentBusiness.HL7 {
	///<summary>An ORU message is an Unsolicited Observation Message.  It is a message sent out from Open Dental containing immunization status in order to satisfy ehr public health requirements.</summary>
	public class ORU {
		private MessageHL7 msg;
		private SegmentHL7 seg;

		///<summary></summary>
		public ORU() {
			
		}
		
		///<summary>Creates the Message object and fills it with data.  All panels will be for the same patient.</summary>
		public void Initialize(List<LabPanel> panels) {
			if(panels.Count==0) {
				throw new ApplicationException("Must be at least one lab panel.");
			}
			//LabPanel panel=panelList[0];
			//List<LabResult> labresultList=LabResults.GetForPanel(panel.LabPanelNum);
			//if(labresultList.Count!=1) {
			//	throw new ApplicationException("Lab panel must have exactly one lab result.");
			//}
			//LabResult labresult=labresultList[0];
			Patient pat=Patients.GetPat(panels[0].PatNum);
			msg=new MessageHL7(MessageType.ORU);
			MSH();
			PID(pat);
			for(int p=0;p<panels.Count;p++) {
				List<LabResult> results=LabResults.GetForPanel(panels[p].LabPanelNum);
				OBR(panels[p],results[0].DateTimeTest);
				for(int r=0;r<results.Count;r++) {
					OBX(results[r],r);
				}
			}
		}

		///<summary>Message Header Segment</summary>
		private void MSH(){
			seg=new SegmentHL7(@"MSH|^~\&|Open Dental|"//MSH-3: Sending application
				+"OD|"//MSH-4: Sending Facility. Dummy.
				+"Y|"//MSH-5: Receiving Application.  Dummy.
				+"OU|"//MSH-6: Receiving Facility. Dummy.
				+DateTime.Now.ToString("yyyyMMddHHmmss")+"||"
				+"ORU^R01^ORU_R01|"//MSH-9: fixed
				+DateTime.Now.ToString("yyyyMMddHHmmss")+"|"//MSH-10: Control Id, str20. Fixed is ok for testing. We'll send date
				+"P|"//MSH-11: P=production
				+"2.3.1");
			msg.Segments.Add(seg);
		}

		///<summary>Patient identification.</summary>
		private void PID(Patient pat){
			seg=new SegmentHL7(SegmentName.PID);
			seg.SetField(0,"PID");
			seg.SetField(2,pat.PatNum.ToString());
			seg.SetField(3,pat.PatNum.ToString());
			seg.SetField(5,pat.LName,pat.FName);
			if(pat.Birthdate.Year>1880) {//7: dob optional
				seg.SetField(7,pat.Birthdate.ToString("yyyyMMdd"));
			}
			seg.SetField(8,ConvertGender(pat.Gender));
			seg.SetField(10,ConvertRace(pat.Race));
			seg.SetField(11,pat.Address,pat.Address2,pat.City,pat.State,pat.Zip,"","M");//M is for mailing.
			seg.SetField(13,ConvertPhone(pat.HmPhone));
			seg.SetField(22,ConvertEthnicGroup(pat.Race));
			msg.Segments.Add(seg);
		}

		/*This is the example that we are using for incorporating lab results in our other test.
		  It is simpler than the above examples.  We might want to use it instead.
		MSH|^~\&|KAM|DGI|Y|OU|20100920093000||ORU^R01^ORU_R01|20100920093000|P|2.3.1
PID||405979410 |405979410^^^&2.16.840.1.113883.19.3.2.1&ISO^MR||Lewis^Frank ||19500101|M||2106-3^White^HL70005|622 Chestnut^^Springfield^Tennessee^37026^^M||^^^^^615^3826396|||||405979410 ||||N^Not Hispanic or Latino^HL70189
OBR|1|OrderNum-1001|FillOrder-1001|24331-1^Lipid Panel^LN||20100920083000|20100920083000|20100920083000|||||Hemolyzed ||| ProviderIDNum-100^Crow^Tom^Black^III^Dr.||||Aloha Laboratories 575 Luau Street Honolulu Hawaii 96813 ||||CH|F|
OBX|1|NM|14647-2^Total cholesterol^LN |134465|162|mg/dl |<200| N|||F|||20100920083000 
OBX|2|NM|14646-4^HDL cholesterol^LN|333123|43|mg/dl|>=40| N|||F|||20100920083000
OBX|3|NM|2089-1^LDL cholesterol^LN|333123|84|mg/dl|<100| N|||F|||20100920083000
OBX|4|NM|14927-8^Triglycerides^LN|333123|127|mg/dl|<150| N|||F|||20100920083000*/

		/*This is example #5.  Hepatitis C is a legitimate reportable syndrome which would be reported to public health
		MSH|^~\&|EHR Application^2.16.840.1.113883.3.72.7.1^HL7|EHR Facility^2.16.840.1.113883.3.72.7.2^HL7|PH Application^2.16.840.1.113883.3.72.7.3^HL7|PH Facility^2.16.840.1.113883.3.72.7.4^HL7|20110316102334||ORU^R01^ORU_R01|NIST-110316102333943|P|2.5.1|||||||||PHLabReport-Ack^^2.16.840.1.114222.4.10.3^ISO
		SFT|NIST Lab, Inc.|3.6.23|A-1 Lab System|6742873-12||20080303
		PID|||686774009^^^MPI&2.16.840.1.113883.19.3.2.1&ISO^MR||Takamura^Michael||19820815|M||2028-9^Asian^HL70005|3567 Maple Street^^Oakland^CA^94605^USA^M||^PRN^^^^510^6658876|||||||||N^Not Hispanic or Latino^HL70189
		OBR|1||7564832^Lab^2.16.840.1.113883.19.3.1.6^ISO|10676-5^Hepatitis C Virus RNA^LN^1198112^Hepatitis C Test^99USI|||201007281400||||||Nausea, vomiting, abdominal pain|||1234^Admit^Alan^^^^^^ABC Medical Center&2.16.840.1.113883.19.4.6&ISO||||||201007301500|||F||||||787.01^Nausea and vomiting^I9CDX~789.0^Abdominal pain^I9CDX
		OBX|1|NM|10676-5^Hepatitis C Virus RNA^LN|1|850000|iU/mL^international units per mililiter^UCUM|High Viral Load > or = 850000iU/mL|H|||F|||201007281400|||||200807301500||||Lab^L^^^^CLIA&2.16.840.1.113883.19.4.6&ISO^XX^^^1236|3434 Industrial Lane^^Ann Arbor^MI^48103^^B
		SPM||||122555007^Venous blood specimen^SCT^BLDV^Blood venous^HL70487^20080131^2.5.1
		*/
		

		private void OBR(LabPanel panel, DateTime datetime) {
			seg=new SegmentHL7(SegmentName.OBR);
			seg.SetField(0,"OBR");
			seg.SetField(1,"1");
			seg.SetField(2,"OrderNum-1001");
			seg.SetField(3,"FillOrder-1001");
			seg.SetField(4,panel.ServiceId,panel.ServiceName,"LN");
			seg.SetField(6,datetime.ToString("yyyyMMddhhmm"));
			seg.SetField(7,datetime.ToString("yyyyMMddhhmm"));
			seg.SetField(8,datetime.ToString("yyyyMMddhhmm"));
			seg.SetField(13,panel.SpecimenCondition);
			seg.SetField(15,panel.SpecimenSource);
			seg.SetField(20,panel.LabNameAddress);
			seg.SetField(24,"CH");
			seg.SetField(25,"F");
			msg.Segments.Add(seg);
		}

		/// <summary>idx passed in will be zero-based. Will be converted to 1-based.</summary>
		private void OBX(LabResult labresult,int idx) {
			seg=new SegmentHL7(SegmentName.OBX);
			seg.SetField(0,"OBX");
			seg.SetField(1,(idx+1).ToString());
			seg.SetField(2,"NM");//ValueType. NM=numeric, referring to the value that will follow in OBX-5
			seg.SetField(3,labresult.TestID,labresult.TestName,"LN");//TestPerformed  ID^text^codingSystem.  eg. 10676-5^Hepatitis C Virus RNA^LN
			seg.SetField(4,"1");//?
			seg.SetField(5,labresult.ObsValue);//Value. Type based on OBX-2.  eg. 43.
			seg.SetField(6,labresult.ObsUnits);//Units. Example ml/dl.
			seg.SetField(7,labresult.ObsRange);//Range. Example <100
			seg.SetField(8,"N");//?
			seg.SetField(11,"F");//OBX-11 is required.  F means final.
			seg.SetField(14,labresult.DateTimeTest.ToString("yyyyMMddhhmm"));//OBX-14 datetime
			msg.Segments.Add(seg);
		}

		public string GenerateMessage() {
			return msg.ToString();
		}

		private string ConvertGender(PatientGender gender) {
			if(gender==PatientGender.Female) {
				return "F";
			}
			if(gender==PatientGender.Male) {
				return "M";
			}
			return "U";
		}

		private string ConvertRace(PatientRace race) {
			switch(race) {
				case PatientRace.AmericanIndian:
					return "1002-5^American Indian Or Alaska Native^HL70005";
				case PatientRace.Asian:
					return "2028-9^Asian^HL70005";
				case PatientRace.AfricanAmerican:
					return "2054-5^Black or African American^HL70005";
				case PatientRace.HawaiiOrPacIsland:
					return "2076-8^Native Hawaiian or Other Pacific Islander^HL70005";
				case PatientRace.White:
					return "2106-3^White^HL70005";
				case PatientRace.Other:
					return "2131-1^Other Race^HL70005";
				default://including hispanic
					return "2131-1^Other Race^HL70005";
			}
		}

		private string ConvertEthnicGroup(PatientRace race) {
			switch(race) {
				case PatientRace.HispanicLatino:
					return "H^Hispanic or Latino^HL70189";
				default:
					return "N^Not Hispanic or Latino^HL70189";
			}
		}

		private string ConvertPhone(string phone) {
			string digits="";
			for(int i=0;i<phone.Length;i++) {
				if(!Char.IsNumber(phone,i)) {
					continue;
				}
				if(digits=="" && phone.Substring(i,1)=="1") {
					continue;//skip leading 1.
				}
				digits+=phone.Substring(i,1);
			}
			if(digits.Length!=10) {
				return "";
			}
			string retVal="";
			retVal+="^";//1:deprecated
			retVal+="PRN^";//2:table201. PRN=primary residence number.  
			retVal+="^^^";//3-5:
			retVal+=digits.Substring(0,3)+"^";//6:area code
			retVal+=digits.Substring(3);
			return retVal;
		}

		



	}
}
