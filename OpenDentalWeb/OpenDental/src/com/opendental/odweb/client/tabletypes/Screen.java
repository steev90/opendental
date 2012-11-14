package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;

public class Screen {
		/** Primary key */
		public int ScreenNum;
		/** The date of the screening. */
		public String ScreenDate;
		/** FK to site.Description, although it will not crash if key absent. */
		public String GradeSchool;
		/** FK to county.CountyName, although it will not crash if key absent. */
		public String County;
		/** Enum:PlaceOfService */
		public PlaceOfService PlaceService;
		/** FK to provider.ProvNum.  ProvNAME is always entered, but ProvNum supplements it by letting user select from list.  When entering a provNum, the name will be filled in automatically. Can be 0 if the provider is not in the list, but provName is required. */
		public int ProvNum;
		/** Required. */
		public String ProvName;
		/** Enum:PatientGender */
		public PatientGender Gender;
		/** Enum:PatientRace and ethnicity. */
		public PatientRace Race;
		/** Enum:PatientGrade */
		public PatientGrade GradeLevel;
		/** Age of patient at the time the screening was done. Faster than recording birthdates. */
		public byte Age;
		/** Enum:TreatmentUrgency */
		public TreatmentUrgency Urgency;
		/** Enum:YN Set to true if patient has cavities. */
		public YN HasCaries;
		/** Enum:YN Set to true if patient needs sealants. */
		public YN NeedsSealants;
		/** Enum:YN */
		public YN CariesExperience;
		/** Enum:YN */
		public YN EarlyChildCaries;
		/** Enum:YN */
		public YN ExistingSealants;
		/** Enum:YN */
		public YN MissingAllTeeth;
		/** Optional */
		public String Birthdate;
		/** FK to screengroup.ScreenGroupNum. */
		public int ScreenGroupNum;
		/** The order of this item within its group. */
		public int ScreenGroupOrder;
		/** . */
		public String Comments;

		/** Deep copy of object. */
		public Screen Copy() {
			Screen screen=new Screen();
			screen.ScreenNum=this.ScreenNum;
			screen.ScreenDate=this.ScreenDate;
			screen.GradeSchool=this.GradeSchool;
			screen.County=this.County;
			screen.PlaceService=this.PlaceService;
			screen.ProvNum=this.ProvNum;
			screen.ProvName=this.ProvName;
			screen.Gender=this.Gender;
			screen.Race=this.Race;
			screen.GradeLevel=this.GradeLevel;
			screen.Age=this.Age;
			screen.Urgency=this.Urgency;
			screen.HasCaries=this.HasCaries;
			screen.NeedsSealants=this.NeedsSealants;
			screen.CariesExperience=this.CariesExperience;
			screen.EarlyChildCaries=this.EarlyChildCaries;
			screen.ExistingSealants=this.ExistingSealants;
			screen.MissingAllTeeth=this.MissingAllTeeth;
			screen.Birthdate=this.Birthdate;
			screen.ScreenGroupNum=this.ScreenGroupNum;
			screen.ScreenGroupOrder=this.ScreenGroupOrder;
			screen.Comments=this.Comments;
			return screen;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Screen>");
			sb.append("<ScreenNum>").append(ScreenNum).append("</ScreenNum>");
			sb.append("<ScreenDate>").append(Serializing.EscapeForXml(ScreenDate)).append("</ScreenDate>");
			sb.append("<GradeSchool>").append(Serializing.EscapeForXml(GradeSchool)).append("</GradeSchool>");
			sb.append("<County>").append(Serializing.EscapeForXml(County)).append("</County>");
			sb.append("<PlaceService>").append(PlaceService.ordinal()).append("</PlaceService>");
			sb.append("<ProvNum>").append(ProvNum).append("</ProvNum>");
			sb.append("<ProvName>").append(Serializing.EscapeForXml(ProvName)).append("</ProvName>");
			sb.append("<Gender>").append(Gender.ordinal()).append("</Gender>");
			sb.append("<Race>").append(Race.ordinal()).append("</Race>");
			sb.append("<GradeLevel>").append(GradeLevel.ordinal()).append("</GradeLevel>");
			sb.append("<Age>").append(Age).append("</Age>");
			sb.append("<Urgency>").append(Urgency.ordinal()).append("</Urgency>");
			sb.append("<HasCaries>").append(HasCaries.ordinal()).append("</HasCaries>");
			sb.append("<NeedsSealants>").append(NeedsSealants.ordinal()).append("</NeedsSealants>");
			sb.append("<CariesExperience>").append(CariesExperience.ordinal()).append("</CariesExperience>");
			sb.append("<EarlyChildCaries>").append(EarlyChildCaries.ordinal()).append("</EarlyChildCaries>");
			sb.append("<ExistingSealants>").append(ExistingSealants.ordinal()).append("</ExistingSealants>");
			sb.append("<MissingAllTeeth>").append(MissingAllTeeth.ordinal()).append("</MissingAllTeeth>");
			sb.append("<Birthdate>").append(Serializing.EscapeForXml(Birthdate)).append("</Birthdate>");
			sb.append("<ScreenGroupNum>").append(ScreenGroupNum).append("</ScreenGroupNum>");
			sb.append("<ScreenGroupOrder>").append(ScreenGroupOrder).append("</ScreenGroupOrder>");
			sb.append("<Comments>").append(Serializing.EscapeForXml(Comments)).append("</Comments>");
			sb.append("</Screen>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				ScreenNum=Integer.valueOf(doc.getElementsByTagName("ScreenNum").item(0).getFirstChild().getNodeValue());
				ScreenDate=doc.getElementsByTagName("ScreenDate").item(0).getFirstChild().getNodeValue();
				GradeSchool=doc.getElementsByTagName("GradeSchool").item(0).getFirstChild().getNodeValue();
				County=doc.getElementsByTagName("County").item(0).getFirstChild().getNodeValue();
				PlaceService=PlaceOfService.values()[Integer.valueOf(doc.getElementsByTagName("PlaceService").item(0).getFirstChild().getNodeValue())];
				ProvNum=Integer.valueOf(doc.getElementsByTagName("ProvNum").item(0).getFirstChild().getNodeValue());
				ProvName=doc.getElementsByTagName("ProvName").item(0).getFirstChild().getNodeValue();
				Gender=PatientGender.values()[Integer.valueOf(doc.getElementsByTagName("Gender").item(0).getFirstChild().getNodeValue())];
				Race=PatientRace.values()[Integer.valueOf(doc.getElementsByTagName("Race").item(0).getFirstChild().getNodeValue())];
				GradeLevel=PatientGrade.values()[Integer.valueOf(doc.getElementsByTagName("GradeLevel").item(0).getFirstChild().getNodeValue())];
				Age=Byte.valueOf(doc.getElementsByTagName("Age").item(0).getFirstChild().getNodeValue());
				Urgency=TreatmentUrgency.values()[Integer.valueOf(doc.getElementsByTagName("Urgency").item(0).getFirstChild().getNodeValue())];
				HasCaries=YN.values()[Integer.valueOf(doc.getElementsByTagName("HasCaries").item(0).getFirstChild().getNodeValue())];
				NeedsSealants=YN.values()[Integer.valueOf(doc.getElementsByTagName("NeedsSealants").item(0).getFirstChild().getNodeValue())];
				CariesExperience=YN.values()[Integer.valueOf(doc.getElementsByTagName("CariesExperience").item(0).getFirstChild().getNodeValue())];
				EarlyChildCaries=YN.values()[Integer.valueOf(doc.getElementsByTagName("EarlyChildCaries").item(0).getFirstChild().getNodeValue())];
				ExistingSealants=YN.values()[Integer.valueOf(doc.getElementsByTagName("ExistingSealants").item(0).getFirstChild().getNodeValue())];
				MissingAllTeeth=YN.values()[Integer.valueOf(doc.getElementsByTagName("MissingAllTeeth").item(0).getFirstChild().getNodeValue())];
				Birthdate=doc.getElementsByTagName("Birthdate").item(0).getFirstChild().getNodeValue();
				ScreenGroupNum=Integer.valueOf(doc.getElementsByTagName("ScreenGroupNum").item(0).getFirstChild().getNodeValue());
				ScreenGroupOrder=Integer.valueOf(doc.getElementsByTagName("ScreenGroupOrder").item(0).getFirstChild().getNodeValue());
				Comments=doc.getElementsByTagName("Comments").item(0).getFirstChild().getNodeValue();
			}
			catch(Exception e) {
				throw e;
			}
		}

		/**  */
		public enum PlaceOfService {
			/** 0. CPT code 11 */
			Office,
			/** 1. CPT code 12 */
			PatientsHome,
			/** 2. CPT code 21 */
			InpatHospital,
			/** 3. CPT code 22 */
			OutpatHospital,
			/** 4. CPT code 31 */
			SkilledNursFac,
			/** 5. CPT code 33.  In X12, a similar code AdultLivCareFac 35 is mentioned. */
			CustodialCareFacility,
			/** 6. CPT code ?.  We use 11 for office. */
			OtherLocation,
			/** 7. CPT code 15 */
			MobileUnit,
			/** 8. CPT code 03 */
			School,
			/** 9. CPT code 26 */
			MilitaryTreatFac,
			/** 10. CPT code 50 */
			FederalHealthCenter,
			/** 11. CPT code 71 */
			PublicHealthClinic,
			/** 12. CPT code 72 */
			RuralHealthClinic,
			/** 13. CPT code 23 */
			EmergencyRoomHospital,
			/** 14. CPT code 24 */
			AmbulatorySurgicalCenter
		}

		/**  */
		public enum PatientGender {
			/** 0 */
			Male,
			/** 1 */
			Female,
			/** 2- This is not a joke. Required by HIPAA for privacy.  Required by ehr to track missing entries. */
			Unknown
		}

		/** Race and ethnicity for patient. Used by public health.  The problem is that everyone seems to want different choices.  If we give these choices their own table, then we also need to include mapping functions.  These are currently used in ArizonaReports, HL7 w ECW, and EHR.  Foreign users would like their own mappings. */
		public enum PatientRace {
			/** 0 */
			Unknown,
			/** 1 */
			Multiracial,
			/** 2 */
			HispanicLatino,
			/** 3 */
			AfricanAmerican,
			/** 4 */
			White,
			/** 5 */
			HawaiiOrPacIsland,
			/** 6 */
			AmericanIndian,
			/** 7 */
			Asian,
			/** 8 */
			Other,
			/** 9 */
			Aboriginal,
			/** 10 - Required by EHR, even though it's stupid. */
			BlackHispanic
		}

		/** Grade level used in public health. */
		public enum PatientGrade {
			/** 0 */
			Unknown,
			/** 1 */
			First,
			/** 2 */
			Second,
			/** 3 */
			Third,
			/** 4 */
			Fourth,
			/** 5 */
			Fifth,
			/** 6 */
			Sixth,
			/** 7 */
			Seventh,
			/** 8 */
			Eighth,
			/** 9 */
			Ninth,
			/** 10 */
			Tenth,
			/** 11 */
			Eleventh,
			/** 12 */
			Twelfth,
			/** 13 */
			PrenatalWIC,
			/** 14 */
			PreK,
			/** 15 */
			Kindergarten,
			/** 16 */
			Other
		}

		/** For public health.  Unknown, NoProblems, NeedsCarE, or Urgent. */
		public enum TreatmentUrgency {
			/**  */
			Unknown,
			/**  */
			NoProblems,
			/**  */
			NeedsCare,
			/**  */
			Urgent
		}

		/** Unknown,Yes, or No. */
		public enum YN {
			/** 0 */
			Unknown,
			/** 1 */
			Yes,
			/** 2 */
			No
		}


}