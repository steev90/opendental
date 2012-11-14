package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;

public class HL7DefField {
		/** Primary key. */
		public int HL7DefFieldNum;
		/** FK to HL7DefSegment.HL7DefSegmentNum */
		public int HL7DefSegmentNum;
		/** Position within the segment. */
		public int OrdinalPos;
		/** HL7 table Id, if applicable. Example: 0234. Example: 1234/2345.  DataType will be ID. */
		public String TableId;
		/** The DataTypeHL7 enum will be unlinked from the db by storing as string in db. As it's loaded into OD, it will become an enum. */
		public DataTypeHL7 DataType;
		/** User will get to pick from a list of fields that we will maintain. Example: guar.nameLFM, prov.provIdName, or pat.addressCityStateZip.  See below for the full list.  This will be blank if this is a fixed text field. */
		public String FieldName;
		/** User will need to insert fixed text for some fields.  Either FixedText or FieldName will have a value, not both. */
		public String FixedText;

		/** Deep copy of object. */
		public HL7DefField Copy() {
			HL7DefField hl7deffield=new HL7DefField();
			hl7deffield.HL7DefFieldNum=this.HL7DefFieldNum;
			hl7deffield.HL7DefSegmentNum=this.HL7DefSegmentNum;
			hl7deffield.OrdinalPos=this.OrdinalPos;
			hl7deffield.TableId=this.TableId;
			hl7deffield.DataType=this.DataType;
			hl7deffield.FieldName=this.FieldName;
			hl7deffield.FixedText=this.FixedText;
			return hl7deffield;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<HL7DefField>");
			sb.append("<HL7DefFieldNum>").append(HL7DefFieldNum).append("</HL7DefFieldNum>");
			sb.append("<HL7DefSegmentNum>").append(HL7DefSegmentNum).append("</HL7DefSegmentNum>");
			sb.append("<OrdinalPos>").append(OrdinalPos).append("</OrdinalPos>");
			sb.append("<TableId>").append(Serializing.EscapeForXml(TableId)).append("</TableId>");
			sb.append("<DataType>").append(DataType.ordinal()).append("</DataType>");
			sb.append("<FieldName>").append(Serializing.EscapeForXml(FieldName)).append("</FieldName>");
			sb.append("<FixedText>").append(Serializing.EscapeForXml(FixedText)).append("</FixedText>");
			sb.append("</HL7DefField>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				HL7DefFieldNum=Integer.valueOf(doc.getElementsByTagName("HL7DefFieldNum").item(0).getFirstChild().getNodeValue());
				HL7DefSegmentNum=Integer.valueOf(doc.getElementsByTagName("HL7DefSegmentNum").item(0).getFirstChild().getNodeValue());
				OrdinalPos=Integer.valueOf(doc.getElementsByTagName("OrdinalPos").item(0).getFirstChild().getNodeValue());
				TableId=doc.getElementsByTagName("TableId").item(0).getFirstChild().getNodeValue();
				DataType=DataTypeHL7.values()[Integer.valueOf(doc.getElementsByTagName("DataType").item(0).getFirstChild().getNodeValue())];
				FieldName=doc.getElementsByTagName("FieldName").item(0).getFirstChild().getNodeValue();
				FixedText=doc.getElementsByTagName("FixedText").item(0).getFirstChild().getNodeValue();
			}
			catch(Exception e) {
				throw e;
			}
		}

		/** Data types are listed in HL7 docs section 2.15.  The items in this enumeration can be freely rearranged without damaging the database.  But can't change spelling or remove existing item. */
		public enum DataTypeHL7 {
			/** Coded with no exceptions.  Examples: ProcCode (Dxxxx) or TreatmentArea like tooth^surface */
			CNE,
			/** Composite price.  Example: 125.00 */
			CP,
			/** Coded with exceptions.  Example: Race: American Indian or Alaska Native,Asian,Black or African American,Native Hawaiian or Other Pacific,White, Hispanic,Other Race. */
			CWE,
			/** Extended composite ID with check digit.  Example: patient.PatNum or patient.ChartNumber or appointment.AptNum. */
			CX,
			/** Date.  yyyyMMdd.  4,6,or 8 */
			DT,
			/** Date/Time.  yyyyMMddHHmmss etc.  Allowed 4,6,8,10,12,14.  Possibly more, but unlikely. */
			DTM,
			/** Entity identifier.  Example: appointment.AptNum */
			EI,
			/** Hierarchic designator.  Application identifier.  Example: "OD" for OpenDental. */
			HD,
			/** Coded value for HL7 defined tables.  Must include TableId.  Example: 0003 is eCW's event type table id. */
			ID,
			/** Coded value for user-defined tables.  Example: Administrative Sex, F=Female, M=Male,U=Unknown. */
			IS,
			/** Message type.  Example: composed of messageType^eventType like DFT^P03 */
			MSG,
			/** Numeric.  Example: transaction quantity of 1.0 */
			NM,
			/** Processing type.  Examples: P-Production, T-Test. */
			PT,
			/** Sequence ID.  Example: for repeating segments number that begins with 1. */
			SI,
			/** String, alphanumeric.  Example: SSN or patient.FeeSchedule */
			ST,
			/** Timing quantity.  Example: for eCW appointment ^^duration^startTime^endTime like ^^1200^20120101112000^20120101114000 for 20 minute (1200 second) appointment starting at 11:20 on 01/01/2012 */
			TQ,
			/** Version identifier.  Example: 2.3 */
			VID,
			/** Extended address.  Example: Addr1^Addr2^City^State^Zip like 120 Main St.^Suite 3^Salem^OR^97302 */
			XAD,
			/** Extended composite ID number and name for person.  Example: provider.EcwID^provider.LName^provider.FName^provider.MI */
			XCN,
			/** Extended person name.  Composite data type.  Example: LName^FName^MI). */
			XPN,
			/** Extended telecommunication number.  Example: 5033635432 */
			XTN
		}


}