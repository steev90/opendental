using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeBase{
    public class TopazWrapper{

        public static Control GetTopaz(){
            return new Topaz.SigPlusNET();
        }

        public static void ClearTopaz(Control topaz){
            ((Topaz.SigPlusNET)topaz).ClearTablet();
        }

        public static void SetTopazCompressionMode(Control topaz,int compressionMode){
            ((Topaz.SigPlusNET)topaz).SetSigCompressionMode(compressionMode);
        }

        public static void SetTopazEncryptionMode(Control topaz,int encryptionMode){
            ((Topaz.SigPlusNET)topaz).SetEncryptionMode(encryptionMode);
        }

        public static void SetTopazKeyString(Control topaz,string str){
            ((Topaz.SigPlusNET)topaz).SetKeyString(str);
        }

        public static void SetTopazAutoKeyData(Control topaz,string data){
            ((Topaz.SigPlusNET)topaz).SetAutoKeyData(data);
        }

        public static void SetTopazSigString(Control topaz,string signature){
            ((Topaz.SigPlusNET)topaz).SetSigString(signature);
        }

        public static void SetTopazState(Control topaz,int state){
            ((Topaz.SigPlusNET)topaz).SetTabletState(state);
        }

        public static int GetTopazNumberOfTabletPoints(Control topaz){
            return ((Topaz.SigPlusNET)topaz).NumberOfTabletPoints();
        }

        public static string GetTopazString(Control topaz){
            return ((Topaz.SigPlusNET)topaz).GetSigString();
        }

    }
}
