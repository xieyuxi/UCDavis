using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Add PowerShell System Automation Service
using System.Management.Automation;
using System.Collections.ObjectModel;
// Add System Collections to Output shell pipeline
using System.Collections.ObjectModel;
// Add File Stream
using System.IO;
// Background activities
using System.Threading;

namespace AutomationService
{
    class KeyManager
    {
        private String vaultName;
        private String location;
        private String userPrivateKey;

        public KeyManager(String vaultName, String location){
            this.vaultName = vaultName;
            this.location = location;
            this.userPrivateKey = null;
        }
        
        public void configure_keyvault_environment(String resourceGroup){
            // Create PowerShell instance for an empty pipeline
            using (PowerShell PowerShellInstance = PowerShell.Create()){
                // Notify errors
                PowerShellInstance.Streams.Error.DataAdded += Error_DataAdded;

                // Sign-in the user to Azure 
                PowerShellInstance.AddScript("Add-AzureAccount");
                // Create Resource Manager for this application
                PowerShellInstance.AddScript("Switch-AzureMode AzureResourceManager");
                PowerShellInstance.AddScript("New-AzureResourceGroup -Name '" + resourceGroup + "' -Location '" + location + "'");
                // Create a Key Vault
                PowerShellInstance.AddScript("New-AzureKeyVault -VaultName '" + vaultName + "' -ResourceGroupName '" + resourceGroup + "' -Location '" + location + "'");
               
                //Begin execution on the pipeline
                IAsyncResult result = PowerShellInstance.BeginInvoke();

                // While waiting for the execution to be completed
                while(result.IsCompleted == false){
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Execution has stopped. The pipeline state: " + PowerShellInstance.InvocationStateInfo.State);  
            }
        }

        public void create_user_key( String username){
            using (PowerShell PowerShellInstance = PowerShell.Create()){
                // Prepare a collection to store the user's key
                PSDataCollection<PSObject> privateKey = new PSDataCollection<PSObject>();
                privateKey.DataAdded += privateKey_DataAdded;

                PowerShellInstance.AddScript("$key = Add-AzureKeyVaultKey -VaultName '" + vaultName + "' -Name 'UCDavis" + username + "' -Destination 'Software'");
                //Begin execution on the pipeline
                IAsyncResult user_key = PowerShellInstance.BeginInvoke<PSObject>(privateKey);

                // While waiting for the execution to be completed
                while (user_key.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Execution has stopped. The pipeline state: " + PowerShellInstance.InvocationStateInfo.State);
                // Set user's private key
                this.userPrivateKey = privateKey.ToString();
            }
        }

        // Check user's Vault
        public Boolean check_user(String user_id, String vault_name){
            Boolean userHasKey = false;

            // Write your code here...

            return userHasKey;
        }
        
        public String get_user_key(){
            return this.userPrivateKey;
        }

        // Encrypt Key
        public void encrypt_stream(String key, FileStream rawDataStream){

            // Write your code here...
        }

        void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            Console.WriteLine("Sorry, there was an error");
        }

        void privateKey_DataAdded(object sender, DataAddedEventArgs e)
        {
            Console.WriteLine("Private Key added to output");
        }

    }
}
