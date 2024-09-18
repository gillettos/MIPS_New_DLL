using TwinCAT.Ads;

namespace MyLibrary
{
    public class PlcCommunicator
    {
        private AdsClient adsClient;

        public PlcCommunicator()
        {
            adsClient = new AdsClient();
            adsClient.Connect("5.143.125.242.1.1", 851); // Replace with your PLC's NET address and port
        }

        public void ToggleBoolean(string variableName)
        {
            int variableHandle = (int)adsClient.CreateVariableHandle(variableName);
            bool currentValue = adsClient.ReadAny<bool>((uint)variableHandle);
            adsClient.WriteAny((uint)variableHandle, !currentValue);
            adsClient.DeleteVariableHandle((uint)variableHandle);
        }

        public int ReadInteger(string variableName)
        {
            int variableHandle = (int)adsClient.CreateVariableHandle(variableName);
            int value = adsClient.ReadAny<int>((uint)variableHandle);
            adsClient.DeleteVariableHandle((uint)variableHandle);
            return value;
        }





        // mc_motors_on (Enable all Axis)
        public int mc_motors_on()
        {

            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                client.Connect(remoteAmsNetId, remotePort);
                client.WriteValue("Main.fbAxis1.bEnable", true);  //
                //client.WriteValue("Main.fbAxis2.bEnable", true);  //
            }
            return 0;
        }



    

        ~PlcCommunicator()

        {
            adsClient.Dispose();
        }
    }

}
