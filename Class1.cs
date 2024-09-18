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

        ~PlcCommunicator()

        {
            adsClient.Dispose();
        }
    }
}
