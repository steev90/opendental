package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;

/** DO NOT MAKE CHANGES TO THIS FILE.  THEY WILL GET OVERWRITTEN BY THE CRUD. */
public class Printer {
		/** Primary key. */
		public int PrinterNum;
		/** FK to computer.ComputerNum.  This will be changed some day to refer to the computername, because it would make more sense as a key than a cryptic number. */
		public int ComputerNum;
		/** Enum:PrintSituation One of about 10 different situations where printing takes place.  If no printer object exists for a situation, then a default is used and a prompt is displayed. */
		public PrintSituation PrintSit;
		/** The name of the printer as set from the specified computer. */
		public String PrinterName;
		/** If true, then user will be prompted for printer.  Otherwise, print directly with little user interaction. */
		public boolean DisplayPrompt;

		/** Deep copy of object. */
		public Printer deepCopy() {
			Printer printer=new Printer();
			printer.PrinterNum=this.PrinterNum;
			printer.ComputerNum=this.ComputerNum;
			printer.PrintSit=this.PrintSit;
			printer.PrinterName=this.PrinterName;
			printer.DisplayPrompt=this.DisplayPrompt;
			return printer;
		}

		/** Serialize the object into XML. */
		public String serialize() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Printer>");
			sb.append("<PrinterNum>").append(PrinterNum).append("</PrinterNum>");
			sb.append("<ComputerNum>").append(ComputerNum).append("</ComputerNum>");
			sb.append("<PrintSit>").append(PrintSit.ordinal()).append("</PrintSit>");
			sb.append("<PrinterName>").append(Serializing.escapeForXml(PrinterName)).append("</PrinterName>");
			sb.append("<DisplayPrompt>").append((DisplayPrompt)?1:0).append("</DisplayPrompt>");
			sb.append("</Printer>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void deserialize(Document doc) throws Exception {
			try {
				if(Serializing.getXmlNodeValue(doc,"PrinterNum")!=null) {
					PrinterNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"PrinterNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"ComputerNum")!=null) {
					ComputerNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"ComputerNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"PrintSit")!=null) {
					PrintSit=PrintSituation.values()[Integer.valueOf(Serializing.getXmlNodeValue(doc,"PrintSit"))];
				}
				if(Serializing.getXmlNodeValue(doc,"PrinterName")!=null) {
					PrinterName=Serializing.getXmlNodeValue(doc,"PrinterName");
				}
				if(Serializing.getXmlNodeValue(doc,"DisplayPrompt")!=null) {
					DisplayPrompt=(Serializing.getXmlNodeValue(doc,"DisplayPrompt")=="0")?false:true;
				}
			}
			catch(Exception e) {
				throw e;
			}
		}

		/**  */
		public enum PrintSituation {
			/** 0- Covers any printing situation not listed separately. */
			Default,
			/**  */
			Statement,
			/**  */
			LabelSingle,
			/**  */
			Claim,
			/** TP and perio */
			TPPerio,
			/**  */
			Rx,
			/**  */
			LabelSheet,
			/**  */
			Postcard,
			/**  */
			Appointments,
			/**  */
			RxControlled,
			/**  */
			Receipt
		}


}
