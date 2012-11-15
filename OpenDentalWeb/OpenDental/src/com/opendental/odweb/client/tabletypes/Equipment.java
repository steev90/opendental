package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

public class Equipment {
		/** Primary key. */
		public int EquipmentNum;
		/** Short description, need not be very unique. */
		public String Description;
		/** Must be unique among all pieces of equipment.  Auto-generated 3 char alpha numeric gives 1.5M unique serial numbers.  Zero never part of autogenerated serial number. */
		public String SerialNumber;
		/** Limit 2 char. */
		public String ModelYear;
		/** Date when this corporation obtained the equipment.  Always has a valid value. */
		public Date DatePurchased;
		/** Normally 01-01-0001 if equipment still in possession.  Once sold, a date will be present. */
		public Date DateSold;
		/** . */
		public double PurchaseCost;
		/** . */
		public double MarketValue;
		/** Freeform text. */
		public String Location;
		/** Security uses this date to lock older entries from accidental deletion.  Date, no time. */
		public Date DateEntry;

		/** Deep copy of object. */
		public Equipment Copy() {
			Equipment equipment=new Equipment();
			equipment.EquipmentNum=this.EquipmentNum;
			equipment.Description=this.Description;
			equipment.SerialNumber=this.SerialNumber;
			equipment.ModelYear=this.ModelYear;
			equipment.DatePurchased=this.DatePurchased;
			equipment.DateSold=this.DateSold;
			equipment.PurchaseCost=this.PurchaseCost;
			equipment.MarketValue=this.MarketValue;
			equipment.Location=this.Location;
			equipment.DateEntry=this.DateEntry;
			return equipment;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Equipment>");
			sb.append("<EquipmentNum>").append(EquipmentNum).append("</EquipmentNum>");
			sb.append("<Description>").append(Serializing.EscapeForXml(Description)).append("</Description>");
			sb.append("<SerialNumber>").append(Serializing.EscapeForXml(SerialNumber)).append("</SerialNumber>");
			sb.append("<ModelYear>").append(Serializing.EscapeForXml(ModelYear)).append("</ModelYear>");
			sb.append("<DatePurchased>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DatePurchased)).append("</DatePurchased>");
			sb.append("<DateSold>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateSold)).append("</DateSold>");
			sb.append("<PurchaseCost>").append(PurchaseCost).append("</PurchaseCost>");
			sb.append("<MarketValue>").append(MarketValue).append("</MarketValue>");
			sb.append("<Location>").append(Serializing.EscapeForXml(Location)).append("</Location>");
			sb.append("<DateEntry>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateEntry)).append("</DateEntry>");
			sb.append("</Equipment>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				EquipmentNum=Integer.valueOf(doc.getElementsByTagName("EquipmentNum").item(0).getFirstChild().getNodeValue());
				Description=doc.getElementsByTagName("Description").item(0).getFirstChild().getNodeValue();
				SerialNumber=doc.getElementsByTagName("SerialNumber").item(0).getFirstChild().getNodeValue();
				ModelYear=doc.getElementsByTagName("ModelYear").item(0).getFirstChild().getNodeValue();
				DatePurchased=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(doc.getElementsByTagName("DatePurchased").item(0).getFirstChild().getNodeValue());
				DateSold=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(doc.getElementsByTagName("DateSold").item(0).getFirstChild().getNodeValue());
				PurchaseCost=Double.valueOf(doc.getElementsByTagName("PurchaseCost").item(0).getFirstChild().getNodeValue());
				MarketValue=Double.valueOf(doc.getElementsByTagName("MarketValue").item(0).getFirstChild().getNodeValue());
				Location=doc.getElementsByTagName("Location").item(0).getFirstChild().getNodeValue();
				DateEntry=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(doc.getElementsByTagName("DateEntry").item(0).getFirstChild().getNodeValue());
			}
			catch(Exception e) {
				throw e;
			}
		}


}
